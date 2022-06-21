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
    [SerializeField] private float attackRange = 4f;
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float attackCooldown = .8f;
    [SerializeField] private float animationBuffer = .3f;
    [SerializeField] private bool triggerEntered = false;

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
        animator.CrossFade(movingAnim(), 2, 0);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") || animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Invoke("DisableRB", 1f);
            DisableCollider();
        }
        else
        {
            enemyDestination();
        }
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerEntered = true;
            while (triggerEntered)
            {
                animator.CrossFade("Attack", .05f, 0);
                yield return new WaitForSeconds(animationBuffer);
                dealDamage();
                yield return new WaitForSeconds(attackCooldown);
            }
            yield break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.CrossFade(movingAnim(), 1.3f, 0);
            triggerEntered = false;
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

    void dealDamage()
    {
        playerHealth.currentPlayerHealth -= enemyDamage;
        playerHealth.TakeDamage();
    }

    string movingAnim()
    {
        if (navAgent.speed < 4)
        {
            return "Walk";
        }
        else
        {
            return "Run";
        }
    }
}
