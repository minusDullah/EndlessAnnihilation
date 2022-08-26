using UnityEngine;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Legacy;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Movement movement;
    [SerializeField] private GameObject _player;
    [SerializeField] private GrenadeScript grenade;

    [SerializeField] private float damageBoostIncrease = .5f;
    [SerializeField] private float DamageBoostCDTimerDecrease = .5f;   
    
    [SerializeField] private float grenadeDamageIncrease = 25f;
    [SerializeField] private float grenadeRadiusIncrease = 2.5f;
    [SerializeField] private float grenadeCDTimerDecrease = .5f;

    [SerializeField] private float runningSpeedIncrease = 2.5f;
    [SerializeField] private float aimingSpeedIncrease = 1f;
    [SerializeField] private float crouchingSpeedIncrease = 1f;
    [SerializeField] private float allowedJumpsIncrease = 1f;
    [SerializeField] private float jumpForceIncrease = .5f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip uiClick;
    [SerializeField] private AudioClip uiHover;

    [SerializeField] public GameObject animatedCanvas;
    [SerializeField] public AnimationClip animationShow;
    [SerializeField] public AnimationClip animationHide;
    public Animation animationComponent;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        character = _player.GetComponent<Character>();
        movement = _player.GetComponent<Movement>();
        animationComponent = animatedCanvas.GetComponent<Animation>();
        grenade.grenadeDamage = 50f;
        grenade.radius = 5f;
        character.grenadeCDTimer = 3f;
        gameObject.SetActive(false);
    }

    #region Damage Boost

    public void DamageBoostDamageUpgrade()
    {
        character.damageBoost += damageBoostIncrease;
    }
    
    public void DamageBoostCDUpgrade()
    {
        character.damageBoostCDTimer -= DamageBoostCDTimerDecrease;
    }

    #endregion

    #region Grenade

    public void GrenadeDamageUpgrade()
    {
        grenade.grenadeDamage += grenadeDamageIncrease;
    }
    public void GrenadeRadiusUpgrade()
    {
        grenade.radius += grenadeRadiusIncrease;
    }

    public void GrenadeCDUpgrade()
    {
        character.grenadeCDTimer -= grenadeCDTimerDecrease;
    }

    #endregion

    #region Movement

    public void RunningSpeedIncrease()
    {
        movement.speedRunning += runningSpeedIncrease;
    }

    public void AimingSpeedIncrease()
    {
        movement.speedAiming += aimingSpeedIncrease;
    }

    public void CrouchingSpeedIncrease()
    {
        movement.speedCrouching += crouchingSpeedIncrease;
    }

    public void AllowedJumpsIncrease()
    {
        movement.allowedJumps += allowedJumpsIncrease;
    }

    public void JumpForceIncrease()
    {
        movement.jumpForce += jumpForceIncrease;
    }

    #endregion

    #region Audio

    public void UIClick()
    {
        audioSource.PlayOneShot(uiClick);
    }

    public void UIHover()
    {
        audioSource.PlayOneShot(uiHover);
    }

    #endregion
}
