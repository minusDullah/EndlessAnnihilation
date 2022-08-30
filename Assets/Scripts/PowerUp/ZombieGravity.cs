using UnityEngine;
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;

public class ZombieGravity : MonoBehaviour
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
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < zombieHolder.transform.childCount; i++)
            {
                enemyMovement.Add(zombieHolder.transform.GetChild(i).GetComponent<EnemyMovement>());
            }

            foreach (EnemyMovement enemyMovement in enemyMovement)
            {
                enemyMovement.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                enemyMovement.navAgent.speed *= 0;
                enemyMovement.DisablePhysics();
                enemyMovement.rb.useGravity = true;
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
        {
            enemyMovement.rb.constraints = RigidbodyConstraints.None;
            enemyMovement.GetComponent<Target>().TakeDamage(999);
            enemyMovement.rb.isKinematic = true;
        }

        Destroy(gameObject);
    }
}
