using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPMPerk : MonoBehaviour, IInteractable
{
    [Header("Scriptable Object")]
    [SerializeField] private PerkMachineScriptable perkMachineScriptable;

    [Header("Weapon")]
    [SerializeField] private float rateOfFireMultiplier = 2;

    private GameObject weaponHolder;

    public string InteractionPrompt => perkMachineScriptable.prompt + " " + perkMachineScriptable.perkName + " " + "[Cost: " + perkMachineScriptable.perkCost + "]";

    private void Start()
    {
        weaponHolder = GameObject.FindGameObjectWithTag("Weapon");
    }

    public void Interact(ScoreUpdate scoreUI, GameObject player)
    {
        PerkHandler perkHandler = player.GetComponent<PerkHandler>();
        Inventory inventory = player.GetComponentInChildren<Inventory>();
        Weapon currWeapon;

        if (perkHandler.AlreadyBought(perkMachineScriptable.perkName))
            return;

        if (scoreUI.scoreTotal < perkMachineScriptable.perkCost)
            return;

        perkHandler.BuyPerk(perkMachineScriptable.perkName);

        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            currWeapon = inventory.transform.GetChild(i).GetComponent<Weapon>();
            currWeapon.RPMPerkActive(rateOfFireMultiplier);
        }

        for (int i = 0; i < weaponHolder.transform.childCount; i++)
        {
            currWeapon = weaponHolder.transform.GetChild(i).GetComponent<Weapon>();
            currWeapon.ReloadSpeedPerkActive(rateOfFireMultiplier);
        }
    }
}
