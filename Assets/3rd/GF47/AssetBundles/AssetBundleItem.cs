/***************************************************************
 * @File Name       : AssetBundleItem
 * @Author          : GF47
 * @Description     : TODO what's the use of the [AssetBundleItem]
 * @Date            : 2017/7/31/星期一 15:57:09
 * @Edit            : none
 **************************************************************/

using System.Collections;
using System.IO;
using GF47RunTime;
using UnityEngine;

namespace Assets
{
    public class AssetBundleItem
    {
        public string path;
        public AssetBundle ab;
        public int referenceCount;

        /// <summary>
        /// 初始化ABItem
        /// </summary>
        /// <param name="path">包的路径，可能存在persistent文件夹中，也可能存在streaming文件夹中</param>
        /// <param name="isAsync">是否异步读取</param>
        public AssetBundleItem(string path, bool isAsync = false)
        {
            this.path = path;

            string nativePath = ABConfig.AssetbundleRoot_Hotfix + "/" + this.path;
            if (!File.Exists(nativePath)) { nativePath = ABConfig.AssetbundleRoot_Streaming_AsFile + "/" + this.path; }

            if (isAsync)
            {
                Coroutines.StartACoroutine(GetAssetBundleAsync(nativePath));
            }
            else
            {
                ab = AssetBundle.LoadFromFile(nativePath);
            }
        }

        private IEnumerator GetAssetBundleAsync(string nativePath)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(nativePath);
            yield return request;
            ab = request.assetBundle;
        }

        public void Unload(bool force)
        {
            if (referenceCount < 1)
            {
                ab.Unload(force);
            }
        }
    }
}
