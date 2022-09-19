using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAmmo : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Character character;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private int ammoCost = 1000;
    [SerializeField] private bool cooldownOff = true;
    [SerializeField] private Weapon currWeapon;

    public string InteractionPrompt => _prompt;

    public void Interact(EAInteractor interactor)
    {
        if (character.IsInspecting() || character.IsInvoking())
            return;

        if (!cooldownOff)
            return;

        if (scoreUI.scoreTotal < ammoCost)
            return;

        scoreUI.UpdateScoreLose(ammoCost);

        cooldownOff = false;

        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            currWeapon = inventory.transform.GetChild(i).GetComponent<Weapon>();
            currWeapon.AddAmmunitionInventoryAmount(currWeapon.ammunitionMax);
        }

        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(2f);
        cooldownOff = true;
    }
}
