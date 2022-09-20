using UnityEngine;
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;

public class ZombieFreezePowerUp : MonoBehaviour
{
    [Header("Powerup Stats")]
    public List<EnemyMovement> enemyMovement;
    [SerializeField] private float PerkCooldown = 5f;
    [SerializeField] private float destroyTimer = 15f;

    [Header("Rotation")]
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float randomRotationX;
    [SerializeField] private float randomRotationY;
    [SerializeField] private float randomRotationZ;

    [Header("References")]
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private SphereCollider sphereCollider;

    private GameObject zombieHolder;
    private WaveSpawner waveSpawner;

    private void Start()
    {
        zombieHolder = GameObject.FindGameObjectWithTag("ZombieHolder");
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();
        randomRotationX = Random.Range(-rotateSpeed, rotateSpeed);
        randomRotationY = Random.Range(-rotateSpeed, rotateSpeed);
        randomRotationZ = Random.Range(-rotateSpeed, rotateSpeed);
        Destroy(gameObject, destroyTimer);
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(randomRotationX, randomRotationY, randomRotationZ), Space.World);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (waveSpawner.enemiesFrozen)
            Destroy(gameObject);

        if (other.CompareTag("Player"))
        {
            waveSpawner.enemiesFrozen = true;
            for (int i = 0; i < zombieHolder.transform.childCount; i++)
            {
                enemyMovement.Add(zombieHolder.transform.GetChild(i).GetComponent<EnemyMovement>());
            }

            foreach (EnemyMovement enemyMovement in enemyMovement)
            {
                enemyMovement.navAgent.speed *= 0;
                enemyMovement.navAgent.isStopped = true;
                enemyMovement.animator.CrossFade("Idle", .5f, 0);
                enemyMovement.capsuleCollider.enabled = false;
                enemyMovement.triggerEntered = false;
            }
                
            StartCoroutine(PerkLength());
        }
    }

    IEnumerator PerkLength()
    {
        sphereCollider.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(PerkCooldown);

        foreach (EnemyMovement enemyMovement in enemyMovement)
            if(enemyMovement.navAgent != null && enemyMovement.GetComponent<Target>().health > 0)
            {
                enemyMovement.navAgent.isStopped = false;
                enemyMovement.navAgent.speed = enemyMovement.randomSpeed;
                enemyMovement.capsuleCollider.enabled = true;
                enemyMovement.animator.CrossFade(enemyMovement.MovingAnim(), .5f, 0);
            }
            
        waveSpawner.enemiesFrozen = false;
        Destroy(gameObject);
    }
}
