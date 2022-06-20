using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] public float health = 100f;
    [SerializeField] private int scoreWorth = 50;
    [SerializeField] private int randomDeath;
    [SerializeField] private int destroyTimer = 15;

    [Header("References")]
    [SerializeField] private GameObject bloodFX;
    [SerializeField] private Transform bloodFXPosition;
    [SerializeField] private GameObject minimapIcon;

    public ScoreUpdate scoreUpdate;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private bool pointsGained;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        scoreUpdate = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUpdate>();
        randomDeath = Random.Range(0, 2);
        animator.SetInteger("randomDeath", randomDeath);
        pointsGained = false;
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
            ScoreOnDeath();
            FXonDeath();
            RemoveFromMinimap();
        }
    }

    private void ScoreOnDeath()
    {
        if(pointsGained == false)
        {
            scoreUpdate.scoreTotal += scoreWorth;
            scoreUpdate.updateScore();
            pointsGained = true;
        }
    }

    private void FXonDeath()
    {
        GameObject bloodfx = Instantiate(bloodFX, bloodFXPosition.position, Quaternion.identity);
        bloodfx.transform.parent = bloodFXPosition;
        Destroy(bloodfx, 3f);
        Destroy(gameObject, destroyTimer);
    }

    private void RemoveFromMinimap()
    {
        if (minimapIcon.activeInHierarchy == true)
        {
            minimapIcon.SetActive(false);
        }
        
    }
}
