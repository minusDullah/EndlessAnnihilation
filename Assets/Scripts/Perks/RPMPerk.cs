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
    public void Start()
    {
        scoreUI = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUpdate>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
    }

    public string InteractionPrompt => _prompt;

    public void Interact(EAInteractor interactor)
    {
        if (alreadyBought)
            return;

        if (scoreUI.scoreTotal < perkCost)
            return;

        scoreUI.UpdateScoreLose(perkCost);

        alreadyBought = true;

        Weapon currWeapon = inventory.GetEquipped().GetComponent<Weapon>();

        currWeapon.roundsPerMinutes *= rateOfFireMultiplier;
    }
}
