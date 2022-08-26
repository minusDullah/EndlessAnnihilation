using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] public float health = 100f;
    [SerializeField] public int scoreWorth = 50;
    [SerializeField] private int destroyTimer = 15;

    [Header("References")]
    [SerializeField] private GameObject bloodFX;
    [SerializeField] private Transform bloodFXPosition;
    [SerializeField] private GameObject minimapIcon;
    [SerializeField] private WaveSpawner waveSpawner;
    public ScoreUpdate scoreUpdate;

    [Header("Audio")]
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioClip[] enemyDie;

    
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private bool pointsGained;
    private bool soundPlayed;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAudioSource = GetComponent<AudioSource>();
        scoreUpdate = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUpdate>();
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();
        pointsGained = false;
        soundPlayed = false;
        pointsGained = false;
        health *= waveSpawner.currWave/4;
        health = Mathf.Clamp(health, 100, 500);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            waveSpawner.totalKills++;
            waveSpawner.timeRemaining += 5f;
            SetAllCollidersStatus();
            DeathSound();
            navMeshAgent.isStopped = true;
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
            scoreUpdate.CalculateScore(scoreWorth);
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
        if (!enemyAudioSource.isPlaying && soundPlayed == false)
        {
            int clipToPlay = Random.Range(0, enemyDie.Length);
            enemyAudioSource.clip = enemyDie[clipToPlay];
            enemyAudioSource.Play();
            soundPlayed = true;
        }
    }

    public void SetAllCollidersStatus()
    {
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            //c.enabled = active;
            if (c.gameObject.tag != null)
            {
                c.gameObject.tag = "Untagged";
            }
            
        }
    }
}
