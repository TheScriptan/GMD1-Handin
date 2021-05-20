using System;
using UnityEngine;

namespace AI
{
    public class VisitTownHallState : HeroAIState
    {
        public VisitTownHallState(HeroAI heroAI) : base(heroAI)
        {
        }

        public override Type Tick()
        {
            var preconditionalState = GetPreconditionalState();
            if (preconditionalState != null)
                return preconditionalState;

            var townhallPos = GetTownHall();
            if (_destination == null && townhallPos != null)
            {
                _destination = townhallPos;
                _agent.destination = (Vector3) _destination;
            }

            if (_heroAI.IsDestinationReached())
            {
                //StartCoroutine to measure when objective is catpured
                return typeof(WanderState);
            }

            return null;
        }

        private Vector3? GetTownHall()
        {
            var townhalls = GameObject.FindGameObjectsWithTag("TownHall");

            foreach (GameObject townhall in townhalls)
            {
                if (townhall.GetComponent<TownHall>().GetTeam() == _heroAI.GetTeam())
                {
                    return townhall.transform.position;
                }
            }

            return null;
        }
    }
}