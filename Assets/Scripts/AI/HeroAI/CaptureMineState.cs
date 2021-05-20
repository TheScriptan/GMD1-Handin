using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class CaptureMineState : HeroAIState
    {
        private GameObject[] _mines;
        private bool _isCaptured;
        private bool _isCapturing;

        public CaptureMineState(HeroAI heroAI) : base(heroAI)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _isCaptured = false;
            _isCapturing = false;
        }

        public override Type Tick()
        {
            var preconditionalState = GetPreconditionalState();
            if (preconditionalState != null)
                return preconditionalState;

            //Capture mine
            var uncapturedMine = GetUncapturedMine();
            if (_destination == null && uncapturedMine != null)
            {
                var newDest = uncapturedMine;
                _destination = newDest;
                _agent.destination = (Vector3) _destination;
            }

            if (_heroAI.IsDestinationReached() && !_isCapturing)
            {
                _isCapturing = true;
                _heroAI.StartCoroutine(CapturingDelay());
            }

            if (_isCaptured)
            {
                return typeof(WanderState);
            }

            return null;
        }

        private GameObject[] GetMines() => GameObject.FindGameObjectsWithTag("Mine");

        private Vector3? GetUncapturedMine()
        {
            float minDist = Mathf.Infinity;
            Vector3? nearestPoint = null;
            _mines = GetMines();
            foreach (GameObject mine in _mines)
            {
                if (mine.GetComponent<IsCaptureable>().capturedTeam != _heroAI.GetTeam())
                {
                    float dist = Vector3.Distance(transform.position, mine.transform.position);
                    if (dist < minDist)
                    {
                        nearestPoint = mine.transform.position;
                        minDist = dist;
                    }
                }
            }

            return nearestPoint;
        }

        private IEnumerator CapturingDelay()
        {
            yield return new WaitForSeconds(3);
            _isCaptured = true;
        }
    }
}