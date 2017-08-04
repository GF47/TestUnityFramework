using System;
using System.Collections.Generic;

public class Controller : IController
{
    protected IDictionary<string, Type> commandMap;
    protected IDictionary<IView, List<string>> viewCommandMap;

    protected static volatile IController instance;
    protected readonly object syncLocker = new object();
    protected static readonly object StaticSyncLocker = new object();

    public static IController Instance
    {
        get
        {
            if (instance == null)
            {
                lock (StaticSyncLocker)
                {
                    if (instance == null)
                    {
                        Controller controller = new Controller();
                        controller.InitializeController();
                        instance = controller;
                    }
                }
            }
            return instance;
        }
    }

    protected virtual void InitializeController()
    {
        commandMap = new Dictionary<string, Type>();
        viewCommandMap = new Dictionary<IView, List<string>>();
    }

    public virtual void ExecuteCommand(IMessage note)
    {
        Type commandType = null;
        List<IView> views = null;
        lock (syncLocker)
        {
            if (commandMap.ContainsKey(note.Name))
            {
                commandType = commandMap[note.Name];
            }
            else
            {
                views = new List<IView>();
                foreach (var de in viewCommandMap)
                {
                    if (de.Value.Contains(note.Name))
                    {
                        views.Add(de.Key);
                    }
                }
            }
        }
        if (commandType != null)
        {  //Controller
            object commandInstance = Activator.CreateInstance(commandType);
            var command = commandInstance as ICommand;
            if (command != null)
            {
                command.Execute(note);
            }
        }
        if (views != null && views.Count > 0)
        {
            for (int i = 0; i < views.Count; i++)
            {
                views[i].OnMessage(note);
            }
        }
    }

    public virtual void RegisterCommand(string commandName, Type commandType)
    {
        lock (syncLocker)
        {
            commandMap[commandName] = commandType;
        }
    }

    public virtual void RegisterViewCommand(IView view, IList<string> commandNames)
    {
        lock (syncLocker)
        {
            if (viewCommandMap.ContainsKey(view))
            {
                List<string> list;
                if (viewCommandMap.TryGetValue(view, out list))
                {
                    for (int i = 0; i < commandNames.Count; i++)
                    {
                        if (list.Contains(commandNames[i])) continue;
                        list.Add(commandNames[i]);
                    }
                }
            }
            else
            {
                viewCommandMap.Add(view, new List<string>(commandNames));
            }
        }
    }

    public virtual bool HasCommand(string commandName)
    {
        lock (syncLocker)
        {
            return commandMap.ContainsKey(commandName);
        }
    }

    public virtual void RemoveCommand(string commandName)
    {
        lock (syncLocker)
        {
            if (commandMap.ContainsKey(commandName))
            {
                commandMap.Remove(commandName);
            }
        }
    }

    public virtual void RemoveViewCommand(IView view, IList<string> commandNames)
    {
        lock (syncLocker)
        {
            if (viewCommandMap.ContainsKey(view))
            {
                List<string> list;
                if (viewCommandMap.TryGetValue(view, out list))
                {
                    for (int i = 0; i < commandNames.Count; i++)
                    {
                        if (!list.Contains(commandNames[i])) continue;
                        list.Remove(commandNames[i]);
                    }
                }
            }
        }
    }
}

