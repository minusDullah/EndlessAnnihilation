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
    [SerializeField] private Inventory inventory;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private SphereCollider sphereCollider;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        sphereCollider = GetComponent<SphereCollider>();
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
            Weapon currWeapon = inventory.GetEquipped().GetComponent<Weapon>();
            currWeapon.damagePerBullet *= damageMultiplier;
            StartCoroutine(PerkLength(currWeapon));
        }
    }

    IEnumerator PerkLength(Weapon currWeapon)
    {
        sphereCollider.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(PerkCooldown);
        currWeapon.damagePerBullet /= damageMultiplier;
        Destroy(gameObject);
    }
}
