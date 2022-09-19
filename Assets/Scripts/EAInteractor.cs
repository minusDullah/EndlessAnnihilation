using UnityEngine;
using UnityEngine.InputSystem;
using InfimaGames.LowPolyShooterPack;
using TMPro;
using InfimaGames.LowPolyShooterPack.Interface;
using System.Collections;

public class EAInteractor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius;
    [SerializeField] private LayerMask _interactableMask;

    [SerializeField] private Character character;
    [SerializeField] private GameObject lastInteractableObject;
    [SerializeField] private GameObject interactUI;
    [SerializeField] public TextMeshProUGUI interactionText;
    [SerializeField] private Animator animator;
    [SerializeField] private string stateName = "Visible";
    [SerializeField] private bool interactKeyPressed;

    private readonly Collider[] _colliders = new Collider[3];
    
    [SerializeField] private int _numFound;

    public void FixedUpdate()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();

            lastInteractableObject = _colliders[0].GetComponent<GameObject>();

            animator.SetBool(stateName, true);

            if (interactionText.text == "" || interactionText.ToString() != interactable.InteractionPrompt)
            {
                interactionText.text = interactable.InteractionPrompt;
            }

            if (interactable != null && interactKeyPressed && !character.reloading)
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

            if (lastInteractableObject != null && lastInteractableObject.activeInHierarchy)
            {
                character.cursorLocked = true;
                character.UpdateCursorState();
                lastInteractableObject.SetActive(false);
            }
        }
    }

    public void InteractKeyPressed(InputAction.CallbackContext context)
    {
        interactKeyPressed = true;
        StartCoroutine(KeyCooldown());
    }

    IEnumerator KeyCooldown()
    {
        yield return new WaitForSeconds(.1f);
        interactKeyPressed = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
