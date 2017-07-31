using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class Test : MonoBehaviour {

    void Awake()
    {
    }

	// Use this for initialization
	void Start () {
        AssetsMapUpdater updater = new AssetsMapUpdater();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log(AssetsMap.Instance.version);
        }
	}
}
