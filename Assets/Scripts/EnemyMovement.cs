using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private HealthController playerHealth;
    private WaveSpawner waveSpawner;
    private GameObject player;

    [SerializeField] public NavMeshAgent navAgent;
    [SerializeField] public Animator animator;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public CapsuleCollider capsuleCollider;
    [SerializeField] public Target target; 

    [Header("Enemy Stats")]
    [SerializeField] public float enemyDamage = 25f;
    [SerializeField] public float minSpeed = 1f;
    [SerializeField] public float maxSpeed = 15f;
    [SerializeField] public float randomSpeed;
    [SerializeField] public float attackCooldown = .8f;
    [SerializeField] public float animationBuffer = .3f;
    [SerializeField] public bool triggerEntered = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthController>();
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();

        rb.isKinematic = true;
        randomSpeed = Random.Range(waveSpawner.currWave/1.5F, waveSpawner.currWave);
        randomSpeed = Mathf.Clamp(randomSpeed, minSpeed, maxSpeed);
        navAgent.speed = randomSpeed;
        animator.CrossFade(MovingAnim(), .5f, 0);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") || animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            DisablePhysics();
        }

        enemyDestination();
    }

    public void DisablePhysics()
    {
        animator.enabled = false;
        capsuleCollider.enabled = false;
        rb.isKinematic = false;
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !waveSpawner.enemiesFrozen && animator.enabled)
        {
            triggerEntered = true;
            navAgent.speed = 0;
            while (triggerEntered && GetComponent<Target>().health > 0)
            {
                ChooseAttackAnimation();
                yield return new WaitForSeconds(animationBuffer);
                DealDamage();
                yield return new WaitForSeconds(attackCooldown);
            }
        }
    }

    IEnumerator OnTriggerExit(Collider other)
    {
        yield return new WaitForSeconds(animationBuffer);
        animator.CrossFade(MovingAnim(), animationBuffer, 0);
        navAgent.speed = randomSpeed;
        triggerEntered = false;
    }

    private void enemyDestination()
    {
        navAgent.SetDestination(player.transform.position);
    }

    private void DealDamage()
    {
        playerHealth.TakeDamage(enemyDamage);
    }    

    private void ChooseAttackAnimation()
    {
        int atkAnim = Random.Range(0, 2);
        if (atkAnim == 0) { animator.CrossFade("Attack_Left", 0f, 0); }
        if (atkAnim == 1) { animator.CrossFade("Attack_Right", 0f, 0); }
    }

    public string MovingAnim()
    {
        if (navAgent.speed <= 2.5)
        {
            return "Slow_Walk";
        }
        else if (navAgent.speed > 2.5 && navAgent.speed <= 4.5)
        {
            int runAnim = Random.Range(0, 2);
            if (runAnim == 0) { return "Walk"; }
            else return "Walk_2";
        }
        else
        {
            return "Run";
        }
    }
}
