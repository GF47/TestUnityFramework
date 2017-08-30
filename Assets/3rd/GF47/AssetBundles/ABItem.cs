/***************************************************************
 * @File Name       : ABItem
 * @Author          : GF47
 * @Description     : 保存了ab包的路径，ab包本身和引用计数，当引用计数大于0时，不会自动卸载这个ab包
 * @Date            : 2017/7/31/星期一 15:57:09
 * @Edit            : none
 **************************************************************/

using System.Collections;
using System.IO;
using GF47RunTime;
using UnityEngine;

namespace Assets
{
    public class ABItem
    {
        public string path;
        public AssetBundle ab;
        public int referenceCount;

        /// <summary>
        /// 初始化ABItem
        /// </summary>
        /// <param name="path">包的路径，可能存在persistent文件夹中，也可能存在streaming文件夹中</param>
        /// <param name="isAsync">是否异步读取</param>
        public ABItem(string path, bool isAsync = false)
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
            if (force)
            {
                ab.Unload(true);
                return;
            }

            if (referenceCount < 1) { ab.Unload(false); }
        }
    }
}
