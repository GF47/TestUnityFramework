using System;
using System.Collections.Generic;

public interface IController
{
    void RegisterCommand(string messageName, Type commandType);
    void RegisterViewCommand(IView view, IList<string> commandNames);

    void ExecuteCommand(IMessage message);

	void RemoveCommand(string messageName);
    void RemoveViewCommand(IView view, IList<string> commandNames);

	bool HasCommand(string messageName);
}
