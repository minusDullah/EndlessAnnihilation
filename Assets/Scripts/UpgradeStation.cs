using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class UpgradeStation : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private GameObject UpgradeMenuUI;
    [SerializeField] private UpgradeMenu UpgradeMenuAnimation;
    [SerializeField] private Character character;

    public string InteractionPrompt => _prompt;

    public void Awake()
    {
        UpgradeMenuUI = GameObject.FindGameObjectWithTag("UpgradeMenu");
        UpgradeMenuAnimation = UpgradeMenuUI.GetComponent<UpgradeMenu>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    public bool Interact(EAInteractor interactor)
    {
        UpgradeMenuAnimation.animationComponent.clip = UpgradeMenuAnimation.animationShow;
        UpgradeMenuAnimation.animationComponent.Play();
        UpgradeMenuUI.SetActive(true);
        character.cursorLocked = false;
        character.UpdateCursorState();
        return true;
    }

    public bool Close(EAInteractor interactor)
    {
        UpgradeMenuAnimation.animationComponent.clip = UpgradeMenuAnimation.animationHide;
        UpgradeMenuAnimation.animationComponent.Play();
        character.cursorLocked = true;
        character.UpdateCursorState();
        Invoke("DisableMenu", .1f);
        return true;
    }
    
    private void DisableMenu()
    {
        UpgradeMenuUI.SetActive(false);
    }
}
