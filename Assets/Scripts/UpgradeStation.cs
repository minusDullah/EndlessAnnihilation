using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private GameObject UpgradeMenuUI;

    public string InteractionPrompt => _prompt;

    private void Start()
    {
        UpgradeMenuUI = GameObject.FindGameObjectWithTag("UpgradeMenu");
    }

    public bool Interact(EAInteractor interactor)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UpgradeMenuUI.SetActive(true);
        return true;
    }

    public bool Close(EAInteractor interactor)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UpgradeMenuUI.SetActive(false);
        return true;
    }
}
