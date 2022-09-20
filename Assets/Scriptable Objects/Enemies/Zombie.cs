using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class Zombie : MonoBehaviour, IDamageable
{
    [SerializeField] ZombieScriptable zombieScriptable;

    [Header("Audio")]
    [SerializeField] private AudioSource enemyAudioSource;
   
    [Header("Self references")]
    [SerializeField] private Animator animator;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public NavMeshAgent navAgent;
    [SerializeField] public CapsuleCollider capsuleCollider;

    private WaveSpawner waveSpawner;
    [SerializeField] private ScoreUpdate scoreUpdateReference;
    private GameObject target;

    private bool soundPlayed = false;
    private bool pointsGained = false;
    public bool triggerEntered = false;

    private float randomSpeed;
    

    private void Start()
    {
        CachePlayers();

        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();

        zombieScriptable.health *= waveSpawner.currWave / 4;
        zombieScriptable.health = Mathf.Clamp(zombieScriptable.health, zombieScriptable.minHealth, zombieScriptable.maxHealth);

        rb.isKinematic = true;

        randomSpeed = Random.Range(waveSpawner.currWave / 1.5F, waveSpawner.currWave);
        randomSpeed = Mathf.Clamp(randomSpeed, zombieScriptable.minSpeed, zombieScriptable.maxSpeed);
        navAgent.speed = randomSpeed;

        animator.CrossFade(MovingAnim(), .5f, 0);
    }

    private void Update()
    {
        if(zombieScriptable.health <= 0)
        {
            PlayDeathSound(Random.Range(0, zombieScriptable.enemyDie.Length));
            PlayDeathAnimation(Random.Range(0, 2));
            ChanceOfPowerUp(Random.Range(0, 100));
            RemoveFromMinimap();
            ScoreOnDeath();
            UntagColliders();
            DisablePhysics();
        }
    }

    private void FixedUpdate()
    {
        CachePlayers();
        ZombieDestination();
    }

    #region Health And Dying

    public void TakeDamage(float damage, ScoreUpdate scoreUpdate)
    {
        zombieScriptable.health -= damage;
        scoreUpdateReference = scoreUpdate;
    }

    private void PlayDeathSound(int clipToPlay)
    {
        if (!enemyAudioSource.isPlaying && soundPlayed == false)
        {
            enemyAudioSource.clip = zombieScriptable.enemyDie[clipToPlay];
            enemyAudioSource.Play();
            soundPlayed = true;
        }
    }

    private void RemoveFromMinimap()
    {
        if (zombieScriptable.minimapIcon.activeInHierarchy == true)
        {
            zombieScriptable.minimapIcon.SetActive(false);
        }
    }

    private void PlayDeathAnimation(int dieAnim)
    {
        if (dieAnim == 0) { animator.Play("Dying", 0); }
        if (dieAnim == 1) { animator.Play("Death", 0); }
    }

    private void ChanceOfPowerUp(int randomPowerUp)
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z);
        if(zombieScriptable.chanceOfPowerUp <= randomPowerUp) { Instantiate(zombieScriptable.powerUps[Random.Range(0, zombieScriptable.powerUps.Length)], spawnPos, Quaternion.identity); return; }
    }

    private void ScoreOnDeath()
    {
        if (pointsGained == false)
        {
            scoreUpdateReference.CalculateScore(zombieScriptable.scoreWorth);
            pointsGained = true;
        }
    }

    public void UntagColliders()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            //c.enabled = active;
            if (collider.gameObject.tag != null)
            {
                collider.gameObject.tag = "Untagged";
            }
        }
    }

    /*
    private void FXonDeath()
    {
        GameObject bloodfx = Instantiate(bloodFX, bloodFXPosition.position, Quaternion.identity);
        bloodfx.transform.parent = bloodFXPosition;
        Destroy(bloodfx, 3f);
        Destroy(gameObject, destroyTimer);
    }
    */

    #endregion

    #region Damage and Chasing

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !waveSpawner.enemiesFrozen && animator.enabled)
        {
            triggerEntered = true;
            navAgent.speed = 0;
            while (triggerEntered && zombieScriptable.health > 0)
            {
                ChooseAttackAnimation(Random.Range(0, 2));
                yield return new WaitForSeconds(zombieScriptable.animationBuffer);
                DealDamage();
                yield return new WaitForSeconds(zombieScriptable.attackCooldown);
            }
        }
    }

    IEnumerator OnTriggerExit(Collider other)
    {
        yield return new WaitForSeconds(zombieScriptable.animationBuffer);
        animator.CrossFade(MovingAnim(), zombieScriptable.animationBuffer, 0);
        navAgent.speed = randomSpeed;
        triggerEntered = false;
    }

    private void DealDamage()
    {
        target.GetComponentInParent<HealthController>().TakeDamage(zombieScriptable.damage);
    }

    private void ZombieDestination()
    {
        if (target == null)
            return;

        if (navAgent == null)
            return;

        navAgent.SetDestination(target.transform.position);
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
            return "Walk_2";
        }
        else
        {
            return "Run";
        }
    }

    private void ChooseAttackAnimation(int atkAnim)
    {
        if (atkAnim == 0) { animator.CrossFade("Attack_Left", 0f, 0); }
        if (atkAnim == 1) { animator.CrossFade("Attack_Right", 0f, 0); }
    }

    public void DisablePhysics()
    {
        animator.enabled = false;
        capsuleCollider.enabled = false;
        rb.isKinematic = false;
    }

    private void CachePlayers()
    {
        // get list of objects with tag "Player"
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        // get closest transform from gos[] array, into target variable, as transform object
        target = gos.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).First();
    }

    #endregion
}
