using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float _rotation_Speed = 100.0f;

    [SerializeField]
    [SyncVar] private int _health;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        agent.velocity = forward * vertical * agent.speed;
    }
}
