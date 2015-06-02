using UnityEngine;
using System.Collections;
using System;
/*
 * 
 * Base Class for messages used by the messenger
 *
*/
[Serializable]
public class SerializableMessage : System.Object{
	
    public string listenerType { get; private set; }
    public string functionName { get; private set; }

    public SerializableMessage(string _listenerType)
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
