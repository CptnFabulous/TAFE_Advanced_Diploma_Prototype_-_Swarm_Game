using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
public class SwarmMember : MonoBehaviour
{
    public SwarmController controller;

    public Health HealthData { get; private set; }
    public float movementSpeed = 5;
    public float corpseCheckDistance = 2;
    public float corpseCheckMaxAngle = 45;
    public LayerMask corpseCheckDetection = ~0;

    public float corpseCheckDelay = 1f;
    float corpseCheckTimer = float.MaxValue;


    [Header("Damage")]
    public int attackDamage = 1;
    public float attackRange = 3;
    public float attackAngle = 75;
    public LayerMask hitDetection = ~0;
    public UnityEvent onAttack;

    public float attackDelay = 0.5f;
    float attackTimer = float.MaxValue;

    public Rigidbody Rigidbody { get; private set; }


    public Vector3 MovementValue { get; set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        HealthData = GetComponent<Health>();
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */


    public void MoveInDirection(Vector3 input)
    {
        MovementValue = input;
    }


    public void CountTimers()
    {
        corpseCheckTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
    }

    public void Attack()
    {
        if (attackTimer >= attackDelay)
        {
            Enemy closestEnemy = null;
            float closestAngle = attackAngle;
            float closestDistance = attackRange;
            Collider[] things = Physics.OverlapSphere(transform.position, attackRange, corpseCheckDetection);
            for (int i = 0; i < things.Length; i++)
            {
                Enemy e = things[i].GetComponentInParent<Enemy>();
                if (e != null)
                {
                    float corpseAngleRelativeToDirection = Vector3.Angle(e.transform.position - transform.position, transform.forward);
                    float corpseDistance = Vector3.Distance(e.transform.position, transform.position);

                    if (corpseAngleRelativeToDirection < closestAngle)
                    {
                        if (closestEnemy == null || corpseDistance < closestDistance)
                        {
                            closestEnemy = e;
                            closestAngle = corpseAngleRelativeToDirection;
                            closestDistance = corpseDistance;
                        }
                    }
                }
            }

            if (closestEnemy != null)
            {
                Debug.Log(name + " is attacking " + closestEnemy.name);
                closestEnemy.HealthData.Damage(attackDamage);
                onAttack.Invoke();

                attackTimer = 0;
            }
        }
    }

    public void CheckCorpseToConsume()
    {
        if (corpseCheckTimer >= corpseCheckDelay)
        {
            Corpse closestCorpse = null;
            float closestAngle = corpseCheckMaxAngle;
            float closestDistance = corpseCheckDistance;
            Collider[] corpses = Physics.OverlapSphere(transform.position, corpseCheckDistance, corpseCheckDetection);
            for (int i = 0; i < corpses.Length; i++)
            {
                Corpse c = corpses[i].GetComponentInParent<Corpse>();
                if (c != null)
                {
                    float corpseAngleRelativeToDirection = Vector3.Angle(c.transform.position - transform.position, transform.forward);
                    float corpseDistance = Vector3.Distance(c.transform.position, transform.position);

                    if (corpseAngleRelativeToDirection < closestAngle)
                    {
                        if (closestCorpse == null || corpseDistance < closestDistance)
                        {
                            closestCorpse = c;
                            closestAngle = corpseAngleRelativeToDirection;
                            closestDistance = corpseDistance;
                        }
                    }
                }
            }

            if (closestCorpse != null)
            {
                closestCorpse.ConvertIntoNewMember(this);
                corpseCheckTimer = 0;
            }
        }
    }
        
        


    private void FixedUpdate()
    {
        if (MovementValue.magnitude > 0)
        {
            Rigidbody.MoveRotation(Quaternion.LookRotation(MovementValue, controller.transform.up));
            Rigidbody.MovePosition(transform.position + (transform.forward * MovementValue.magnitude * movementSpeed * Time.fixedDeltaTime));
            //Rigidbody.MovePosition(transform.position + (MovementValue * movementSpeed * Time.fixedDeltaTime));
        }
    }


}
