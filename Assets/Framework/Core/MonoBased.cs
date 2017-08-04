using UnityEngine;
using System.Collections.Generic;

public partial class MonoBased : MonoBehaviour
{
    private AppFacade _facade;

    /// <summary>
    /// 注册消息
    /// </summary>
    protected void RegisterMessage(IView view, IList<string> messages)
    {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RegisterViewCommand(view, messages);
    }

    /// <summary>
    /// 移除消息
    /// </summary>
    protected void RemoveMessage(IView view, IList<string> messages)
    {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RemoveViewCommand(view, messages);
    }

    protected AppFacade facade
    {
        get
        {
            if (_facade == null)
            {
                _facade = AppFacade.Instance;
            }
            return _facade;
        }
    }

}
