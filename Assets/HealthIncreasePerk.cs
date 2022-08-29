using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class HealthIncreasePerk : MonoBehaviour, IInteractable
{
    [Header("Prompt")]
    [SerializeField] private string _prompt;

    [Header("Cost")]
    [SerializeField] private int perkCost = 2500;

    [Header("Health")]
    [SerializeField] private float maxHealthMultiplier = 2;

    [Header("References")]
    [SerializeField] private bool alreadyBought = false;
    [SerializeField] private ScoreUpdate scoreUI;
    [SerializeField] private Character character;
    [SerializeField] private HealthController health;

    public void Start()
    {
        scoreUI = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUpdate>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        health = character.GetComponent<HealthController>();
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

        health.maxPlayerHealth *= maxHealthMultiplier;
        health.currentPlayerHealth = health.maxPlayerHealth;
    }
}
