using UnityEngine;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Legacy;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Movement movement;
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

    void Start()
    {
        gameObject.SetActive(false);
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        grenade.grenadeDamage = 50f;
        grenade.radius = 5f;
        character.grenadeCDTimer = 3f;
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
}
