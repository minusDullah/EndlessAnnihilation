using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSpeedPerk : MonoBehaviour, IInteractable
{
    [Header("Prompt")]
    [SerializeField] private string _prompt;

    [Header("Cost")]
    [SerializeField] private int perkCost = 2500;

    [Header("Weapon")]
    [SerializeField] private float reloadSpeedMultiplier = 2;

    [Header("References")]
    [SerializeField] private bool alreadyBought = false;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Character character;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private GameObject player;
    [SerializeField] private Weapon currWeapon;

    public string InteractionPrompt => _prompt;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weaponHolder = GameObject.FindGameObjectWithTag("Weapon");
        scoreUI = player.GetComponentInChildren<ScoreUpdate>();
        inventory = GetComponentInChildren<Inventory>();
        character = GetComponent<Character>();
    }

    public void Interact(EAInteractor interactor)
    {
        if (alreadyBought)
            return;

        if (scoreUI.scoreTotal < perkCost)
            return;

        scoreUI.UpdateScoreLose(perkCost);

        alreadyBought = true;

        character.characterAnimator.speed *= reloadSpeedMultiplier;

        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            currWeapon = inventory.transform.GetChild(i).GetComponent<Weapon>();
            currWeapon.ReloadSpeedPerkActive(reloadSpeedMultiplier);
        }

        for (int i = 0; i < weaponHolder.transform.childCount; i++)
        {
            currWeapon = weaponHolder.transform.GetChild(i).GetComponent<Weapon>();
            currWeapon.ReloadSpeedPerkActive(reloadSpeedMultiplier);
        }
    }
}
