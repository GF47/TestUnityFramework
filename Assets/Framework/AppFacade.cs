public class AppFacade : Facade
{
    private static AppFacade _instance;

    public static AppFacade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppFacade();
                _instance.InitFramework();
            }
            return _instance;
        }
    }

    public override void InitFramework()
    {
        base.InitFramework();
        RegisterCommand(NotiConst.START_UP, typeof(StartUpCommand));
    }

    /// <summary>
    /// 启动框架
    /// </summary>
    public void StartUp()
    {
        SendMessageCommand(NotiConst.START_UP);
        RemoveMultiCommand(NotiConst.START_UP);
    }
}

