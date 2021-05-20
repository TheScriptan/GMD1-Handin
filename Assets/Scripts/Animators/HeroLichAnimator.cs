using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroLichAnimator : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _anim;

    private float motionSmoothTime = .1f;
    private static readonly int Speed = Animator.StringToHash("Speed");

    void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = _agent.velocity.magnitude / _agent.speed;
        _anim.SetFloat(Speed, speed, motionSmoothTime, Time.deltaTime);
    }
}
