using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class Test : MonoBehaviour {

    void Awake()
    {
        AssetBundlesManager.ConstructFunc = () => new AssetBundlesManager();
    }

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AssetBundlesManager.Instance.GetAssetsMapFromServer();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            AssetBundlesManager.Instance.UpdateAssetBundles();
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            AssetBundlesManager.Instance.GetManifest();
        }

        Debug.Log(AssetBundlesManager.Instance.GetUpdateProgress() + AssetBundlesManager.Instance.GetUpdateState().ToString());

        if (Input.GetKeyUp(KeyCode.C))
        {
            int abID = AssetsMap.Instance.assets["Assets/~Temp/Prefabs/tea_pot.prefab"];
            string abName = AssetsMap.Instance.assetbundles[abID].Key;

            string path = AssetsMap.Instance.IsStreamingAssets
                ? ABConfig.AssetbundleRoot_Streaming_AsFile
                : ABConfig.AssetbundleRoot_Hotfix;

            AssetBundle ab = AssetBundle.LoadFromFile(path + "/" + abName);
            GameObject go = ab.LoadAsset<GameObject>("Assets/~Temp/Prefabs/tea_pot.prefab");
            Object.Instantiate(go, Vector3.zero, Quaternion.identity);
            ab.Unload(false);
        }
	}
}
