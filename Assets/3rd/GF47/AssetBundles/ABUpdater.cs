/***************************************************************
 * @File Name       : ABUpdater
 * @Author          : GF47
 * @Description     : 更新ab包，只需要new一下然后等待yield就行
 * @Date            : 2017/8/1/星期二 11:11:10
 * @Edit            : none
 **************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using GF47RunTime;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets
{
    public class ABUpdater : CustomYieldInstruction
    {
        public int Progress
        {
            get
            {
                if (_isUpdateFinished) { return 100; }

                if (_downLoaders == null) { return 0; }
                if (_downLoaders.Count == 0) { return 100; }

                int value = 0;
                for (int i = 0; i < _downLoaders.Count; i++)
                {
                    value += _downLoaders[i].Progress;
                }
                value /= _downLoaders.Count;
                return value;
            }
        }
        public override bool keepWaiting
        {
            get
            {
                if (_isUpdateFinished) { return false; }
                if (_downLoaders == null) { return true; }
                for (int i = 0; i < _downLoaders.Count; i++)
                {
                    if (_downLoaders[i].keepWaiting) { return true; }
                }
                return false;
            }

        }

        private bool _isUpdateFinished;

        private List<ABDownLoader> _downLoaders;

        public ABUpdater()
        {
            AssetBundlesManager.ConstructFunc = () => new AssetBundlesManager();
            Coroutines.StartACoroutine(__Init());
        }

        private IEnumerator __Init()
        {
            yield return new AssetsMapDownLoader();
            yield return new AssetsMap();

            if (AssetsMap.Instance.IsStreamingAssets)
            {
                _isUpdateFinished = true;
                yield break;
            }

            KeyValuePair<string, string>[] abArray = AssetsMap.Instance.assetbundles;

            _downLoaders  = new List<ABDownLoader>(abArray.Length + 1);

            CheckIfShouldUpdate(AssetsMap.Instance.manifest.Key, AssetsMap.Instance.manifest.Value);

            for (int i = 0; i < abArray.Length; i++)
            {
                CheckIfShouldUpdate(abArray[i].Key, abArray[i].Value);
            }

            yield return this;
            Assert.IsFalse(keepWaiting);

            _isUpdateFinished = true;
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
                _downLoaders.Add(new ABDownLoader(abName));
            }
        }

    }
}
