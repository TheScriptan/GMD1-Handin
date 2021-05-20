using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class MinionWalkState : BaseState
    {
        private Minion _minion;
        private NavMeshAgent _agent;
        
        private ArrayList points = new ArrayList();
        private int destPoint = 0;
        
        public MinionWalkState(Minion minion) : base(minion.gameObject)
        {
            _minion = minion;
            _agent = _minion.GetComponent<NavMeshAgent>();

            GotoNextPoint();
        }

        public override Type Tick()
        {
            //Giving time for .Initialize method to run since Awake initialized StateMachine earlier
            if (_minion.team == "")
                return null;
            
            if (points.Count == 0)
            {
                //If red team captures blue team barracks and builds it there, there will be a bug with pathfinding ;/
                var pathTag =  _minion.team == "Red" ? "RedMinionPath" : "BlueMinionPath";
                var pathNodes = GameObject.FindGameObjectsWithTag(pathTag);
                foreach (GameObject path in pathNodes)
                {
                    points.Add(path.transform);
                }
            }
            
            
            _minion.CheckForAggro();

            if (_minion.targetedEnemy != null)
            {
                return typeof(MinionAttackState);
            }
            
            if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
            {
                GotoNextPoint();
                //Debug.Log("Finished moving to path");
            }

            return null;
        }
        
        private void GotoNextPoint()
        {
            if (points.Count == 0)
                return;

            if (destPoint == points.Count - 1)
            {
                //Attack town hall
                //Debug.Log("Finished pathing");
            }
                

            _agent.destination = ((Transform) points[destPoint]).position;
            
            //destPoint = (destPoint + 1) % points.Count; Patrolling
            if (destPoint < points.Count - 1)
                destPoint += 1;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _agent.stoppingDistance = 0f;
            _agent.destination = transform.position;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _agent.destination = transform.position;
        }
    }
}