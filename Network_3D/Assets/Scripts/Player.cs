using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : NetworkBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float _rotationSpeed = 100.0f;

    [SerializeField]
    [SyncVar] private int _health;

    [SerializeField] GameObject _attackPrefab;
    [SerializeField] Transform _attackStartPoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!Application.isFocused) return;

        Rotation();
        Movement();

        Attack();
    }

    private void Movement()
    {
        if(!this.isLocalPlayer) return;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 side = transform.TransformDirection(Vector3.right);

        Debug.Log(((forward * vertical) + (side * horizontal)) * agent.speed);
        agent.velocity = ((forward * vertical) + (side * horizontal)) * agent.speed;
    }

    private void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 50))
        {
            Vector3 lookDir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(Vector3.Lerp(transform.position, lookDir, _rotationSpeed * Time.deltaTime));
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CommandAttack();
        }
    }

    [Command]
    void CommandAttack()
    {
        GameObject AttackEffect = Instantiate(this._attackPrefab, _attackStartPoint.position, _attackStartPoint.rotation);
        NetworkServer.Spawn(AttackEffect);

        RpcAttack();
    }

    [ClientRpc]
    void RpcAttack()
    {
        Debug.LogWarning($"{this.netId}가 공격 중...");
    }
}
