using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LogManager : MonoBehaviour
{
 [SerializeField]private Text LogTest;


    private void Start()
    {
        Application.logMessageReceived += OnLogShown;
    }

    private void OnLogShown(string condition, string stackTrace, LogType type)
    {
        LogTest.text = condition;
    }
}
