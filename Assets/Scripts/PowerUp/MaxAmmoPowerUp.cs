using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class MaxAmmoPowerUp : MonoBehaviour
{

    [SerializeField] private Inventory inventory;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Weapon currWeapon = inventory.GetEquipped().GetComponent<Weapon>();
            currWeapon.AddAmmunitionInventoryAmount(currWeapon.ammunitionMax);
            Destroy(gameObject);
        }
    }
}
