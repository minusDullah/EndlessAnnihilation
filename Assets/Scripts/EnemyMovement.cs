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

    [SerializeField] private float enemyDamage = 25f;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();
        rb.isKinematic = true;
        navAgent.speed = (waveSpawner.currWave*1.5f);
        navAgent.speed = Mathf.Clamp(navAgent.speed, 1f, 15f);
        animator.SetFloat("Speed", navAgent.speed);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") || animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Invoke("DisableRB", .5f);
            DisableCollider();
        }
        else
        {
            if (Vector3.Distance(gameObject.transform.position, playerTransform.position) < 4)
            {
                animator.SetBool("Attacking", true);
            }
            else
            {
                animator.SetBool("Attacking", false);
                Invoke("enemyDestination", 2.2f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth.currentPlayerHealth -= enemyDamage;
            playerHealth.TakeDamage();
        }
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
}
