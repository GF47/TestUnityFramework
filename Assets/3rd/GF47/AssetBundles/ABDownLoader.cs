/***************************************************************
 * @File Name       : ABDownLoader
 * @Author          : GF47
 * @Description     : ab包下载器
 * @Date            : 2017/7/31/星期一 15:50:39
 * @Edit            : none
 **************************************************************/

using System;
using System.IO;
using UnityEngine;

namespace Assets
{
    public class ABDownLoader : CustomYieldInstruction
    {
        public int Progress
        {
            get
            {
                if (_isDone) { return 100; }
                if (_downLoader == null) { return 0; }
                return _downLoader.percent;
            }
        }
        public override bool keepWaiting { get { return !_isDone; } }
        private bool _isDone;


        private HttpAsyncDownLoader _downLoader;
        private int _retryNumber;

        private readonly string _url;
        private readonly string _nativePath;

        public ABDownLoader(string abPath)
        {
            _url = ABConfig.SERVER_URL + "/" + ABConfig.PLATFORM + "/" + abPath;
            _nativePath = ABConfig.AssetbundleRoot_Hotfix + "/" + abPath;

            Start();
        }

        private void Start()
        {
            if (File.Exists(_nativePath))
            {
                File.Delete(_nativePath);
            }

            _downLoader = new HttpAsyncDownLoader(_url, _nativePath, ErrorCallback, DownLoadFinishedCallback);
            _downLoader.Start();
        }

        private void ErrorCallback(Exception e)
        {
            if (_retryNumber < 2)
            {
                _retryNumber++;
                Start();
            }
            else
            {
                _isDone = true;
                if (File.Exists(_nativePath))
                {
                    File.Delete(_nativePath);
                }
            }
        }

        private void DownLoadFinishedCallback(bool b)
        {
            _isDone = b;
        }
    }
}
