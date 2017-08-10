using System.Collections;
using Assets;
using GF47RunTime;
using UnityEngine;

public partial class StartUpCommand
{
    private ABUpdater _updater;

    public override void Execute(IMessage message)
    {
        // //-----------------关联命令-----------------------
        // AppFacade.Instance.RegisterCommand(NotiConst.DISPATCH_MESSAGE, typeof(SocketCommand));

        Coroutines.StartACoroutineWithCallback(UpdateAssetBundles(), InitManager);
    }

    IEnumerator UpdateAssetBundles()
    {
        _updater = new ABUpdater();
        yield return _updater;
    }

    void InitManager()
    {
        AppFacade.Instance.AddManager(ManagerName.ASSET_BUNDLES_MANAGER, AssetBundlesManager.Instance);

        // //-----------------初始化管理器-----------------------
        // AppFacade.Instance.AddManager<PanelManager>(ManagerName.Panel);
        // AppFacade.Instance.AddManager<SoundManager>(ManagerName.Sound);
        // AppFacade.Instance.AddManager<TimerManager>(ManagerName.Timer);
        // AppFacade.Instance.AddManager<NetworkManager>(ManagerName.Network);
        // AppFacade.Instance.AddManager<ResourceManager>(ManagerName.Resource);
        // AppFacade.Instance.AddManager<ThreadManager>(ManagerName.Thread);
        // AppFacade.Instance.AddManager<ObjectPoolManager>(ManagerName.ObjectPool);
        // AppFacade.Instance.AddManager<TestManager>(ManagerName.Game);

    }
}
