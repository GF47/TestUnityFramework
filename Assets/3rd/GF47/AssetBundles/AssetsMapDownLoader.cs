using System;
using System.Diagnostics.SymbolStore;
using System.IO;

namespace Assets
{
    public class AssetsMapDownLoader
    {
        public int Progress { get { return _downLoader.percent; } }
        public bool IsDone { get; private set; }

        private HttpAsyncDownLoader _downLoader;
        private int _retryNumber;

        private string _url;
        private string _nativePath;

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
            _downLoader = new HttpAsyncDownLoader(_url, _nativePath, Callback);
            _downLoader.Start();
        }

        private void Callback(bool b)
        {
            IsDone = b;
            if (!IsDone)
            {
                _retryNumber++;
                if (_retryNumber < 2)
                {
                    Start();
                }
                else
                {
                    IsDone = true;
                    if (File.Exists(_nativePath))
                    {
                        File.Delete(_nativePath);
                    }
                    //throw new Exception(_nativePath + "下载失败");
                }
            }
        }
    }
}
