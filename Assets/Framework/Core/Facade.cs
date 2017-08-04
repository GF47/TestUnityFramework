using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Facade
{
    private static readonly Dictionary<string, object> Managers = new Dictionary<string, object>();

    private GameObject AppGameManager
    {
        get
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager");
                if (_gameManager == null) { _gameManager = new GameObject("GameManager"); }
            }
            return _gameManager;
        }
    }
    private static GameObject _gameManager;

    protected IController controller;

    public virtual void InitFramework()
    {
        if (controller != null) return;
        controller = Controller.Instance;
    }

    public virtual void RegisterCommand(string commandName, Type commandType)
    {
        controller.RegisterCommand(commandName, commandType);
    }

    public virtual void RemoveCommand(string commandName)
    {
        controller.RemoveCommand(commandName);
    }

    public virtual bool HasCommand(string commandName)
    {
        return controller.HasCommand(commandName);
    }

    public void RegisterMultiCommand(Type commandType, params string[] commandNames)
    {
        RegisterCommandsList(commandType, commandNames);
    }

    public void RegisterCommandsList(Type commandType, IList<string> commandNames)
    {
        for (int i = 0; i < commandNames.Count; i++)
        {
            RegisterCommand(commandNames[i], commandType);
        }
    }

    public void RemoveMultiCommand(params string[] commandName)
    {
        RemoveCommandsList(commandName);
    }

    public void RemoveCommandsList(IList<string> commandName)
    {
        for (int i = 0; i < commandName.Count; i++)
        {
            RemoveCommand(commandName[i]);
        }
    }

    public void SendMessageCommand(string message, object body = null)
    {
        controller.ExecuteCommand(new Message(message, body));
    }

    /// <summary>
    /// 添加管理器
    /// </summary>
    public void AddManager(string typeName, object obj)
    {
        if (!Managers.ContainsKey(typeName))
        {
            Managers.Add(typeName, obj);
        }
    }

    /// <summary>
    /// 添加Unity对象
    /// </summary>
    public T AddManager<T>(string typeName) where T : Component
    {
        object result;
        Managers.TryGetValue(typeName, out result);
        if (result != null)
        {
            return (T)result;
        }
        Component c = AppGameManager.AddComponent<T>();
        Managers.Add(typeName, c);
        return default(T);
    }

    /// <summary>
    /// 获取系统管理器
    /// </summary>
    public T GetManager<T>(string typeName) where T : class
    {
        if (!Managers.ContainsKey(typeName))
        {
            return default(T);
        }
        object manager;
        Managers.TryGetValue(typeName, out manager);
        return (T)manager;
    }

    /// <summary>
    /// 删除管理器
    /// </summary>
    public void RemoveManager(string typeName)
    {
        if (!Managers.ContainsKey(typeName))
        {
            return;
        }
        object manager;
        Managers.TryGetValue(typeName, out manager);
        if (manager != null)
        {
            Type type = manager.GetType();
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                Object.Destroy((Component)manager);
            }
        }
        Managers.Remove(typeName);
    }
}
