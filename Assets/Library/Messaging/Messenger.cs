using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Library/Messenger")]
public class Messenger : MonoBehaviour {

    public static Messenger instance;
    private Dictionary<string, List<GameObject>> messageDictionary;

    public void Awake() {
		DontDestroyOnLoad(gameObject);
		if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            messageDictionary = new Dictionary<string,List<GameObject>>();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SubscribeTo(string _listenerType, GameObject _gameObjectToAdd)
    {
        List<GameObject> objectList;
        if (!messageDictionary.ContainsKey(_listenerType))
        {
            objectList = new List<GameObject>();
            messageDictionary.Add(_listenerType, objectList);
        }

        objectList = messageDictionary[_listenerType];

        if (!objectList.Contains(_gameObjectToAdd))
        {
            objectList.Add(_gameObjectToAdd);
        }
    }

    public void UnsubcribeFrom(string _listenerType, GameObject _gameObjectToRemove)
    {
        if (messageDictionary.ContainsKey(_listenerType))
        {
            List<GameObject> objectList = messageDictionary[_listenerType];
            if (objectList.Contains(_gameObjectToRemove))
            {
                objectList.Remove(_gameObjectToRemove);
            }
        }
    }

    public void Send(SerializableMessage _message)
    {
        string _listenerType = _message.listenerType;
		if(!messageDictionary.ContainsKey(_listenerType)) {
			Debug.LogWarning("No Object is listening to " + _listenerType);
		} else {
			List<GameObject> objectList = messageDictionary[_listenerType];
			if (objectList != null)
			{
				foreach (GameObject _gameObject in objectList)
				{
					if (_gameObject != null)
					{
						_gameObject.SendMessage(_message.functionName, _message, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
    }

	public void Send(Message _message)
	{
		string _listenerType = _message.listenerType;
		if(!messageDictionary.ContainsKey(_listenerType)) {
			Debug.LogWarning("No Object is listening to " + _listenerType);
		}
		List<GameObject> objectList = messageDictionary[_listenerType];
		if (objectList != null)
		{
			foreach (GameObject _gameObject in objectList)
			{
				if (_gameObject != null)
				{
					_gameObject.SendMessage(_message.functionName, _message, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
