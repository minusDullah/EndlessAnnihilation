using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] public float health = 100f;
    [SerializeField] public float minHealth = 100f;
    [SerializeField] public float maxHealth = 999f;
    [SerializeField] public float scoreWorth = 50;
    [SerializeField] private int destroyTimer = 15;

    [Header("References")]
    [SerializeField] private GameObject bloodFX;
    [SerializeField] private Transform bloodFXPosition;
    [SerializeField] private GameObject minimapIcon;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] public ScoreUpdate scoreUpdate;

    [Header("Audio")]
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioClip[] enemyDie;

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private GameObject player;
    private bool pointsGained;
    private bool soundPlayed;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAudioSource = GetComponent<AudioSource>();
        scoreUpdate = player.GetComponentInChildren<ScoreUpdate>();
        
        pointsGained = false;
        soundPlayed = false;
        health *= waveSpawner.currWave/4;
        health = Mathf.Clamp(health, minHealth, maxHealth);
    }

    public void TakeDamage(float damage, ScoreUpdate scoreUpdate)
    {
        health -= damage;
        if (health <= 0)
        {
            waveSpawner.totalKills++;
            waveSpawner.buffKillCounter -= 1;
            waveSpawner.timeRemaining += 2f;
            SetAllCollidersStatus();
            DeathSound();
            navMeshAgent.isStopped = true;
            int dieAnim = Random.Range(0, 2);
            if (dieAnim == 0) { animator.Play("Dying", 0); }
            if (dieAnim == 1) { animator.Play("Death", 0); }
            ScoreOnDeath();
            FXonDeath();
            RemoveFromMinimap();
            ChanceOfPowerUp(350);
        }
    }

    private void ChanceOfPowerUp(int range)
    {
        int randomPowerUp = Random.Range(0, range);
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z);
        if (randomPowerUp <= 5) { Instantiate(waveSpawner.powerUps[0], spawnPos, Quaternion.identity); }
        if(randomPowerUp > 5 && randomPowerUp <= 10) { Instantiate(waveSpawner.powerUps[1], spawnPos, Quaternion.identity); }
        if(randomPowerUp > 10 && randomPowerUp <= 15) { Instantiate(waveSpawner.powerUps[2], spawnPos, Quaternion.identity); }
        if(randomPowerUp > 15 && randomPowerUp <= 20) { Instantiate(waveSpawner.powerUps[3], spawnPos, Quaternion.identity); }
        if(randomPowerUp > 20 && randomPowerUp <= 25) { Instantiate(waveSpawner.powerUps[4], spawnPos, Quaternion.identity); }
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
