using UnityEngine;
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;

public class ZombieFreezePowerUp : MonoBehaviour
{
    [Header("Perk Stats")]
    public List<EnemyMovement> enemyMovement;
    [SerializeField] private float PerkCooldown = 5f;
    [SerializeField] private GameObject zombieHolder;
    [SerializeField] private WaveSpawner waveSpawner;

    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private SphereCollider sphereCollider;

    private void Start()
    {
        zombieHolder = GameObject.FindGameObjectWithTag("ZombieHolder");
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();
        mesh = GetComponent<MeshRenderer>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (waveSpawner.enemiesFrozen)
            return;

        if (other.CompareTag("Player"))
        {
            waveSpawner.enemiesFrozen = true;
            for (int i = 0; i < zombieHolder.transform.childCount; i++)
            {
                enemyMovement.Add(zombieHolder.transform.GetChild(i).GetComponent<EnemyMovement>());
            }

            foreach (EnemyMovement enemyMovement in enemyMovement)
                enemyMovement.navAgent.speed *= 0;

            StartCoroutine(PerkLength());
        }
    }

    IEnumerator PerkLength()
    {
        sphereCollider.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(PerkCooldown);

        foreach (EnemyMovement enemyMovement in enemyMovement)
            if(enemyMovement.navAgent != null)
            {
                enemyMovement.navAgent.speed = enemyMovement.randomSpeed;
            }
            
        waveSpawner.enemiesFrozen = false;
        Destroy(gameObject);
    }
}
