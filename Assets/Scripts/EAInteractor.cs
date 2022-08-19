using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EAInteractor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius;
    [SerializeField] private LayerMask _interactableMask;

    [SerializeField] GameObject SettingsMenu;

    [SerializeField] public bool canPause = true;
    private readonly Collider[] _colliders = new Collider[3];

    [SerializeField] private int _numFound;

    private void Start()
    {
        SettingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu");
    }

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            SettingsMenu.SetActive(false);

            var interactable = _colliders[0].GetComponent<IInteractable>();

            if (interactable != null && Keyboard.current.fKey.wasPressedThisFrame)
            {
                interactable.Interact(this);
            }

            if(interactable != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                interactable.Close(this);
            }
        }
        else
        {
            SettingsMenu.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
