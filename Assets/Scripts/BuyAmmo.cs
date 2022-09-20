using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAmmo : MonoBehaviour, IInteractable
{
    [Header("Scriptable Object")]
    [SerializeField] private InteractableBoxScriptable interactableBoxScriptable;


    [SerializeField] private bool cooldownOff = true;

    public string InteractionPrompt => interactableBoxScriptable.prompt + " " + "[Cost: " + interactableBoxScriptable.cost + "]";

    public void Interact(ScoreUpdate scoreUI, GameObject player)
    {
        Inventory inventory = player.GetComponentInChildren<Inventory>();
        Character character = player.GetComponent<Character>();
        Weapon currWeapon;

        if (character.IsInspecting() || character.IsInvoking())
            return;

        if (!cooldownOff)
            return;

        if (scoreUI.scoreTotal < interactableBoxScriptable.cost)
            return;

        scoreUI.UpdateScoreLose(interactableBoxScriptable.cost);

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
