using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Header("Player Health Amount")]
    public float currentPlayerHealth = 100f;
    [SerializeField] private float maxPlayerHealth = 100f;
    [SerializeField] private int regenRate = 1;
    private bool canRegen = false;

    [Header("Add the splatter image here")]
    [SerializeField] private Image redSplatterImage = null;

    [Header("Hurt Image Flash")]
    [SerializeField] private Image hurtImage = null;
    [SerializeField] public float hurtTimer = 0.1f;

    [Header("Heal timer")]
    [SerializeField] private float healCooldown = 3f;
    [SerializeField] private float maxHealCooldown = 3f;
    [SerializeField] private bool startCooldown = false;

    [Header("Audio name")]
    [SerializeField] private AudioSource healthAudioSource;

    private void Start()
    {
        hurtImage = GameObject.Find("RadialBloodHurt").GetComponent<Image>();
        redSplatterImage = GameObject.Find("RedSplatter").GetComponent<Image>();
        hurtImage.enabled = false;
        UpdateHealth();
    }

    void UpdateHealth()
    {
        Color splatterAlpha = redSplatterImage.color;
        splatterAlpha.a = 1 - (currentPlayerHealth / maxPlayerHealth);
        redSplatterImage.color = splatterAlpha;
    }

    public void TakeDamage()
    {
        if(currentPlayerHealth >= 0)
        {
            canRegen = false;
            StartCoroutine(HurtFlash());
            UpdateHealth();
            healCooldown = maxHealCooldown;
            startCooldown = true;
        }
        else
        {
            //kill player
            currentPlayerHealth = 0;
        }
    }

    IEnumerator HurtFlash()
    {
        hurtImage.enabled = true;
        healthAudioSource.Play();
        yield return new WaitForSeconds(hurtTimer);
        hurtImage.enabled = false;
    }

    private void Update()
    {
        if(startCooldown)
        {
            healCooldown -= Time.deltaTime;
            if(healCooldown <= 0)
            {
                canRegen = true;
                startCooldown = false;
            }
        }

        if (canRegen)
        {
            if(currentPlayerHealth <= maxPlayerHealth - 0.01)
            {
                currentPlayerHealth += Time.deltaTime * regenRate;
                UpdateHealth();
            }
            else
            {
                currentPlayerHealth = maxPlayerHealth;
                healCooldown = maxHealCooldown;
                canRegen = false;
            }
        }
    }
}
