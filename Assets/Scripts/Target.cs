using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] public float health = 100;
    [SerializeField] private int scoreWorth = 50;
    [SerializeField] private int randomDeath;
    [SerializeField] private GameObject bloodFX;
    [SerializeField] private Transform bloodFXPosition;

    public ScoreUpdate scoreUpdate;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        scoreUpdate = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUpdate>();
        randomDeath = Random.Range(0, 2);
        animator.SetInteger("randomDeath", randomDeath);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetFloat("Health", health);
        if (health <= 0)
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            navMeshAgent.Stop();
            #pragma warning restore CS0618 // Type or member is obsolete
            animator.SetBool("Dead", true);
            scoreUpdate.scoreTotal += scoreWorth;
            scoreUpdate.updateScore();
            GameObject bloodfx = Instantiate(bloodFX, bloodFXPosition.position, Quaternion.identity);
            bloodfx.transform.parent = bloodFXPosition;
            Destroy(bloodfx, 3f);
            Destroy(gameObject, 15f);
        }
    }

}
