﻿using UnityEngine;

public partial class StartUpCommand
{
    public override void Execute(IMessage message) {
        Debug.Log("test");
        // if (!Util.CheckEnvironment()) return;

        // GameObject gameMgr = GameObject.Find("GlobalGenerator");
        // if (gameMgr != null) {
        //     AppView appView = gameMgr.AddComponent<AppView>();
        // }
        // //-----------------关联命令-----------------------
        // AppFacade.Instance.RegisterCommand(NotiConst.DISPATCH_MESSAGE, typeof(SocketCommand));

        // //-----------------初始化管理器-----------------------
        AppFacade.Instance.AddManager<GameManager>(ManagerName.GAME_MANAGER);
        // AppFacade.Instance.AddManager<PanelManager>(ManagerName.Panel);
        // AppFacade.Instance.AddManager<SoundManager>(ManagerName.Sound);
        // AppFacade.Instance.AddManager<TimerManager>(ManagerName.Timer);
        // AppFacade.Instance.AddManager<NetworkManager>(ManagerName.Network);
        // AppFacade.Instance.AddManager<ResourceManager>(ManagerName.Resource);
        // AppFacade.Instance.AddManager<ThreadManager>(ManagerName.Thread);
        // AppFacade.Instance.AddManager<ObjectPoolManager>(ManagerName.ObjectPool);
        // AppFacade.Instance.AddManager<GameManager>(ManagerName.Game);
    }
}