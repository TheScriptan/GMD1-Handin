using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class LichAttackState : BaseState
    {
        private HeroAI _heroAI;
        private NavMeshAgent _agent;
        private Animator _anim;
        private float attackRange = 7f;
        public LichAttackState(HeroAI heroAi) : base(heroAi.gameObject)
        {
            _heroAI = heroAi;
            _agent = heroAi.GetComponent<NavMeshAgent>();
            _anim = heroAi.GetComponent<Animator>();
        }

        public override Type Tick()
        {
            if (_heroAI.targetedEnemy == null)
            {
                return typeof(WanderState);
            }
            
            var dist = Vector3.Distance(_heroAI.transform.position, _heroAI.targetedEnemy.transform.position);
            //Cancels aggro
            if (dist > _heroAI.aggroRange)
            {
                _heroAI.targetedEnemy = null;
                return typeof(WanderState);
            }
        
            
            //Gets closer to target for attack
            if (dist > attackRange)
            {
                Debug.Log("ATTACKING 1");
                _agent.destination = _heroAI.targetedEnemy.transform.position;
                _agent.stoppingDistance = attackRange - 0.5f;
                _anim.SetBool("Basic Attack", false);
            }
            //Attacking
            else
            {
                Quaternion rotationToLookAt = Quaternion.LookRotation(
                    _heroAI.targetedEnemy.transform.position - transform.position);
                float rotationY = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref _heroAI.rotateVelocity,
                    _heroAI.rotateSpeedForAttack * (Time.deltaTime * 5));
                transform.eulerAngles = new Vector3(0, rotationY, 0);
                
                _agent.SetDestination(transform.position); //If conditions for attacking is met, stop yourself so animation conditions are met too
                _anim.SetBool("Basic Attack", true);
                if (_heroAI.canAttack)
                {
                    _heroAI.StartCoroutine(RangedAttackInterval());
                }
            }
            
            return null;
        }
        
        private IEnumerator RangedAttackInterval()
        {
            _heroAI.canAttack = false;
            _anim.SetBool("Basic Attack", true);

            yield return new WaitForSeconds(_heroAI.stats.attackTime / ((100 + _heroAI.stats.attackTime) * 0.01f));
            if (_heroAI.targetedEnemy == null)
            {
                _anim.SetBool("Basic Attack", false);
                _heroAI.canAttack = true;
            }
        }
        
        public override void OnStateExit()
        {
            base.OnStateExit();
            _anim.SetBool("Basic Attack", false);
            _heroAI.canAttack = true;
        }
    }
}