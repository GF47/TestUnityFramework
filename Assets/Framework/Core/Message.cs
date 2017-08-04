public class Message : IMessage
{
    public Message(string name)
        : this(name, null, null)
    { }

    public Message(string name, object body)
        : this(name, body, null)
    { }

    public Message(string name, object body, string type)
    {
        _name = name;
        _body = body;
        _type = type;
    }

    /// <summary>
    /// Get the string representation of the <c>Notification instance</c>
    /// </summary>
    /// <returns>The string representation of the <c>Notification</c> instance</returns>
    public override string ToString()
    {
        string msg = "Notification Name: " + Name;
        msg += "\nBody:" + (Body == null ? "null" : Body.ToString());
        msg += "\nType:" + (Type == null ? "null" : Type);
        return msg;
    }

    /// <summary>
    /// The name of the <c>Notification</c> instance
    /// </summary>
    public virtual string Name
    {
        get { return _name; }
    }

    /// <summary>
    /// The body of the <c>Notification</c> instance
    /// </summary>
    /// <remarks>This accessor is thread safe</remarks>
    public virtual object Body
    {
        get
        {
            // Setting and getting of reference types is atomic, no need to lock here
            return _body;
        }
        set
        {
            // Setting and getting of reference types is atomic, no need to lock here
            _body = value;
        }
    }

    /// <summary>
    /// The type of the <c>Notification</c> instance
    /// </summary>
    /// <remarks>This accessor is thread safe</remarks>
    public virtual string Type
    {
        get
        {
            // Setting and getting of reference types is atomic, no need to lock here
            return _type;
        }
        set
        {
            // Setting and getting of reference types is atomic, no need to lock here
            _type = value;
        }
    }

    /// <summary>
    /// The name of the notification instance 
    /// </summary>
    private readonly string _name;

    /// <summary>
    /// The type of the notification instance
    /// </summary>
	private string _type;

    /// <summary>
    /// The body of the notification instance
    /// </summary>
	private object _body;
}

