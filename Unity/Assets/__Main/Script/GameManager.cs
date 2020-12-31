using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    #region public
    [Inject] public PlayerDataModel PlayerData;// only reference to player data
    [Inject] public RunningMatchData RunningMatchData;
    public void ShowPopup(string headMessage, string bodyMessage, UnityAction okButtonClickAction)
    {
        this._popupWindow.ShowPopup(headMessage, bodyMessage, okButtonClickAction);
    }


    public void EnablePanel(Panels panel, bool disableCurrentPanel = false, Panels current = Panels.None)
    {
        _gamePanels[panel].SetActive(true);
        if (!disableCurrentPanel) return;
        _gamePanels[current].SetActive(false);

    }

    public void Disablepanel(Panels panel)
    {
        _gamePanels[panel].SetActive(false);

    }

    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   
    }



    #endregion
    #region unity refferences
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _lobbyPanel;
    [SerializeField] private GameObject _questionPanel;
    [SerializeField] private PopupWindow _popupWindow;
    #endregion


    #region private
    private Dictionary<Panels, GameObject> _gamePanels;
    #endregion

    #region unity callbacks
    private void Awake()
    {
        initPanels();
    }
    #endregion


    #region utility


    private void initPanels()
    {
        _gamePanels = new Dictionary<Panels, GameObject>();
        _gamePanels.Add(Panels.StartPanel, _startPanel);
        _gamePanels.Add(Panels.LobbyPanel, _lobbyPanel);
        _gamePanels.Add(Panels.QuestionPanel, _questionPanel);



    }

    #endregion



}

public enum Panels { None, StartPanel, LobbyPanel, QuestionPanel }
