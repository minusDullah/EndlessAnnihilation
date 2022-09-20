using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class HealthIncreasePerk : MonoBehaviour, IInteractable
{
    [Header("Scriptable Object")]
    [SerializeField] private PerkMachineScriptable perkMachineScriptable;

    [Header("Health")]
    [SerializeField] private float maxHealthMultiplier = 2;

    public string InteractionPrompt => perkMachineScriptable.prompt + " " + perkMachineScriptable.perkName + " " + "[Cost: " + perkMachineScriptable.perkCost + "]";

    public void Interact(ScoreUpdate scoreUI, GameObject player)
    {
        PerkHandler perkHandler = player.GetComponent<PerkHandler>();
        HealthController health = player.GetComponent<HealthController>();

        if (perkHandler.AlreadyBought(perkMachineScriptable.perkName))
            return;

        if (scoreUI.scoreTotal < perkMachineScriptable.perkCost)
            return;

        perkHandler.BuyPerk(perkMachineScriptable.perkName);

        health.maxPlayerHealth *= maxHealthMultiplier;
        health.currentPlayerHealth = health.maxPlayerHealth;
    }
}
