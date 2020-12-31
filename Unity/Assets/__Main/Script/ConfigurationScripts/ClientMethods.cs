using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ClientMethods", menuName = "Configurations/ClientMethods", order = 1)]
public class ClientMethods:  ScriptableObject
{
    public string GetMethodName(ClientMethodEnums methodEnum)
    {
        return MethodsDictionary[methodEnum];
    }


    private Dictionary<ClientMethodEnums, string> _methodsDictionary;

    public Dictionary<ClientMethodEnums, string> MethodsDictionary
    {
        get
        {
            if (_methodsDictionary == null)
            {
                _methodsDictionary = new Dictionary<ClientMethodEnums, string>();
                for (int i = 0; i < _clientMethodsArray.Length; i++)
                {
                    _methodsDictionary.Add(_clientMethodsArray[i].MethodEnum, _clientMethodsArray[i].MethodText);
                }
            }
            return _methodsDictionary;
        }

    }




    [SerializeField] private ClientMethod[] _clientMethodsArray;
}



[System.Serializable]
public class ClientMethod
{
    public ClientMethodEnums MethodEnum;
    public string MethodText;
}

public enum ClientMethodEnums {OnJoinedToLobby, OnRoundStarted, OnGameFinished ,EndMatch}