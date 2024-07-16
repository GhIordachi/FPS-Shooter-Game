using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public ZombieAnimatorManager zombieAnimatorManager;
    public ZombieStatsManager zombieStatsManager;
    public ZombieCombatManager zombieCombatManager;

    public IdleState startingState;
    private State currentState;

    [Header("Flags")]
    public bool isPerformingAction;
    public bool isDead;

    [Header("Current Target")]
    public PlayerManager currentTarget;
    public Vector3 targetsDirection;
    public float distanceFromCurrentTarget;
    public float viewableAngleFromCurrentTarget;

    [Header("Components")]
    public Animator animator;
    public NavMeshAgent zombieNavMeshAgent;
    public Rigidbody zombieRigidbody;

    [Header("Movement")]
    public float rotationSpeed = 5;
    public float zombieVerticalMovement = 1f;

    [Header("Attack")]
    public float attackCooldownTimer;
    public float minimumAttackDistance = 1f; //Minimum attack distance, or the shortest range attack
    public float maximumAttackDistance = 2.5f; //Maximum attack distance, or the longest range attack

    private void Awake()
    {
        currentState = startingState;
        zombieAnimatorManager = GetComponent<ZombieAnimatorManager>();
        zombieStatsManager = GetComponent<ZombieStatsManager>();
        zombieCombatManager = GetComponent<ZombieCombatManager>();
        animator = GetComponent<Animator>();
        zombieNavMeshAgent = GetComponentInChildren<NavMeshAgent>();
        zombieRigidbody = GetComponent<Rigidbody>();
        SoundManager.instance.PlayMusic("zombie_breathing");
    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            HandleStateMachine();
        }
    }

    private void Update()
    {
        zombieNavMeshAgent.transform.localPosition = Vector3.zero;

        if(attackCooldownTimer > 0 )
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if(currentTarget != null )
        {
            targetsDirection = currentTarget.transform.position - transform.position;
            viewableAngleFromCurrentTarget = Vector3.SignedAngle(targetsDirection, transform.forward, Vector3.up);
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
    }

    private void HandleStateMachine()
    {
        State nextState;

        if(currentState != null)
        {
            nextState = currentState.Tick(this);

            if(nextState != null )
            {
                currentState = nextState;
            }
        }
    }
}
