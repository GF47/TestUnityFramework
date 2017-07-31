/***************************************************************
 * @File Name       : AssetbundlesUpdater
 * @Author          : GF47
 * @Description     : TODO what's the use of the [AssetbundlesUpdater]
 * @Date            : 2017/7/31/星期一 15:50:39
 * @Edit            : none
 **************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets
{
    public class AssetbundlesUpdater
    {
        public AssetbundlesUpdater(string abPath)
        {
            string url = ABConfig.SERVER_URL + "/" + ABConfig.PLATFORM + "/" + abPath;
            string nativePath = ABConfig.AssetbundleRoot_Hotfix + "/" + abPath;
            if (File.Exists(nativePath))
            {
                File.Delete(nativePath);
            }

            HttpAsyncDownLoader downLoader = new HttpAsyncDownLoader(url, nativePath);

            downLoader.Start();
        }
    }
}
