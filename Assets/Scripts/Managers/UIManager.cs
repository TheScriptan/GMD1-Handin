using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiBlueTeamScore;
    [SerializeField] private GameObject uiBlueTeamGold;
    [SerializeField] private GameObject uiRedTeamScore;
    [SerializeField] private GameObject uiRedTeamGold;
    [SerializeField] private GameObject uiEndGameText;


    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.OnScoreUpdate += OnScoreUpdate;
        GameManager.Instance.OnGoldUpdate += OnGoldUpdate;
        GameManager.Instance.OnGameFinished += OnGameFinish;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnScoreUpdate(object sender, GameManager.OnScoreUpdateEventArgs e)
    {
        if (e.team == "Red")
            ChangeText(uiRedTeamScore, "Red score: " + e.score);
        if (e.team == "Blue")
            ChangeText(uiBlueTeamScore, "Blue score: " + e.score);
    }
    
    private void OnGoldUpdate(object sender, GameManager.OnGoldUpdateEventArgs e)
    {
        if (e.team == "Red")
            ChangeText(uiRedTeamGold, "Red gold: " + e.gold);
        if (e.team == "Blue")
            ChangeText(uiBlueTeamGold, "Blue gold: " + e.gold);
    }

    private void OnGameFinish(object sender, GameManager.OnGameFinishedEventArgs e)
    {
        if (e.team == "Red")
        {
            ChangeText(uiEndGameText, $"Red team wins by {e.redScore - e.blueScore} points!");
            uiEndGameText.GetComponent<Text>().color = Color.red;
        }

        if (e.team == "Blue")
        {
            ChangeText(uiEndGameText, $"Blue team wins by {e.blueScore - e.redScore} points!");
            uiEndGameText.GetComponent<Text>().color = Color.blue;
        }
    }

    private void ChangeText(GameObject obj, string text)
    {
        obj.GetComponent<Text>().text = text;
    }
}