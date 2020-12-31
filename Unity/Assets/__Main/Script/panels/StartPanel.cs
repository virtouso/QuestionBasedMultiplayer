using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartPanel : MonoBehaviour
{

    #region Uity References

    [SerializeField] private Button _startGameButton;
    [SerializeField] private InputField _userInputField;


    #endregion
    #region private

    [Inject] private GameManager _gameManager;
    [Inject] private NetworkManager _networkManager;
    #endregion


    private void Awake()
    {
        _startGameButton.onClick.AddListener(requestJoinGame);
    }



    private void requestJoinGame()
    {



        if (_networkManager.PlayerNetworkStateHolder.ConnectionState == ConnectionStates.Disconnected)
            _networkManager.RestartConnection();

        _gameManager.PlayerData.UserId = _userInputField.text;


        if (string.IsNullOrEmpty(_gameManager.PlayerData.UserId))
        {

            _gameManager.ShowPopup("no connection", "please enter you unique suer Id", null);
            return;

        }

        _gameManager.RunningMatchData.PlayerUserId = _userInputField.text;

        if (_networkManager.PlayerNetworkStateHolder.ConnectionState == ConnectionStates.Disconnected)
        {

            _gameManager.ShowPopup("no connection", "no connecction is established to the game server", null);
            return;
        }
        _gameManager.EnablePanel(Panels.LobbyPanel, true, Panels.StartPanel);
        _networkManager.SignalrConnection.SendMessage(_networkManager.ServerMethods.GetMethodName(ServerMethodEnums.JoinMeToLobby), _gameManager.PlayerData.UserId);

    }





}
