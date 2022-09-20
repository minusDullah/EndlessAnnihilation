using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class DoubleJumpPerk : MonoBehaviour, IInteractable
{
    [Header("Scriptable Object")]
    [SerializeField] private PerkMachineScriptable perkMachineScriptable;

    [Header("Movement")]
    [SerializeField] private int maxAllowedJumps = 1;

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

        movement.allowedJumps = maxAllowedJumps;
        movement.remainingJumps = maxAllowedJumps;
    }
}
