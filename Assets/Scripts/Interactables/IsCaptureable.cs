using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCaptureable : MonoBehaviour
{
    public event EventHandler OnCaptured;

    private float CAPTURE_DISTANCE = 5;
    private float CAPTURE_CANCEL_DISTANCE = 7;
    // Start is called before the first frame update
    private GameObject[] _heros;
    private GameObject _isBeingCapturedBy;
    private bool _isCaptured = false;
    public string capturedTeam = "";

    void Start()
    {
        _heros = GameObject.FindGameObjectsWithTag("Hero");
    }

    // Update is called once per frame
    void Update()
    {
        IsHeroNear();
    }

    private void IsHeroNear()
    {
        //Requery all heroes if current hero count doesn't match max hero count. Needed if for example hero gets killed
        if (_heros.Length != HeroSpawner.maxHeroCount)
        {
            _heros = GameObject.FindGameObjectsWithTag("Hero");
        }
        
        if (HeroSpawner.heroCount != HeroSpawner.maxHeroCount)
        {
            _heros = GameObject.FindGameObjectsWithTag("Hero");
        }

        //Cancel capturing when multiple heroes are near captureable object
        if (MultipleHerosNear() && _isBeingCapturedBy != null)
        {
            StopCoroutine("CapturingTimer");
            _isBeingCapturedBy = null;
            return;
        }
        
        //Cancel capturing by distance
        if (_isBeingCapturedBy != null)
        {
            var dist = Vector3.Distance(_isBeingCapturedBy.transform.position, transform.position);
            if (dist > CAPTURE_CANCEL_DISTANCE)
            {
                _isBeingCapturedBy = null;
                StopCoroutine("CapturingTimer");
            }
        }
        else
        {
            //Iterate through all heros and check distances
            foreach (GameObject hero in _heros)
            {
                var dist = Vector3.Distance(hero.transform.position, transform.position);
                var heroTeam = hero.GetComponent<HeroMain>().teams.ToString();
                //Capture object if distance is satisfied and if there are no multiple heroes nearby and if you are not recapturing your own object
                if (dist < CAPTURE_DISTANCE && MultipleHerosNear() == false && capturedTeam != heroTeam)
                {
                    _isBeingCapturedBy = hero;
                    StartCoroutine("CapturingTimer", heroTeam);
                }
            }
        }
        
    }

    private bool MultipleHerosNear()
    {
        var herosNearCount = 0;
        foreach (GameObject hero in _heros)
        {
            if (hero == null)
                continue;
            var dist = Vector3.Distance(hero.transform.position, transform.position);
            if (dist < CAPTURE_DISTANCE)
            {
                herosNearCount++;
            }
        }

        return herosNearCount > 1;
    }

    IEnumerator CapturingTimer(string team)
    {
        float duration = 3f;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        
        _isCaptured = true;
        _isBeingCapturedBy = null;
        capturedTeam = team;
        OnCaptured?.Invoke(this, EventArgs.Empty);
    }
}