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
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject interactUI;
    [SerializeField] public TextMeshProUGUI interactionText;
    [SerializeField] private Animator animator;
    [SerializeField] private string stateName = "Visible";

    private readonly Collider[] _colliders = new Collider[3];
    

    [SerializeField] private int _numFound;

    public void Start()
    {
        character = gameObject.GetComponent<Character>();
        upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");
        interactUI = GameObject.FindGameObjectWithTag("InteractUI");
        interactionText = interactUI.GetComponentInChildren<TextInteraction>().textToModify;
        animator = interactUI.GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();

            animator.SetBool(stateName, true);

            if (interactionText.text == "")
            {
                interactionText.text = interactable.InteractionPrompt;
            }

            if (interactable != null && Keyboard.current.fKey.wasPressedThisFrame)
            {
                interactable.Interact(this);
            }
        }
        else
        {
            if (interactionText.text != "")
            {
                interactionText.text = "";
                animator.SetBool(stateName, false);
            }

            if (upgradeMenu.activeInHierarchy)
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
