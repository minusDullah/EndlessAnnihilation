using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryWeapon : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Character character;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private Weapon weapon;
    [SerializeField] private int randomNumber;
    [SerializeField] private bool cooldownOff = true;

    public void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
        character = inventory.GetComponentInParent<Character>();
    }


    public string InteractionPrompt => _prompt;

    public void Interact(EAInteractor interactor)
    {
        if (!cooldownOff)
            return;

        cooldownOff = false;

        randomNumber = Random.Range(0, weaponHolder.transform.childCount);

        weapon = weaponHolder.transform.GetChild(randomNumber).GetComponent<Weapon>();

        weapon.gameObject.transform.SetParent(inventory.transform);

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localScale = Vector3.one;

        if (inventory.weapons.Count == 1)
        {
            inventory.weapons.Capacity = 2;
            inventory.weapons.Insert(inventory.GetEquippedIndex()+1, weapon);
            StartCoroutine(character.Equip(1));
            StartCoroutine(CoolDown());
            return;
        }

        GameObject equippedWeapon = inventory.GetEquipped().gameObject;

        inventory.weapons.RemoveAt(inventory.GetEquippedIndex());
        inventory.weapons.Insert(inventory.GetEquippedIndex(), weapon);

        StartCoroutine(character.Equip(inventory.weapons.Count - 1 <= 0 ? 0 : inventory.weapons.Count - 1));

        StartCoroutine(DisableWeapon(equippedWeapon));

        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1.25f);
        cooldownOff = true;
    }

    IEnumerator DisableWeapon(GameObject equippedWeapon)
    {
        yield return new WaitForSeconds(.3f);
        equippedWeapon.transform.SetParent(weaponHolder.transform);
        equippedWeapon.SetActive(false);
    }
}
