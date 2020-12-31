using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ServerMethods", menuName = "Configurations/ServerMethods", order = 1)]
public class ServerMethods : ScriptableObject
{

    public string GetMethodName(ServerMethodEnums methodEnum)
    {
        return MethodsDictionary[methodEnum];
    }


    private Dictionary<ServerMethodEnums, string> _methodsDictionary;

    public Dictionary<ServerMethodEnums, string> MethodsDictionary
    {
        get
        {
            if (_methodsDictionary == null)
            {
                _methodsDictionary = new Dictionary<ServerMethodEnums, string>();
                for (int i = 0; i < _serverMethodsArray.Length; i++)
                {
                    _methodsDictionary.Add(_serverMethodsArray[i].MethodEnum, _serverMethodsArray[i].MethodText);
                }
            }
            return _methodsDictionary;
        }

    }




    [SerializeField] private ServerMethod[] _serverMethodsArray;

}


[System.Serializable]
public class ServerMethod
{
    public ServerMethodEnums MethodEnum;
    public string MethodText;
}

public enum ServerMethodEnums { JoinMeToLobby, CalculateMyAnswer }
