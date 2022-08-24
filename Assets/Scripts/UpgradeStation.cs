using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class UpgradeStation : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private GameObject UpgradeMenuUI;
    [SerializeField] private UpgradeMenu UpgradeMenuAnimation;
    [SerializeField] private Character character;

    public string InteractionPrompt => _prompt;

    public void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        UpgradeMenuUI = GameObject.FindGameObjectWithTag("UpgradeMenu");
        UpgradeMenuAnimation = UpgradeMenuUI.GetComponent<UpgradeMenu>();
    }

    public void Interact(EAInteractor interactor)
    {
        if (UpgradeMenuUI.activeInHierarchy)
        {
            UpgradeMenuAnimation.animationComponent.clip = UpgradeMenuAnimation.animationHide;
            UpgradeMenuAnimation.animationComponent.Play();
            character.cursorLocked = true;
            character.UpdateCursorState();
            Invoke("DisableMenu", .1f);
        }
        else
        {
            UpgradeMenuAnimation.animationComponent.clip = UpgradeMenuAnimation.animationShow;
            UpgradeMenuAnimation.animationComponent.Play();
            character.cursorLocked = false;
            character.UpdateCursorState();
            UpgradeMenuUI.SetActive(true);
        }
    }
    
    private void DisableMenu()
    {
        UpgradeMenuUI.SetActive(false);
    }
}
