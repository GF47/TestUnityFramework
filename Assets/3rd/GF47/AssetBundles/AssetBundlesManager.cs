/***************************************************************
 * @File Name       : AssetBundlesManager
 * @Author          : GF47
 * @Description     : TODO what's the use of the [AssetBundlesManager]
 * @Date            : 2017/8/1/星期二 10:59:55
 * @Edit            : none
 **************************************************************/

using System;
using System.Collections.Generic;
using GF47RunTime;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    public class AssetBundlesManager : Singleton<AssetBundlesManager>
    {
        private AssetBundlesUpdater _assetBundlesUpdater;
        private bool _isUpdateDone;

        private AssetBundleManifest _manifest;

        public void GetAssetsMapFromServer()
        {
            AssetsMap.ConstructFunc = () => new AssetsMap();
            AssetsMapDownLoader downLoader = new AssetsMapDownLoader();
        }

        public void UpdateAssetBundles()
        {
            AssetBundlesUpdater updater = new AssetBundlesUpdater();
        }

        public int GetUpdateProgress()
        {
            if (_isUpdateDone) { return 100; }
            if (_assetBundlesUpdater == null) { return 0; }
            return _assetBundlesUpdater.Progress;
        }

        public bool GetUpdateState()
        {
            if (_assetBundlesUpdater != null)
            {
                _isUpdateDone = _assetBundlesUpdater.IsDone;
                if (_isUpdateDone)
                {
                    _assetBundlesUpdater = null;
                }
            }
            return _isUpdateDone;
        }

        public void GetManifest()
        {
            Debug.Log(AssetsMap.instance.manifest.Key);
            string abPath = (AssetsMap.instance.IsStreamingAssets
                ? ABConfig.AssetbundleRoot_Streaming_AsFile
                : ABConfig.AssetbundleRoot_Hotfix)
                + "/" + AssetsMap.instance.manifest.Key;
            AssetBundle ab = AssetBundle.LoadFromFile(abPath);
            _manifest = ab.LoadAsset<AssetBundleManifest>(ABConfig.MANIFEST_NAME);
            ab.Unload(false);
        }
    }
}
