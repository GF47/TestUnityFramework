/***************************************************************
 * @File Name       : GameManager
 * @Author          : GF47
 * @Description     : TODO what's the use of the [GameManager]
 * @Date            : 2017/8/5/星期六 10:50:07
 * @Edit            : none
 **************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        AppFacade.Instance.RegisterCommand(NotiConst.CREATE_CUBE, typeof(CreateCubeCommand));

        // 测试
        AppFacade.Instance.SendMessageCommand(NotiConst.CREATE_CUBE);
        AppFacade.Instance.RemoveCommand(NotiConst.CREATE_CUBE);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            AppFacade.Instance.SendMessageCommand(NotiConst.CUBE_RED);
        }
        if (Input.GetMouseButtonUp(1))
        {
            AppFacade.Instance.SendMessageCommand(NotiConst.CUBE_BLUE);
        }
    }
}