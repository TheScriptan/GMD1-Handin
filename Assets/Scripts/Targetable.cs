using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    public enum EnemyType
    {
        Minion,
        Hero,
        TownHall
    }

    public EnemyType enemyType;
}
