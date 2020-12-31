//using BestHTTP.SignalR.Hubs;
//using BestHTTP.SignalR.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WaitPanel : MonoBehaviour
{

    [SerializeField] private Text _messageText;


    [Inject] private NetworkManager _networkManager;
    [Inject] private GameManager _gameManager;

    #region Unity Callbacks
    private void Start()
    {
        _networkManager.OnJoinedLobby += OnJoinedLobby;
      
    }

    #endregion




    #region Utility



    private void OnJoinedLobby(string matchId, int playerIndex)
    {
       
        _messageText.text = "dude you joined the lobby";

        _gameManager.RunningMatchData.PlayerIndexInMatch = playerIndex;
        _gameManager.RunningMatchData.MatchGuid = matchId;

        _gameManager.EnablePanel(Panels.QuestionPanel, true, Panels.LobbyPanel);
    }


    #endregion

}
