using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{

    private IsCaptureable _isCaptureable;
    private ParticleSystem _fire;
    private AudioSource _audioSource;

    private IEnumerator _accumulateScoreCoroutine ;
    // Start is called before the first frame update
    void Start()
    {
        _fire = GetComponentInChildren<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        _accumulateScoreCoroutine = AccumulateScore();
        _isCaptureable = GetComponent<IsCaptureable>();
        _isCaptureable.OnCaptured += OnCaptured;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCaptured(object sender, EventArgs e)
    {
        StopCoroutine(_accumulateScoreCoroutine);
        StartCoroutine(_accumulateScoreCoroutine);

        var main = _fire.main;
        if (_isCaptureable.capturedTeam == "Red")
        {
            main.startColor = new Color(255, 0, 0);
            _fire.Play();
            _audioSource.Play();
        } else if (_isCaptureable.capturedTeam == "Blue")
        {
            main.startColor = new Color(0, 200, 255);
            _fire.Play();
            _audioSource.Play();
        }
    }

    private IEnumerator AccumulateScore()
    {
        float delay = 3f;
        while (true)
        {
            GameManager.Instance.UpdateScore(_isCaptureable.capturedTeam, 1);
            yield return new WaitForSeconds(delay);;
        }
    }
}
