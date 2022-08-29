using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class MovementPerk : MonoBehaviour, IInteractable
{
    [Header("Prompt")]
    [SerializeField] private string _prompt;

    [Header("Cost")]
    [SerializeField] private int perkCost = 2500;

    [Header("Movement")]
    [SerializeField] private float movementSpeedMultiplier = 2;
    [SerializeField] private float jumpForceMultiplier = 2;

    [Header("References")]
    [SerializeField] private bool alreadyBought = false;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private Movement movement;

    public void Start()
    {
        scoreUI = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUpdate>();
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
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

        movement.speedRunning *= movementSpeedMultiplier;
        movement.speedAiming *= movementSpeedMultiplier;
        movement.speedCrouching *= movementSpeedMultiplier;
        movement.speedWalking *= movementSpeedMultiplier;
        movement.jumpForce *= jumpForceMultiplier;
    }
}
