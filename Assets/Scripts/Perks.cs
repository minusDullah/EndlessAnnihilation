using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class Perks : MonoBehaviour, IInteractable
{
    [Header("Prompt")]
    [SerializeField] private string _prompt;

    [Header("Cost")]
    [SerializeField] private int perkCost = 2500;

    [Header("Weapon")]
    [SerializeField] private float weaponDamageMultiplier = 2;
    [SerializeField] private float rateOfFireMultiplier = 2;
    [SerializeField] private float reloadSpeedMultiplier = 2;


    [Header("Movement")]
    [SerializeField] private float movementSpeedMultiplier = 2;
    [SerializeField] private float jumpForceMultiplier = 2;
    [SerializeField] private int maxAllowedJumps = 2;
    
    [Header("Health")]
    [SerializeField] private float maxHealthMultiplier = 2;

    [Header("References")]
    [SerializeField] private bool alreadyBought = false;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Character character;
    [SerializeField] private Movement movement;
    [SerializeField] private HealthController health;

    public void Start()
    {
        scoreUI = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUpdate>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
        character = inventory.GetComponentInParent<Character>();
        movement = inventory.GetComponentInParent<Movement>();
        health = inventory.GetComponentInParent<HealthController>();
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
        character.characterAnimator.speed *= reloadSpeedMultiplier;
        currWeapon.reloadSpeed *= reloadSpeedMultiplier;

        currWeapon.roundsPerMinutes *= rateOfFireMultiplier;

        movement.speedRunning *= movementSpeedMultiplier;
        movement.speedAiming *= movementSpeedMultiplier;
        movement.speedCrouching *= movementSpeedMultiplier;
        movement.speedWalking *= movementSpeedMultiplier;

        movement.allowedJumps = maxAllowedJumps;
        movement.jumpForce *= jumpForceMultiplier;

        health.maxPlayerHealth *= maxHealthMultiplier;
        health.currentPlayerHealth = health.maxPlayerHealth;
    }
}
