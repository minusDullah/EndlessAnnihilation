using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class DoubleJumpPerk : MonoBehaviour, IInteractable
{
    [Header("Prompt")]
    [SerializeField] private string _prompt;

    [Header("Cost")]
    [SerializeField] private int perkCost = 2500;

    [Header("Movement")]
    [SerializeField] private int maxAllowedJumps = 1;

    [Header("References")]
    [SerializeField] private bool alreadyBought = false;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private Movement movement;

    public string InteractionPrompt => _prompt;

    public void Interact(EAInteractor interactor)
    {
        if (alreadyBought)
            return;

        if (scoreUI.scoreTotal < perkCost)
            return;

        scoreUI.UpdateScoreLose(perkCost);

        alreadyBought = true;

        movement.allowedJumps = maxAllowedJumps;
        movement.remainingJumps = maxAllowedJumps;
    }
}
