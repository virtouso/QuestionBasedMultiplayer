using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BestHTTP.SignalR;
using System;
//using BestHTTP;
//using BestHTTP.SignalR.Hubs;
//using BestHTTP.SignalR.Messages;
using Zenject;
using Microsoft.AspNetCore.SignalR.Client;

public class NetworkManager : MonoBehaviour
{
    #region public

    public  void RestartConnection()
    {
        initSignalr();
    }

    [Inject] public PlayerNetworkStateHolder PlayerNetworkStateHolder;
    [SerializeField] public ServerMethods ServerMethods;
    [SerializeField] public ClientMethods ClientMethods;
    [SerializeField] public string GameHubName = "MainHub";



    public System.Action<string> OnConnectionStarted;
    public System.Action<string, int> OnJoinedLobby;
    public System.Action<int, string, int, int> OnRoundStarted;
    public System.Action<int, int> OnGameFinished;
    public System.Action<string> OnDisconnected;

    #endregion
    #region Unity References
    [SerializeField] private string _serverUrl = "http://localhost:5000/MainHub";
    [Inject] private GameManager _gameManager;
    #endregion

    #region private

    public SignalRLib SignalrConnection;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        OnConnectionStarted += connectionStarted;
        OnDisconnected += Disconnected;
    }

    private void Start()
    {

        initSignalr();

    }


    #endregion

    #region SignalR Callbacks
    private void connectionStarted(string result)
    {
        print("conneccted to the server");
        PlayerNetworkStateHolder.ConnectionState = ConnectionStates.Connnected;
    }
    private void Disconnected(string info)
    {
        print(info);
        SignalrConnection.Connection.StopAsync();
        PlayerNetworkStateHolder.ConnectionState = ConnectionStates.Disconnected;
        _gameManager.ShowPopup("disconnecct", "you are disconnected", delegate { _gameManager.RestartGame(); });
    }
    #endregion

  


    #region Utility
    private async void initSignalr()
    {
      



        this.SignalrConnection = new SignalRLib();
        this.SignalrConnection.Connection = new HubConnectionBuilder().WithUrl(_serverUrl).Build();

        this.SignalrConnection.Connection.On<string, int>(ClientMethods.GetMethodName(ClientMethodEnums.OnJoinedToLobby),
            (gameGuid, playerIndex) => { print("joined lobby"); if (OnJoinedLobby != null) OnJoinedLobby.Invoke(gameGuid, playerIndex); });

        this.SignalrConnection.Connection.On<int, string, int, int>(ClientMethods.GetMethodName(ClientMethodEnums.OnRoundStarted),
            (roundNumber, roundQuestion, p1Score, p2Score) => { print("round started"); if (OnRoundStarted != null) OnRoundStarted.Invoke(roundNumber, roundQuestion, p1Score, p2Score); });

        this.SignalrConnection.Connection.On<int, int>(ClientMethods.GetMethodName(ClientMethodEnums.OnGameFinished),
            (p1Score, p2Score) => { print("game finished"); if (OnGameFinished != null) OnGameFinished.Invoke(p1Score, p2Score); });

        this.SignalrConnection.Connection.On<string>(ClientMethods.GetMethodName(ClientMethodEnums.EndMatch),
           (message) => { print("discontected"); if (OnDisconnected != null) OnDisconnected.Invoke(message); });

        try
        {
            await this.SignalrConnection.Connection.StartAsync();
            if (OnConnectionStarted != null) OnConnectionStarted.Invoke(this.SignalrConnection.Connection.ConnectionId);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.InnerException);
        }

    }





    #endregion
}



public class PlayerNetworkStateHolder
{
    public ConnectionStates ConnectionState;
    public int SendErrorCount;
    public int ReceiveErrorCount;
    public int DisconnectionCount;
}


public enum ConnectionStates { Disconnected, Connnected }

