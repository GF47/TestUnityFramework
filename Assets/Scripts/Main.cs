/***************************************************************
 * @File Name       : Main
 * @Author          : GF47
 * @Description     : 程序入口
 * @Date            : 2017/8/5/星期六 11:13:30
 * @Edit            : none
 **************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        AppFacade.Instance.StartUp();
    }
}
