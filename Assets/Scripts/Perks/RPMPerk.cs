using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPMPerk : MonoBehaviour, IInteractable
{
    [Header("Prompt")]
    [SerializeField] private string _prompt;

    [Header("Cost")]
    [SerializeField] private int perkCost = 2500;

    [Header("Weapon")]
    [SerializeField] private float rateOfFireMultiplier = 2;

    [Header("References")]
    [SerializeField] private bool alreadyBought = false;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private Weapon currWeapon;

    public string InteractionPrompt => _prompt;

    public void Interact(EAInteractor interactor)
    {
        if (alreadyBought)
            return;

        if (scoreUI.scoreTotal < perkCost)
            return;

        scoreUI.UpdateScoreLose(perkCost);

        alreadyBought = true;

        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            currWeapon = inventory.transform.GetChild(i).GetComponent<Weapon>();
            currWeapon.RPMPerkActive(rateOfFireMultiplier);
        }

        for (int i = 0; i < weaponHolder.transform.childCount; i++)
        {
            currWeapon = weaponHolder.transform.GetChild(i).GetComponent<Weapon>();
            currWeapon.RPMPerkActive(rateOfFireMultiplier);
        }
    }
}
