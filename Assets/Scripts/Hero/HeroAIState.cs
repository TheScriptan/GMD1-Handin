using System;
using AI;
using UnityEngine;
using UnityEngine.AI;

public abstract class HeroAIState : BaseState
{
    protected HeroAI _heroAI;
    protected NavMeshAgent _agent;
    protected Vector3? _destination;

    public HeroAIState(HeroAI heroAI) : base(heroAI.gameObject)
    {
        _heroAI = heroAI;
        _agent = heroAI.GetComponent<NavMeshAgent>();
    }

    public Type CheckForAggro()
    {
        //Check for Aggro
        _heroAI.CheckForAggro();

        //If target exists, go attack him
        return _heroAI.targetedEnemy != null ? typeof(LichAttackState) : null;
    }

    public Type GetPreconditionalState()
    {
        //Checking for aggro
        _heroAI.CheckForAggro();

        if (_heroAI.targetedEnemy != null)
        {
            return typeof(LichAttackState);
        }

        //Checking for Minion recruitment if Gold condition is met
        if (GameManager.Instance.IsEnoughGold(_heroAI.GetTeam(), 50))
        {
            return typeof(RecruitMinionsState);
        }
        
        return null;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        _destination = null;
    }
}