using UnityEngine;
using UnityEngine.InputSystem;
using InfimaGames.LowPolyShooterPack;

public class EAInteractor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private Character character;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject UpgradeMenu;

    private readonly Collider[] _colliders = new Collider[3];

    [SerializeField] private int _numFound;

    public void Start()
    {
        SettingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu");
        UpgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");
        character = gameObject.GetComponent<Character>();
    }

    public void FixedUpdate()
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
            if (!SettingsMenu.activeSelf)
            {
                SettingsMenu.SetActive(true);
            }
            
            if (UpgradeMenu.activeSelf)
            {
                character.cursorLocked = true;
                character.UpdateCursorState();
                UpgradeMenu.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
