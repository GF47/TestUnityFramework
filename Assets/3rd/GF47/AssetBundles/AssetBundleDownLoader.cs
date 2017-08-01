/***************************************************************
 * @File Name       : AssetBundleDownLoader
 * @Author          : GF47
 * @Description     : TODO what's the use of the [AssetBundleDownLoader]
 * @Date            : 2017/7/31/星期一 15:50:39
 * @Edit            : none
 **************************************************************/

using System.IO;

namespace Assets
{
    public class AssetBundleDownLoader
    {
        public int Progress
        {
            get
            {
                if (IsDone) { return 100; }
                if (_downLoader == null) { return 0; }
                return _downLoader.percent;
            }
        }

        public bool IsDone { get; private set; }


        private HttpAsyncDownLoader _downLoader;
        private int _retryNumber;

        private string _url;
        private string _nativePath;

        public AssetBundleDownLoader(string abPath)
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

            _downLoader = new HttpAsyncDownLoader(_url, _nativePath, Callback);
            _downLoader.Start();
        }

        private void Callback(bool b)
        {
            IsDone = b;
            if (!IsDone)
            {
                _retryNumber++;
                if (_retryNumber < 2) // 尝试次数
                {
                    Start();
                }
                else
                {
                    IsDone = true;
                    // throw new Exception(_nativePath + "下载失败");
                }
            }
        }
    }
}
