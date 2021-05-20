using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int _redTeamScore;
    private int _redTeamGold;

    private int _blueTeamScore;
    private int _blueTeamGold;

    public event EventHandler<OnScoreUpdateEventArgs> OnScoreUpdate;
    public event EventHandler<OnGoldUpdateEventArgs> OnGoldUpdate;
    public event EventHandler<OnGameFinishedEventArgs> OnGameFinished;

    public class OnScoreUpdateEventArgs : EventArgs
    {
        public string team;
        public int score;
    }
    

    public class OnGoldUpdateEventArgs : EventArgs
    {
        public string team;
        public int gold;
    }

    public class OnGameFinishedEventArgs : EventArgs
    {
        public string team;
        public int redScore;
        public int blueScore;
    }

    void Start()
    {
        _redTeamScore = 0;
        _redTeamGold = 100;
        _blueTeamScore = 0;
        _blueTeamGold = 100;
        OnScoreUpdate?.Invoke(this, new OnScoreUpdateEventArgs {team = "Red", score = _redTeamScore});
        OnScoreUpdate?.Invoke(this, new OnScoreUpdateEventArgs {team = "Blue", score = _blueTeamScore});
        OnGoldUpdate?.Invoke(this, new OnGoldUpdateEventArgs {team = "Red", gold = _redTeamGold});
        OnGoldUpdate?.Invoke(this, new OnGoldUpdateEventArgs {team = "Blue", gold = _blueTeamGold});

    }

    void Update()
    {
        HandleWinningCondition();
    }

    public void UpdateScore(string team, int value)
    {
        switch (team)
        {
            case "Red":
                _redTeamScore += value;
                OnScoreUpdate?.Invoke(this, new OnScoreUpdateEventArgs {team = "Red", score = _redTeamScore});
                break;
            case "Blue":
                _blueTeamScore += value;
                OnScoreUpdate?.Invoke(this, new OnScoreUpdateEventArgs {team = "Blue", score = _blueTeamScore});
                break;
            default:
                Debug.Log("Incorrect team given to GameManager to update score!");
                break;
        }
    }

    public void UpdateGold(string team, int value)
    {
        switch (team)
        {
            case "Red":
                _redTeamGold += value;
                OnGoldUpdate?.Invoke(this, new OnGoldUpdateEventArgs {team = "Red", gold = _redTeamGold});
                break;
            case "Blue":
                _blueTeamGold += value;
                OnGoldUpdate?.Invoke(this, new OnGoldUpdateEventArgs {team = "Blue", gold = _blueTeamGold});
                break;
            default:
                Debug.Log("Incorrect team given to GameManager to update gold!");
                break;
        }
    }

    public bool IsEnoughGold(string team, int value)
    {
        switch (team)
        {
            case "Red" when _redTeamGold >= value:
            case "Blue" when _blueTeamGold >= value:
                return true;
            default:
                return false;
        }
    }


    private void HandleWinningCondition()
    {
        if (_blueTeamScore >= 100)
        {
            OnGameFinished?.Invoke(this, new OnGameFinishedEventArgs{blueScore = _blueTeamScore, redScore = _redTeamScore, team =  "Blue"});
            Debug.Log("Blue team wins");   
        }
        else if (_redTeamScore >= 100)
        {
            OnGameFinished?.Invoke(this, new OnGameFinishedEventArgs{blueScore = _blueTeamScore, redScore = _redTeamScore, team =  "Red"});
            Debug.Log("Red team wins");
        }
    }
}