using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CapsuleCollider characterBlockerCollider;

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
        navAgent.speed = (waveSpawner.currWave * speedMultiplier);
        navAgent.speed = Mathf.Clamp(navAgent.speed, minSpeed, maxSpeed);
        animator.CrossFade(movingAnim(), 1, 0);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { return; }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") || animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Invoke("DisableRB", 1f);
            DisableColliders();
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
                int atkAnim = Random.Range(0, 2);
                if (atkAnim == 0) { animator.CrossFade("Attack_Left", .05f, 0); }
                if (atkAnim == 1) { animator.CrossFade("Attack_Right", .05f, 0); }
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

    void DisableColliders()
    {
        if (animator.isActiveAndEnabled)
        {
            capsuleCollider.enabled = false;
            characterBlockerCollider.enabled = false;
        }
    }

    void dealDamage()
    {
        playerHealth.TakeDamage(enemyDamage);
    }

    string movingAnim()
    {
        if (navAgent.speed <= 3) //round 1 + 2
        {
            return "Slow_Walk";
        }
        else if (navAgent.speed > 3 && navAgent.speed <= 7.5)//round 3,4,5
        {
            int runAnim = Random.Range(0, 3);
            if (runAnim == 0) { return "Walk"; }
            else if (runAnim == 1) { return "Walk_2"; }
            else { return "Walk_Aggressive"; }
        }
        else
        {
            return "Run";
        }
    }
}
