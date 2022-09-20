using System.Collections;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class DoubleDamagePowerUp : MonoBehaviour
{
    [Header("Powerup Stats")]
    [SerializeField] private float damageMultiplier = 2f;
    [SerializeField] private float PerkCooldown = 15f;
    [SerializeField] private float destroyTimer = 15f;

    [Header("Rotation")]
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float randomRotationX;
    [SerializeField] private float randomRotationY;
    [SerializeField] private float randomRotationZ;

    [Header("References")]
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private SphereCollider sphereCollider;

    private Inventory inventory;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
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
            foreach (Weapon currWeapon in inventory.weapons)
            {
                currWeapon.damagePerBullet *= damageMultiplier;
            }
            StartCoroutine(PerkLength());
        }
    }

    IEnumerator PerkLength()
    {
        sphereCollider.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(PerkCooldown);
        foreach (Weapon currWeapon in inventory.weapons)
        {
            currWeapon.damagePerBullet /= damageMultiplier;
        }
        Destroy(gameObject, .5f);
    }
}
