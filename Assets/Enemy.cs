using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    
    public Health HealthData { get; private set; }

    [Header("Damage")]
    public int damage = 1;
    public float attackDistance = 2;
    public float attackDelay = 0.5f;
    public float attackTimer;
    public UnityEvent onAttack;


    [Header("Detection")]
    public float detectionRadius = 25;
    public float seekNewTargetDistance = 5;
    public LayerMask detectionMask = ~0;

    SwarmController enemy;
    SwarmMember currentTarget;



    NavMeshAgent na;

    private void Awake()
    {
        na = GetComponent<NavMeshAgent>();
        HealthData = GetComponent<Health>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = FindObjectOfType<SwarmController>();
    }

    SwarmMember FindSwarmMember()
    {
        SwarmMember closest = null;
        float closestDistance = detectionRadius;
        Collider[] things = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);
        foreach(Collider c in things)
        {
            SwarmMember s = c.GetComponentInParent<SwarmMember>();
            if (s != null && s.controller == enemy)
            {
                float distance = Vector3.Distance(s.transform.position, transform.position);
                if (closest == null || distance < closestDistance)
                {
                    closest = s;
                    closestDistance = distance;
                }
            }
        }

        return closest;
    }

    // Update is called once per frame
    void Update()
    {
        


        if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.transform.position) > seekNewTargetDistance);
        {
            currentTarget = FindSwarmMember();
        }

        

        if (currentTarget != null)
        {
            na.destination = currentTarget.transform.position;

            if (Vector3.Distance(transform.position, currentTarget.transform.position) <= attackDistance)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackDelay)
                {
                    attackTimer = 0;
                    Attack(currentTarget);
                }
            }
            else
            {
                attackTimer = 0;
            }
        }
    }

    void Attack(SwarmMember sm)
    {
        Debug.Log(name + " is damaging " + sm.name + " for " + damage + " points");
        sm.HealthData.Damage(damage);
        onAttack.Invoke();
    }
}
