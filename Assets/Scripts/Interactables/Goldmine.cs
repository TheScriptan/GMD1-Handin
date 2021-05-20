using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goldmine : MonoBehaviour
{
    private IsCaptureable _isCaptureable;
    private AudioSource _audioSource;
    private int _accumulatedGold = 0;
    private IEnumerator _accumulateGoldCoroutine;
    
    void Start()
    {
        _isCaptureable = GetComponent<IsCaptureable>();
        _isCaptureable.OnCaptured += OnCaptured;
        _audioSource = GetComponent<AudioSource>();
        _accumulateGoldCoroutine = AccumulateGold();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCaptured(object sender, EventArgs e)
    {
        _audioSource.Play();
        StopCoroutine(_accumulateGoldCoroutine);
        StartCoroutine(_accumulateGoldCoroutine);
    }

    private IEnumerator AccumulateGold()
    {
        float delay = 3f;
        while (true)
        {
            _accumulatedGold+= 3;
            GameManager.Instance.UpdateGold(_isCaptureable.capturedTeam, 3);
            yield return new WaitForSeconds(delay);;
        }
    }
}
