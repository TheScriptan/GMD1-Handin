using System;
using UnityEngine;

namespace AI
{
    public class RecruitMinionsState : HeroAIState
    {
        private GameObject[] _barracks;
        
        public RecruitMinionsState(HeroAI heroAI) : base(heroAI)
        {
        }

        public override Type Tick()
        {
            //Before checking preconditional states, check if we have rax. If not, build it
            if (!HasBarracks())
            {
                return typeof(BuildBarracksState);
            }
            
            //Recruit minions
            if (_destination == null && HasBarracks() && GameManager.Instance.IsEnoughGold(_heroAI.GetTeam(), 50))
            {
                var newDest = GetOwnedBarracks().transform.position;
                _destination = newDest;
                _agent.destination = (Vector3) _destination;
            }
            
            var preconditionalState = GetPreconditionalState();
            if (preconditionalState != null)
                return preconditionalState;

            if (_heroAI.IsDestinationReached())
            {
                //StartCoroutine to measure when objective is catpured
                return typeof(WanderState);
            }
            return null;
        }
        
        private bool HasBarracks()
        {
            _barracks = GetBarracks();
            foreach (GameObject barrack in _barracks)
            {
                if (barrack.GetComponent<Barracks>().GetTeam() == _heroAI.GetTeam())
                {
                    return true;
                }
            }

            return false;
        }
        
        private GameObject GetOwnedBarracks()
        {
            foreach (GameObject rax in _barracks)
            {
                if (rax.GetComponent<Barracks>().GetTeam() == _heroAI.GetTeam())
                {
                    return rax;
                }
            }

            return null;
        }
        
        private GameObject[] GetBarracks() => GameObject.FindGameObjectsWithTag("Barracks");
    }
}