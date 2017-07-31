using System.Collections;
using System.Collections.Generic;
using System.IO;
using GF47RunTime;
using UnityEngine;
using SimpleJSON;

namespace Assets
{
    public class AssetsMap : Singleton<AssetsMap>
    {
        public string serverAddress;
        public int version;
        public string[] assetbundles;
        public Dictionary<string, int> assets;

        public AssetsMap()
        {
            string nativePath = ABConfig.AssetbundleRoot_Hotfix + "/" + ABConfig.NAME_ASSETSMAP; // 首先读取persistentData文件夹，是否有map文件

            if (File.Exists(nativePath))
            {
                ReadJson(File.ReadAllText(nativePath));
            }
            else
            {
                nativePath = ABConfig.AssetbundleRoot_Streaming_AsWWW + "/" + ABConfig.NAME_ASSETSMAP; // 读取StreamingAssets的jar包，是否有map文件
                
                Debug.Log(nativePath);

                Coroutines.StartACoroutine(GetJson(nativePath));
            }

        }

        private IEnumerator GetJson( string nativePath)
        {
            WWW www = new WWW(nativePath);
            yield return www;
            ReadJson(www.text);
            www.Dispose();
            www = null;
        }

        private void ReadJson(string jsonStr)
        {
            if (!string.IsNullOrEmpty(jsonStr))
            {
                JSONObject assetsMap = JSON.Parse(jsonStr).AsObject;
                serverAddress = assetsMap[ABConfig.KEY_SERVER];
                version = assetsMap[ABConfig.KEY_VERSION];
                JSONObject assetBundleJsonObjects = assetsMap[ABConfig.KEY_ASSETBUNDLES].AsObject;
                JSONObject assetJsonObjects = assetsMap[ABConfig.KEY_ASSETS].AsObject;

                assetbundles = new string[assetBundleJsonObjects.Count];
                foreach (KeyValuePair<string, JSONNode> pair in assetBundleJsonObjects)
                {
                    assetbundles[pair.Value] = pair.Key;
                }
                assets = new Dictionary<string, int>(assetJsonObjects.Count);
                foreach (KeyValuePair<string, JSONNode> pair in assetJsonObjects)
                {
                    assets.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}
