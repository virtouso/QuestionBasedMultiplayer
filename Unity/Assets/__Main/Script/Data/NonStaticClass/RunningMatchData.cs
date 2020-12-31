using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningMatchData
{
    public string MatchGuid;
    public string PlayerUserId;
    public string OpponentuserId;
    public int PlayerIndexInMatch;
    public Turns Turn;
    public int Player1Score;
    public int Player2Score;
    public int OpponentScore;


}



public enum Turns { Player, Opponent }