using System;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class Minion : MonoBehaviour
{
    private AudioSource _audioSource;
    public string team = "";
    public GameObject targetedEnemy;
    public bool canAttack = true;
    public float aggroRange = 5f;

    public StateMachine StateMachine => GetComponent<StateMachine>();

    public Stats stats;

    public void Awake()
    {
        stats = GetComponent<Stats>();
        _audioSource = GetComponent<AudioSource>();
        InitializeStateMachine();
    }

    public void Initialize(string team)
    {
        tag = "Minion";
        this.team = team;
    }
    
    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            {typeof(MinionWalkState), new MinionWalkState(this)},
            {typeof(MinionAttackState), new MinionAttackState(this)},
        };
        
        GetComponent<StateMachine>().SetStates(states);
    }

    public void CheckForAggro()
    {
        var heros = GameObject.FindGameObjectsWithTag("Hero");
        var minions = GameObject.FindGameObjectsWithTag("Minion");
        var townhalls = GameObject.FindGameObjectsWithTag("TownHall");

        foreach (GameObject hero in heros)
        {
            var dist = Vector3.Distance(transform.position, hero.transform.position);
            if (dist < aggroRange && hero.GetComponent<HeroMain>().GetTeam() != team)
            {
                targetedEnemy = hero;
                return;
            }
        }
        
        foreach (GameObject minion in minions)
        {
            var dist = Vector3.Distance(transform.position, minion.transform.position);
            if (dist < aggroRange && minion.GetComponent<Minion>().team != team)
            {
                targetedEnemy = minion;
                return;
            }
        }
        
        foreach (GameObject townhall in townhalls)
        {
            var dist = Vector3.Distance(transform.position, townhall.transform.position);
            if (dist < aggroRange && townhall.GetComponent<TownHall>().GetTeam() != team)
            {
                targetedEnemy = townhall;
                return;
            }
        }

        targetedEnemy = null;
    }
    
    //Called from Animation events
    public void MinionMeleeAttack()
    {
        if (targetedEnemy != null)
        {
            var targetable = targetedEnemy.GetComponent<Targetable>();
            _audioSource.Play();
            if (targetable.enemyType == Targetable.EnemyType.Minion)
            {
                targetedEnemy.GetComponent<Health>().health -= stats.attackDmg;
            }

            if (targetable.enemyType == Targetable.EnemyType.Hero)
            {
                targetedEnemy.GetComponent<Health>().health -= stats.attackDmg;
            }
            if (targetable.enemyType == Targetable.EnemyType.TownHall)
            {
                GameManager.Instance.UpdateScore(team, 1);
            }
        }

        canAttack = true;
    }
}