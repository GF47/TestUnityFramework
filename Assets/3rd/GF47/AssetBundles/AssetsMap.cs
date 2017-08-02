using System.Collections;
using System.Collections.Generic;
using System.IO;
using GF47RunTime;
using UnityEngine;
using SimpleJSON;

namespace Assets
{
    public class AssetsMap  : CustomYieldInstruction
    {
        public static AssetsMap Instance { get { return _instance; } }
        private static AssetsMap _instance;

        public string serverAddress;
        public int version;
        public KeyValuePair<string, string> manifest;
        public KeyValuePair<string, string>[] assetbundles;
        public Dictionary<string, int> assets;

        public override bool keepWaiting { get { return !_isDone; } }
        private bool _isDone;

        public bool IsStreamingAssets { get; private set; }

        public AssetsMap()
        {
            _instance = this;

            string nativePath = ABConfig.AssetbundleRoot_Hotfix + "/" + ABConfig.NAME_ASSETSMAP; // 首先读取persistentData文件夹，是否有map文件

            if (File.Exists(nativePath))
            {
                ReadJson(File.ReadAllText(nativePath));
                _isDone = true;
            }
            else
            {
                IsStreamingAssets = true;

                nativePath = ABConfig.AssetbundleRoot_Streaming_AsWWW + "/" + ABConfig.NAME_ASSETSMAP; // 读取StreamingAssets的jar包，是否有map文件

                Coroutines.StartACoroutine(GetJson(nativePath));
            }
        }

        private IEnumerator GetJson( string nativePath)
        {
            WWW www = new WWW(nativePath);
            yield return www;
            ReadJson(www.text);
            www.Dispose();
            www = null;

            _isDone = true;
        }

        private void ReadJson(string jsonStr)
        {
            if (!string.IsNullOrEmpty(jsonStr))
            {
                JSONObject assetsMap = JSON.Parse(jsonStr).AsObject;
                serverAddress = assetsMap[ABConfig.KEY_SERVER];
                version = assetsMap[ABConfig.KEY_VERSION];
                JSONObject manifestJsonObject = assetsMap[ABConfig.KEY_MANIFEST].AsObject;
                JSONObject assetBundleJsonObjects = assetsMap[ABConfig.KEY_ASSETBUNDLES].AsObject;
                JSONObject assetJsonObjects = assetsMap[ABConfig.KEY_ASSETS].AsObject;

                foreach (KeyValuePair<string, JSONNode> pair in manifestJsonObject)
                {
                    manifest = new KeyValuePair<string, string>(pair.Key, pair.Value);
                    break;
                }

                assetbundles = new KeyValuePair<string, string>[assetBundleJsonObjects.Count];
                int index = 0;
                foreach (KeyValuePair<string, JSONNode> pair in assetBundleJsonObjects)
                {
                    assetbundles[index] = new KeyValuePair<string, string>(pair.Key, pair.Value);
                    index++;
                }

                assets = new Dictionary<string, int>(assetJsonObjects.Count);
                foreach (KeyValuePair<string, JSONNode> pair in assetJsonObjects)
                {
                    assets.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}
