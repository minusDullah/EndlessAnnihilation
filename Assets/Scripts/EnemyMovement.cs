using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Transform playerTransform;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        animator.SetFloat("Speed", navAgent.speed);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") || animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Invoke("Disable", .5f);
        }
        else
        {
            if (Vector3.Distance(gameObject.transform.position, playerTransform.position) < 4)
            {
                animator.SetBool("Attacking", true);
                //player taking damage
            }
            else
            {
                animator.SetBool("Attacking", false);
                Invoke("enemyDestination", 2.2f);
            }
        }
    }

    void enemyDestination()
    {
        navAgent.SetDestination(playerTransform.position);
    }

    void Disable()
    {
        if (animator.isActiveAndEnabled)
        {
            animator.enabled = !animator.enabled;
            rb.isKinematic = false;
            capsuleCollider.enabled = !capsuleCollider.enabled;
        }
    }
}
