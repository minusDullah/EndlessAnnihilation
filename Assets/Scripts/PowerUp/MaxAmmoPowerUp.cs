using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class MaxAmmoPowerUp : MonoBehaviour
{
    [Header("Powerup Stats")]
    [SerializeField] private float destroyTimer = 15f;
    [SerializeField] private float rotateSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float randomRotationX;
    [SerializeField] private float randomRotationY;
    [SerializeField] private float randomRotationZ;

    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Weapon currWeapon;

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
            for (int i = 0; i < inventory.transform.childCount; i++)
            {
                currWeapon = inventory.transform.GetChild(i).GetComponent<Weapon>();
                currWeapon.AddAmmunitionInventoryAmount(currWeapon.ammunitionMax);
            }
            Destroy(gameObject);
        }
    }
}
