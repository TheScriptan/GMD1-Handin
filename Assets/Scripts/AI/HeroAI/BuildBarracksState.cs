using System;
using UnityEngine;

namespace AI
{
    public class BuildBarracksState : HeroAIState
    {
        private GameObject[] _buildingSpots;
        private GameObject[] _barracks;

        public BuildBarracksState(HeroAI heroAI) : base(heroAI)
        {
        }

        public override Type Tick()
        {
            //Build barracks
            if (_destination == null && !HasBarracks())
            {
                _buildingSpots = GetBuildingSpots();
                var newDest = _heroAI.GetClosestPoint(_buildingSpots);
                _destination = newDest.position;
                _agent.destination = (Vector3) _destination;
                //Debug.Log("New Building Spot Destination set");
            }
            
            //Check for aggro & if we can recruit minions
            var preconditionalState = GetPreconditionalState();
            if (preconditionalState != null)
                return preconditionalState;

            //Return to WanderState
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

        private GameObject[] GetBuildingSpots() => GameObject.FindGameObjectsWithTag("BuildingSpot");
        private GameObject[] GetBarracks() => GameObject.FindGameObjectsWithTag("Barracks");
    }
}