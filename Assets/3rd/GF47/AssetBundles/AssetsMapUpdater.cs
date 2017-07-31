using System.IO;

namespace Assets
{
    public class AssetsMapUpdater
    {
        public AssetsMapUpdater()
        {
            string url = ABConfig.SERVER_URL + "/" + ABConfig.PLATFORM + "/" + ABConfig.NAME_ASSETSMAP;
            string nativePath = ABConfig.AssetbundleRoot_Hotfix + "/" + ABConfig.NAME_ASSETSMAP;
            if (File.Exists(nativePath))
            {
                File.Delete(nativePath);
            }
            HttpAsyncDownLoader downLoader = new HttpAsyncDownLoader(url, nativePath);
            downLoader.Start();

            AssetsMap.ConstructFunc = () => new AssetsMap(); 
        }
    }
}
