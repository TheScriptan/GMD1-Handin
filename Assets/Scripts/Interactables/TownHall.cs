using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownHall : MonoBehaviour
{
    public enum Teams
    {
        Red,
        Blue
    }

    public Teams teams;

    public string GetTeam() => teams.ToString();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}