using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class HeroPlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    public float rotateSpeedMovement = 0.1f;
    public float rotateVelocity;

    private HeroCombat _heroCombat;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _heroCombat = GetComponent<HeroCombat>();
    }

    private void Update()
    {
        if (_heroCombat.targetedEnemy != null)
        {
            if (_heroCombat.targetedEnemy.GetComponent<HeroCombat>() != null)
            {
                if (_heroCombat.targetedEnemy.GetComponent<HeroCombat>().isHeroAlive)
                {
                    _heroCombat.targetedEnemy = null;
                }
            }
        }

        //POINT TO CLICK MOVEMENT
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Environment"))
                {
                    agent.SetDestination(hit.point);
                    _heroCombat.targetedEnemy = null;
                    agent.stoppingDistance = 0;

                    Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y,
                        ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                    transform.eulerAngles = new Vector3(0, rotationY, 0);
                }
            }
        }

        //INSTANT ROTATION
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
    }
}