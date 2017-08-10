/***************************************************************
 * @File Name       : MonoBased
 * @Author          : GF47
 * @Description     : 扩展框架的管理器类，自定义的管理器需要从这里添加，然后所有的继承自 MonoBased 的类都可以访问到了。
 * @Date            : 2017/8/4/星期五 14:36:48
 * @Edit            : none
 **************************************************************/

using Assets;

public partial class MonoBased
{
    /***************************************************************
     * 自己添加的继承自IManager的管理类
     **************************************************************/

    private AssetBundlesManager _abManager;

    // private LuaManager m_LuaMgr;
    // private ResourceManager m_ResMgr;
    // private NetworkManager m_NetMgr;
    // private SoundManager m_SoundMgr;
    // private TimerManager m_TimerMgr;
    // private ThreadManager m_ThreadMgr;
    // private ObjectPoolManager m_ObjectPoolMgr;

    /***************************************************************
     * 子类的访问接口
     **************************************************************/

    protected AssetBundlesManager ABManager
    {
        get
        {
            if (_abManager == null)
            {
                _abManager = facade.GetManager<AssetBundlesManager>(ManagerName.ASSET_BUNDLES_MANAGER);
            }
            return _abManager;
        }
    }


    /*
    protected LuaManager LuaManager {
        get {
            if (m_LuaMgr == null) {
                m_LuaMgr = facade.GetManager<LuaManager>(ManagerName.Lua);
            }
            return m_LuaMgr;
        }
    }

    protected ResourceManager ResManager {
        get {
            if (m_ResMgr == null) {
                m_ResMgr = facade.GetManager<ResourceManager>(ManagerName.Resource);
            }
            return m_ResMgr;
        }
    }

    protected NetworkManager NetManager {
        get {
            if (m_NetMgr == null) {
                m_NetMgr = facade.GetManager<NetworkManager>(ManagerName.Network);
            }
            return m_NetMgr;
        }
    }

    protected SoundManager SoundManager {
        get {
            if (m_SoundMgr == null) {
                m_SoundMgr = facade.GetManager<SoundManager>(ManagerName.Sound);
            }
            return m_SoundMgr;
        }
    }

    protected TimerManager TimerManager {
        get {
            if (m_TimerMgr == null) {
                m_TimerMgr = facade.GetManager<TimerManager>(ManagerName.Timer);
            }
            return m_TimerMgr;
        }
    }

    protected ThreadManager ThreadManager {
        get {
            if (m_ThreadMgr == null) {
                m_ThreadMgr = facade.GetManager<ThreadManager>(ManagerName.Thread);
            }
            return m_ThreadMgr;
        }
    }

    protected ObjectPoolManager ObjPoolManager {
        get {
            if (m_ObjectPoolMgr == null) {
                m_ObjectPoolMgr = facade.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
            }
            return m_ObjectPoolMgr;
        }
    }
    */

}