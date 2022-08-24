using UnityEngine;
using UnityEngine.InputSystem;
using InfimaGames.LowPolyShooterPack;
using System;
using TMPro;
using InfimaGames.LowPolyShooterPack.Interface;

public class EAInteractor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private Character character;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject interactUI;
    [SerializeField] private GameObject attachmentUI;
    [SerializeField] public TextMeshProUGUI interactionText;
    [SerializeField] private Animator animator;
    [SerializeField] private string stateName = "Visible";

    private readonly Collider[] _colliders = new Collider[3];

    [SerializeField] private int _numFound;

    public void Start()
    {
        settingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu");
        upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");
        interactUI = GameObject.FindGameObjectWithTag("InteractUI");
        attachmentUI = GameObject.FindGameObjectWithTag("AttachmentMenu");
        interactionText = interactUI.GetComponentInChildren<TextInteraction>().textToModify;
        animator = interactUI.GetComponentInChildren<Animator>();
        character = gameObject.GetComponent<Character>();
    }

    public void LateUpdate()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();

            animator.SetBool(stateName, true);

            if (interactionText.text == "" && !upgradeMenu.activeSelf)
            {
                interactionText.text = interactable.InteractionPrompt;
            }

            if (interactable != null && Keyboard.current.fKey.wasPressedThisFrame && !upgradeMenu.activeSelf)
            {
                interactable.Interact(this);
                settingsMenu.SetActive(false);
            }

            if(interactable != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                interactable.Close(this);
                settingsMenu.SetActive(true);
            }

            if(character.characterAnimator.speed != 1f) { character.UnPause(); }
            
        }
        else
        {
            if (interactionText.text != "")
            {
                interactionText.text = "";
                animator.SetBool(stateName, false);
            }

            if (upgradeMenu.activeSelf)
            {
                character.cursorLocked = true;
                character.UpdateCursorState();
                upgradeMenu.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
