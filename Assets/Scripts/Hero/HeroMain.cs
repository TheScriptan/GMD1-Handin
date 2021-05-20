using System;
using UnityEngine;

public class HeroMain : MonoBehaviour
{
    public enum Teams
    {
        Red,
        Blue
    }
    public Teams teams;


    public string GetTeam() => teams.ToString();
    
    private void OnDestroy()
    {
        HeroSpawner.heroCount--;
    }
}