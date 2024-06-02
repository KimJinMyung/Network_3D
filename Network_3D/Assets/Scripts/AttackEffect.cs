using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : NetworkBehaviour
{
    [SerializeField] private float _durationTIme = 2f;
    [SerializeField] private float _forcePower = 100;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.AddForce(transform.forward * _forcePower);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Invoke(nameof(DestroySelf), _durationTIme);
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(this.gameObject);
    }

    [ServerCallback]

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<Player>();
        if (target == null) return;
        DestroySelf();
    }
}
