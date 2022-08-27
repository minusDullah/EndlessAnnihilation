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

    public void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
        character = inventory.GetComponentInParent<Character>();
    }


    public string InteractionPrompt => _prompt;

    public void Interact(EAInteractor interactor)
    {
        randomNumber = Random.Range(0, weaponHolder.transform.childCount);

        weapon = weaponHolder.transform.GetChild(randomNumber).GetComponent<Weapon>();

        weapon.gameObject.transform.SetParent(inventory.transform);

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localScale = Vector3.one;

        GameObject equippedWeapon = inventory.GetEquipped().gameObject;
        equippedWeapon.transform.SetParent(weaponHolder.transform);

        inventory.weapons.RemoveAt(inventory.GetEquippedIndex());
        inventory.weapons.Insert(inventory.GetEquippedIndex(), weapon);

        equippedWeapon.SetActive(false);

        StartCoroutine(character.Equip(inventory.weapons.Count - 1 <= 0 ? 0 : inventory.weapons.Count - 1));
    }
}
