using System;
using System.Collections;
using System.Collections.Generic;
using Assets;
using GF47RunTime;
using UnityEngine;
using Object = UnityEngine.Object;

public class Test : MonoBehaviour
{

    void Start()
    {
        AssetBundlesManager.ConstructFunc = () => new AssetBundlesManager();
        _abUpdater = new ABUpdater();
        _abUpdater.Init();
    }

    private ABUpdater _abUpdater;


    // Update is called once per frame
    void Update()
    {
        if (!_abUpdater.keepWaiting)
        {
            try
            {

                AssetBundlesManager.Instance.Init();

                string assetName = "Assets/~Temp/Prefabs/tea_pot.prefab";
                ABItem item = AssetBundlesManager.Instance.BeginLoadABContain(assetName);
                GameObject go = item.ab.LoadAsset<GameObject>(assetName);
                Object.Instantiate(go, Vector3.zero, Quaternion.identity);
                AssetBundlesManager.Instance.EndLoad(true);

                assetName = "Assets/~Temp/Prefabs/box.prefab";
                item = AssetBundlesManager.Instance.BeginLoadABContain(assetName);
                go = item.ab.LoadAsset<GameObject>(assetName);
                Object.Instantiate(go, -Vector3.one, Quaternion.identity);
                AssetBundlesManager.Instance.EndLoad(true);

                assetName = "Assets/~Temp/Prefabs/@Sphere.prefab";
                item = AssetBundlesManager.Instance.BeginLoadABContain(assetName);
                go = item.ab.LoadAsset<GameObject>(assetName);
                Object.Instantiate(go, Vector3.one, Quaternion.identity);
                AssetBundlesManager.Instance.EndLoad(false);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            Destroy(this);
        }
    }
}
