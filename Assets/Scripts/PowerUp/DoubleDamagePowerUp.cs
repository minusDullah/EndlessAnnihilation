using System.Collections;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class DoubleDamagePowerUp : MonoBehaviour
{
    [Header("Perk Stats")]
    [SerializeField] private float damageMultiplier = 2f;
    [SerializeField] private float PerkCooldown = 15f;
    [SerializeField] private Inventory inventory;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private BoxCollider boxCollider;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
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
        boxCollider.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(PerkCooldown);
        currWeapon.damagePerBullet /= damageMultiplier;
        Destroy(gameObject);
    }
}
