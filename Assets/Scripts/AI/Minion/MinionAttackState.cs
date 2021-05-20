using System;
using System.Collections;
using AI;
using UnityEngine;
using UnityEngine.AI;

public class MinionAttackState : BaseState
{
    private Minion _minion;
    private NavMeshAgent _agent;
    private Animator _anim;

    private float attackRange = 2.5f;
    private float dist = 0f;

    public MinionAttackState(Minion minion) : base(minion.gameObject)
    {
        _minion = minion;
        _agent = _minion.GetComponent<NavMeshAgent>();
        _anim = _minion.GetComponent<Animator>();
    }

    public override Type Tick()
    {
        if (_minion.targetedEnemy == null)
        {
            return typeof(MinionWalkState);
        }

        dist = Vector3.Distance(_minion.transform.position, _minion.targetedEnemy.transform.position);
        //Cancels aggro
        if (dist > _minion.aggroRange)
        {
            _minion.targetedEnemy = null;
            return typeof(MinionWalkState);
        }
        
        //Gets closer to target for attack if distance is too high
        if (dist > attackRange)
        {
            _agent.destination = _minion.targetedEnemy.transform.position;
            _agent.stoppingDistance = attackRange - 0.5f;
            _anim.SetBool("Basic Attack", false);
        }
        //Attacking
        else
        {
            _anim.SetBool("Basic Attack", true);
            if (_minion.canAttack)
            {
                _minion.StartCoroutine(MeleeAttackInterval());
            }
        }

        return null;
    }

    private IEnumerator MeleeAttackInterval()
    {
        _minion.canAttack = false;
        _anim.SetBool("Basic Attack", true);
        yield return new WaitForSeconds(_minion.stats.attackTime / ((100 + _minion.stats.attackTime) * 0.01f));
        if (_minion.targetedEnemy == null)
        {
            _anim.SetBool("Basic Attack", false);
            _minion.canAttack = true;
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        _anim.SetBool("Basic Attack", false);
        _minion.canAttack = true;
    }
}