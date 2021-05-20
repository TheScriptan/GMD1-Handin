using System;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.AI;

public class HeroAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private HeroMain _heroMain;
    private AudioSource _audioSource;
    public Stats stats;
    public StateMachine StateMachine => GetComponent<StateMachine>();

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public GameObject targetedEnemy;
    public bool canAttack = true;
    public float aggroRange = 8f;
    public float rotateSpeedForAttack = 0.075f;
    public float rotateVelocity = 0.1f;


    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _heroMain = GetComponent<HeroMain>();
        _audioSource = GetComponent<AudioSource>();
        stats = GetComponent<Stats>();
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            {typeof(WanderState), new WanderState(this)},
            {typeof(LichAttackState), new LichAttackState(this)},
            {typeof(CaptureMineState), new CaptureMineState(this)},
            {typeof(CapturePillarState), new CapturePillarState(this)},
            {typeof(BuildBarracksState), new BuildBarracksState(this)},
            {typeof(RecruitMinionsState), new RecruitMinionsState(this)},
            {typeof(VisitTownHallState), new VisitTownHallState(this)}
        };
        
        GetComponent<StateMachine>().SetStates(states);
    }

    private void Update()
    {
        Debug.Log("HeroAI current state: " + StateMachine.CurrentState);
    }

    public NavMeshAgent GetAgent() => _agent;

    public bool IsDestinationReached() => !_agent.pathPending && _agent.remainingDistance < 0.1f;

    public string GetTeam() => _heroMain.teams.ToString();

    public GameObject GetEnemyHero()
    {
        var heros = GameObject.FindGameObjectsWithTag("Mine");
        foreach (GameObject hero in heros)
        {
            if (hero.GetComponent<HeroMain>().teams.ToString() != GetTeam())
            {
                return hero;
            }
        }

        return null;
    } 
    
    public void CheckForAggro()
    {
        var heros = GameObject.FindGameObjectsWithTag("Hero");
        var minions = GameObject.FindGameObjectsWithTag("Minion");

        foreach (GameObject hero in heros)
        {
            var dist = Vector3.Distance(transform.position, hero.transform.position);
            if (dist < aggroRange && hero.GetComponent<HeroMain>().GetTeam() != _heroMain.GetTeam())
            {
                targetedEnemy = hero;
                return;
            }
        }
        
        foreach (GameObject minion in minions)
        {
            var dist = Vector3.Distance(transform.position, minion.transform.position);
            if (dist < aggroRange && minion.GetComponent<Minion>().team != _heroMain.GetTeam())
            {
                targetedEnemy = minion;
                return;
            }
        }

        targetedEnemy = null;
    }

    public Transform GetClosestPoint(GameObject[] points)
    {
        float minDist = Mathf.Infinity;
        Transform nearestPoint = null;
        foreach (GameObject point in points)
        {
            float dist = Vector3.Distance(transform.position, point.transform.position);
            if (dist < minDist)
            {
                nearestPoint = point.transform;
                minDist = dist;
            }
        }
        return nearestPoint;
    }
    
    public void LichRangedAttack()
    {
        if (targetedEnemy != null)
        {
            var targetable = targetedEnemy.GetComponent<Targetable>();
            if (targetable.enemyType == Targetable.EnemyType.Minion)
            {
                _audioSource.Play();
                SpawnRangedProjectile("Minion", targetedEnemy);
            }

            if (targetable.enemyType == Targetable.EnemyType.Hero)
            {
                _audioSource.Play();
                SpawnRangedProjectile("Hero", targetedEnemy);
            }
        }

        canAttack = true;
    }

    private void SpawnRangedProjectile(string typeOfEnemy, GameObject targetEnemyObj)
    {
        float dmg = stats.attackDmg;

        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity).GetComponent<RangedProjectile>();
        
        if (typeOfEnemy == "Minion" || typeOfEnemy == "Hero")
        {
            projectile.targetType = typeOfEnemy;
            projectile.target = targetEnemyObj;
            projectile.damage = dmg;
            projectile.targetSet = true;
        }
    }
}