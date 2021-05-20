using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class CapturePillarState : HeroAIState
{
    private GameObject[] _pillars;
    private bool _isCaptured;
    private bool _isCapturing;

    public CapturePillarState(HeroAI heroAI) : base(heroAI)
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

        var uncapturedPillar = GetUncapturedPillar();
        if (_destination == null && uncapturedPillar != null)
        {
            var newDest = uncapturedPillar; //_heroAI.GetClosestPoint(_pillars);
            _destination = newDest;
            _agent.destination = (Vector3) _destination;
            //Debug.Log("New Pillar Destination set");
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

    private Vector3? GetUncapturedPillar()
    {
        _pillars = GetPillars();
        float minDist = Mathf.Infinity;
        Vector3? nearestPoint = null;
        foreach (GameObject pillar in _pillars)
        {
            if (pillar.GetComponent<IsCaptureable>().capturedTeam != _heroAI.GetTeam())
            {
                float dist = Vector3.Distance(transform.position, pillar.transform.position);
                if (dist < minDist)
                {
                    nearestPoint = pillar.transform.position;
                    minDist = dist;
                }
            }
        }

        return nearestPoint;
    }

    private GameObject[] GetPillars() => GameObject.FindGameObjectsWithTag("Pillar");

    private IEnumerator CapturingDelay()
    {
        yield return new WaitForSeconds(3);
        _isCaptured = true;
    }
}