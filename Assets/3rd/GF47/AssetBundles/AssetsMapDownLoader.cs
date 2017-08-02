using System;
using System.IO;
using UnityEngine;

namespace Assets
{
    public class AssetsMapDownLoader : CustomYieldInstruction
    {
        public int Progress { get { return _downLoader.percent; } }
        public override bool keepWaiting { get { return !_isDone; } }
        private bool _isDone;

        private HttpAsyncDownLoader _downLoader;
        private int _retryNumber;

        private readonly string _url;
        private readonly string _nativePath;

        public AssetsMapDownLoader()
        {
            _url = ABConfig.SERVER_URL + "/" + ABConfig.PLATFORM + "/" + ABConfig.NAME_ASSETSMAP;
            _nativePath = ABConfig.AssetbundleRoot_Hotfix + "/" + ABConfig.NAME_ASSETSMAP;

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
