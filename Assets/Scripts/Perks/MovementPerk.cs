using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class MovementPerk : MonoBehaviour, IInteractable
{
    [Header("Scriptable Object")]
    [SerializeField] private PerkMachineScriptable perkMachineScriptable;

    [Header("Movement")]
    [SerializeField] private float movementSpeedMultiplier = 2;
    [SerializeField] private float jumpForceMultiplier = 2;

    public string InteractionPrompt => perkMachineScriptable.prompt + " " + perkMachineScriptable.perkName + " " + "[Cost: " + perkMachineScriptable.perkCost + "]";

    public void Interact(ScoreUpdate scoreUI, GameObject player)
    {
        PerkHandler perkHandler = player.GetComponent<PerkHandler>();
        Movement movement = player.GetComponent<Movement>();

        if (perkHandler.AlreadyBought(perkMachineScriptable.perkName))
            return;

        if (scoreUI.scoreTotal < perkMachineScriptable.perkCost)
            return;

        perkHandler.BuyPerk(perkMachineScriptable.perkName); 

        movement.speedRunning *= movementSpeedMultiplier;
        movement.speedAiming *= movementSpeedMultiplier;
        movement.speedCrouching *= movementSpeedMultiplier;
        movement.speedWalking *= movementSpeedMultiplier;
        movement.jumpForce *= jumpForceMultiplier;
    }
}
