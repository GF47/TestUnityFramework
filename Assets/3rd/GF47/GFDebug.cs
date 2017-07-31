/***************************************************************
 * @File Name       : GFDebug
 * @Author          : GF47
 * @Description     : 一个简单的Log
 * @Date            : 2017/5/16/星期二 10:11:56
 * @Edit            : none
 **************************************************************/

using System;
using System.Collections;
using UnityEngine;

namespace GF47RunTime
{
    public class GFDebug : MonoBehaviour
    {
        public static Vector2 Size;
        public static Vector2 Pos;
        public static Vector2 FPSSize;

        private static readonly GFDebug Instance;

        static GFDebug()
        {
            int screenW = Screen.width;
            int screenH = Screen.height;

            Size = new Vector2(screenW / 2 , screenH / 16);
            Pos = new Vector2(16f, 16f);
            FPSSize = new Vector2(screenW / 4, screenH / 16);

            if (Instance == null)
            {
                GameObject go = new GameObject("__GFDebug")
                {
                    hideFlags = HideFlags.HideInHierarchy,
                };
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<GFDebug>();
                _infos = new string[4];
            }
        }

        private static string[] _infos;
        public static int Capacity
        {
            get { return _infos.Length; }
            set
            {
                if (value == 0) { return; }
                if (value == _infos.Length) { return; }

                Array.Resize(ref _infos, value);
            }
        }

        public static int Count = 4;

        private static int _current;
        private static int GetPrevious(int index)
        {
            index--;
            if (index < 0)
            {
                index += Capacity;
            }
            return index;
        }

        public static bool ShowLog { get { return Instance.enabled; } set { Instance.enabled = value; } }

        public static void Log(object o, float time)
        {
            Log(o);
            Instance.Execute(time, () => Instance.enabled = false);
        }

        public static void Log(object o)
        {
            _current++;
            if (_current >= Capacity)
            {
                _current = 0;
            }

            _infos[_current] = o.ToString();
            Instance.enabled = true;
        }

#region FPS
        public static float FPSRefreshDelta = 2f;
        public static bool ShowFPS
        {
            get { return Instance._showFPS; }
            set
            {
                if (Instance._showFPS != value)
                {
                    Instance._showFPS = value;
                    if (Instance._showFPS)
                    {
                        Instance._duration = 0f;
                        Instance._frameCount = 0;
                    }
                }
            }
        }

        private bool _showFPS = true;

        private float _duration;
        private int _frameCount;
        private float _fps;

        void Update()
        {
            if (_showFPS)
            {
                _frameCount += 1;
                _duration += Time.deltaTime;

                if (_duration > FPSRefreshDelta)
                {
                    _fps = _frameCount / _duration;
                    _duration = 0f;
                    _frameCount = 0;
                }
            }
        }
#endregion

        void OnGUI()
        {
            Rect r = new Rect(Pos.x, Pos.y, Size.x, Size.y);
            for (int i = 0, cursor = _current; i < Count; i++, cursor = GetPrevious(cursor), r = new Rect(r.x, r.y + Size.y, r.width, r.height))
            {
                if (!string.IsNullOrEmpty(_infos[cursor])) { GUI.Label(r, _infos[cursor]); }
            }
            if (_showFPS)
            {
                GUI.Label(new Rect(Screen.width - FPSSize.x - Pos.x, Pos.y, FPSSize.x, FPSSize.y), _fps.ToString("F2"));
            }
        }

        private void Execute(float time, Action action)
        {
            StartCoroutine(_timer(time, action));
        }

        private IEnumerator _timer(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            if (action != null) action();
        }

        // private class CircluarlyLinkedList<T>
        // {
        //     public class Node<T>
        //     {
        //         public Node<T> Next { get; set; }
        //         public T Value { get; set; }
        //     }

        //     public int CurrentIndex => _currentIndex;
        //     private int _currentIndex;

        //     public Node<T> Head { get; set; }
        //     public Node<T> Rear { get; set; }

        //     public CircluarlyLinkedList()
        //     {
        //         Head = new Node<T>();
        //         Rear = Head;
        //     }
        // }
    }
}
