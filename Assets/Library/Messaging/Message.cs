using UnityEngine;
using System.Collections;

public class Message : System.Object{
	
    public string listenerType { get; private set; }
    public string functionName { get; private set; }

    public Message(string _listenerType)
    {
        listenerType = _listenerType;

        functionName = "_" + listenerType;
        //Send itself automatically after creation!
        
    }

    protected void Send()
    {
        Messenger.instance.Send(this);
    }

    public override string ToString()
    {
        return "Message " + listenerType;
    }
}

