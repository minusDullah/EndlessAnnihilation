using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private HealthController playerHealth;
    private Transform playerTransform;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private WaveSpawner waveSpawner;

    [Header("Enemy Stats")]
    [SerializeField] private float enemyDamage = 25f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private bool stillHitting = false;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();

        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        
        rb.isKinematic = true;
        navAgent.speed = (waveSpawner.currWave* speedMultiplier);
        navAgent.speed = Mathf.Clamp(navAgent.speed, minSpeed, maxSpeed);
        animator.SetFloat("Speed", navAgent.speed);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
        {
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") || animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Invoke("DisableRB", .5f);
            DisableCollider();
        }
        else
        {
            if (Vector3.Distance(gameObject.transform.position, playerTransform.position) < attackRange)
            {
                animator.SetBool("Attacking", true);
            }
            else
            {
                animator.SetBool("Attacking", false);
                enemyDestination();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dealDamage();
        }
    }    

    private void OnTriggerExit(Collider other)
    {
        CancelInvoke();
    }

    void enemyDestination()
    {
        navAgent.SetDestination(playerTransform.position);
    }

    void DisableRB()
    {
        if (animator.isActiveAndEnabled)
        {
            animator.enabled = false;
            rb.isKinematic = false;
        }
    }

    void DisableCollider()
    {
        if (animator.isActiveAndEnabled)
        {
            capsuleCollider.enabled = false;
        }
    }

    void dealDamage()
    {
        playerHealth.currentPlayerHealth -= enemyDamage;
        playerHealth.TakeDamage();
    }
}
