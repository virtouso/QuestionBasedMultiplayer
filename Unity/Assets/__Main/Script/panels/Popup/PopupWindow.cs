using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PopupWindow : MonoBehaviour
{

    public void ShowPopup(string headMessage, string bodyMessage, UnityAction okButtonAction)
    {
        gameObject.SetActive(true);
        _headText.text = headMessage;
        _bodyText.text = bodyMessage;
        _okButton.onClick.RemoveAllListeners();
        if (okButtonAction != null) _okButton.onClick.AddListener(  okButtonAction);
        _okButton.onClick.AddListener(closePopup);
    }






    [SerializeField] private Text _headText;
    [SerializeField] private Text _bodyText;
    [SerializeField] private Button _okButton;



    private void closePopup()
    {
        this.gameObject.SetActive(false);
    }





}
