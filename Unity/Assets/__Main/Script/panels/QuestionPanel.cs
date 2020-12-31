using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;

public class QuestionPanel : MonoBehaviour
{
    #region Unity references
    [SerializeField] private Text _roundTurnText;
    [SerializeField] private Text _questionText;
    [SerializeField] private Button _questionChoicePrefab;
    [SerializeField] private Transform _choicesParent;
    [SerializeField] private GameObject _disableWindow;
    [SerializeField] private Text _scoresText;

    #endregion

    #region private
    [Inject] private GameManager _gameManager;
    [Inject] private NetworkManager _networkManager;


    #endregion


    private void Start()
    {
        _networkManager.OnRoundStarted += OnRoundStarted;
        _networkManager.OnGameFinished += OnGameFinished;
        _disableWindow.SetActive(true);

    }

    #region SignalR Client Callbacks


    private void OnRoundStarted
        (int roundNumber, string serializedQuestion, int p1Score, int p2Score)
    {

        _disableWindow.SetActive(false);
        bool isMyTurn = checkIsMyTurn(roundNumber);
        _roundTurnText.text = "Round:" + "/" + roundNumber + "Turn:" + findUserIdUserTurn(isMyTurn);
        _scoresText.text = generateScoresText(p1Score, p2Score);
        generateQuestion(serializedQuestion, isMyTurn);
    }



    private void OnGameFinished(int p1Score, int p2Score)
    {
        bool playerIsWinner = false;
        bool isDraw = false;

        if (p1Score == p2Score) { isDraw = true; }
        else if (_gameManager.RunningMatchData.PlayerIndexInMatch == 1) { if (p1Score > p2Score) playerIsWinner = true; else playerIsWinner = false; }
        else { if (p1Score < p2Score) playerIsWinner = true; else playerIsWinner = false; }

        restartGame(isDraw, playerIsWinner);


    }


    #endregion


    #region Utility
    private string generateScoresText(int p1Score, int p2Score)
    {
        bool isPlayer1 = _gameManager.RunningMatchData.PlayerIndexInMatch == 1;

        if (isPlayer1) { return "You: " + p1Score + "| Opponent: " + p2Score; }
        else { return "You: " + p2Score + "| Opponent: " + p1Score; }
    }

    private void generateQuestion(string serializedQuestion, bool myTurn)
    {
        Question question = JsonConvert.DeserializeObject<Question>(serializedQuestion);
        _questionText.text = question.QuestionBody;


        foreach (Transform child in _choicesParent)
        {
            Destroy(child.gameObject);
        }


        foreach (var item in question.Choices)
        {
            GameObject choice = Instantiate(_questionChoicePrefab.gameObject, _choicesParent, false);
            choice.transform.GetComponentInChildren<Text>().text = item.Value;
            if (myTurn) choice.GetComponent<Button>().onClick.AddListener(delegate { calculateMyAnswer(item.Key); });
            else choice.GetComponent<Button>().interactable = false;
        }
    }



    private bool checkIsMyTurn(int roundNumber)
    {
        if (_gameManager.RunningMatchData.PlayerIndexInMatch == 2)
        {
            if (roundNumber % 2 == 0) { return true; } else { return false; }
        }
        else
        {
            if (roundNumber % 2 != 0) { return true; } else { return false; }
        }

    }



    private string findUserIdUserTurn(bool isMyTurn)
    {
        if (isMyTurn) return _gameManager.RunningMatchData.PlayerUserId;
        else return "Opponent";
    }

    private void calculateMyAnswer(int answerIndex)
    {
        _networkManager.SignalrConnection.SendMessage(_networkManager.ServerMethods.GetMethodName(ServerMethodEnums.CalculateMyAnswer), _gameManager.RunningMatchData.MatchGuid, answerIndex);
    }

    private void restartGame(bool isDraw, bool playerIsWinner)
    {
        if (isDraw) _gameManager.ShowPopup("draw", "both players was equal", delegate { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
        else if (playerIsWinner) _gameManager.ShowPopup("winner", "you won the match", delegate { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
        else _gameManager.ShowPopup("Losr", "you lost the match", delegate { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
    } 
    #endregion
}
