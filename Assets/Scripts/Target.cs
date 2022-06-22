using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] public float health = 100f;
    [SerializeField] private int scoreWorth = 50;
    [SerializeField] private int destroyTimer = 15;

    [Header("References")]
    [SerializeField] private GameObject bloodFX;
    [SerializeField] private Transform bloodFXPosition;
    [SerializeField] private GameObject minimapIcon;
    public ScoreUpdate scoreUpdate;

    [Header("Audio")]
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioClip[] enemyDie;

    
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private bool pointsGained;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAudioSource = GetComponent<AudioSource>();
        scoreUpdate = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUpdate>();
        pointsGained = false;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DeathSound();
            navMeshAgent.Stop();
            int dieAnim = Random.Range(0, 2);
            if(dieAnim == 0) { animator.Play("Dying", 0); }
            if(dieAnim == 1) { animator.Play("Death", 0); }
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

    private void DeathSound()
    {
        int clipToPlay = Random.Range(0, enemyDie.Length);
        enemyAudioSource.clip = enemyDie[clipToPlay];
        enemyAudioSource.Play();
    }
}
