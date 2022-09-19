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
    [SerializeField] private GameObject player;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private int randomNumber;
    [SerializeField] private int mysteryWeaponCost = 5000;
    [SerializeField] private bool cooldownOff = true;

    private Weapon weapon;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scoreUI = player.GetComponent<ScoreUpdate>();
        inventory = player.GetComponentInChildren<Inventory>();
        character = player.GetComponent<Character>();
    }

    public string InteractionPrompt => _prompt;

    public void Interact(EAInteractor interactor)
    {
        if (character.IsInspecting() || character.IsInvoking())
                return;

        if (!cooldownOff)
                return;
        
        if (scoreUI.scoreTotal < mysteryWeaponCost)
                return;

        scoreUI.UpdateScoreLose(mysteryWeaponCost);

        cooldownOff = false;

        randomNumber = Random.Range(0, weaponHolder.transform.childCount);

        weapon = weaponHolder.transform.GetChild(randomNumber).GetComponent<Weapon>();

        weapon.gameObject.transform.SetParent(inventory.transform);


        if (inventory.GetEquippedIndex() != weapon.transform.GetSiblingIndex())
        {
            weapon.transform.SetSiblingIndex(inventory.GetEquippedIndex());
        }

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

        StartCoroutine(character.Equip(inventory.GetEquippedIndex()));

        StartCoroutine(DisableWeapon(equippedWeapon));

        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(2f);
        cooldownOff = true;
    }   

    IEnumerator DisableWeapon(GameObject equippedWeapon)
    {
        yield return new WaitForSeconds(.3f);
        Weapon currWeapon = equippedWeapon.GetComponent<Weapon>();
        currWeapon.AddAmmunitionInventoryAmount(currWeapon.ammunitionMax);
        currWeapon.FillAmmunition(currWeapon.ammunitionMax);
        equippedWeapon.transform.SetParent(weaponHolder.transform);
        equippedWeapon.SetActive(false);
        character.weaponAttachment.laserIndex = -1;
        character.weaponAttachment.gripIndex = -1;
        character.weaponAttachment.muzzleIndex = 0;
        character.weaponAttachment.scopeIndex = -1;
        equippedWeapon.GetComponent<Weapon>().UpdateWeaponBehaviour();
        character.WeaponAttachmentUpdate();
    }
}
