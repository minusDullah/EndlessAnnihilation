using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class MaxAmmoPowerUp : MonoBehaviour
{

    [SerializeField] private Inventory inventory;
    [SerializeField] private Weapon currWeapon;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < inventory.transform.childCount; i++)
            {
                currWeapon = inventory.transform.GetChild(i).GetComponent<Weapon>();
                currWeapon.AddAmmunitionInventoryAmount(currWeapon.ammunitionMax);
            }
            Destroy(gameObject);
        }
    }
}
