/***************************************************************
 * @File Name       : AssetBundlesUpdater
 * @Author          : GF47
 * @Description     : TODO what's the use of the [AssetBundlesUpdater]
 * @Date            : 2017/8/1/星期二 11:11:10
 * @Edit            : none
 **************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using GF47RunTime;
using UnityEngine;

namespace Assets
{
    public class AssetBundlesUpdater
    {
        public int Progress
        {
            get
            {
                int value = 0;
                for (int i = 0; i < _downLoaders.Count; i++)
                {
                    value += _downLoaders[i].Progress;
                }
                value /= _downLoaders.Count;
                return value;
            }
        }

        public bool IsDone
        {
            get
            {
                for (int i = 0; i < _downLoaders.Count; i++)
                {
                    if (!_downLoaders[i].IsDone) { return false; }
                }
                return true;
            }

        }

        private List<AssetBundleDownLoader> _downLoaders;


        public AssetBundlesUpdater()
        {
            if (AssetsMap.Instance.IsStreamingAssets) { return; }

            KeyValuePair<string, string>[] abArray = AssetsMap.Instance.assetbundles;

            _downLoaders  = new List<AssetBundleDownLoader>(abArray.Length + 1);

            CheckIfShouldUpdate(AssetsMap.Instance.manifest.Key, AssetsMap.Instance.manifest.Value);

            for (int i = 0; i < abArray.Length; i++)
            {
                CheckIfShouldUpdate(abArray[i].Key, abArray[i].Value);
            }
        }

        private void CheckIfShouldUpdate(string abName, string md5)
        {
            bool shouldUpdate = true;
            string nativePath = ABConfig.AssetbundleRoot_Hotfix + "/" + abName;

            if (File.Exists(nativePath))
            {
                string nativeMD5 = FileUtility.GetFileHash(nativePath);
                if (string.Equals(md5, nativeMD5)) { shouldUpdate = false; }
            }
            if (shouldUpdate)
            {
                _downLoaders.Add(new AssetBundleDownLoader(abName));
            }
        }
    }
}
