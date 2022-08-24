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
    private GameObject _player;

    [Header("Enemy Stats")]
    [SerializeField] private float enemyDamage = 25f;
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float attackCooldown = .8f;
    [SerializeField] private float animationBuffer = .3f;
    [SerializeField] private bool triggerEntered = false;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = _player.transform;
        playerHealth = _player.GetComponent<HealthController>();
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();

        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        float randomSpeed = Random.Range(waveSpawner.currWave/1.5F, waveSpawner.currWave);
        randomSpeed = Mathf.Clamp(randomSpeed, minSpeed, maxSpeed);
        navAgent.speed = randomSpeed;
        animator.CrossFade(MovingAnim(), 1, 0);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { return; }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") || animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            DisablePhysics();
        }
        else
        {
            enemyDestination();
        }
    }

    private void DisablePhysics()
    {
        animator.enabled = false;
        capsuleCollider.enabled = false;
        rb.isKinematic = false;
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerEntered = true;
            while (triggerEntered && GetComponent<Target>().health > 0)
            {
                ChooseAnimation();
                yield return new WaitForSeconds(animationBuffer);
                DealDamage();
                yield return new WaitForSeconds(attackCooldown);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.CrossFade(MovingAnim(), 1.3f, 0);
        triggerEntered = false;
    }

    void enemyDestination()
    {
        navAgent.SetDestination(playerTransform.position);
    }

    void DealDamage()
    {
        playerHealth.TakeDamage(enemyDamage);
    }

    void ChooseAnimation()
    {
        int atkAnim = Random.Range(0, 2);
        if (atkAnim == 0) { animator.CrossFade("Attack_Left", .05f, 0); }
        if (atkAnim == 1) { animator.CrossFade("Attack_Right", .05f, 0); }
    }

    string MovingAnim()
    {
        if (navAgent.speed <= 2)
        {
            return "Slow_Walk";
        }
        else if (navAgent.speed > 2 && navAgent.speed <= 4)
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
