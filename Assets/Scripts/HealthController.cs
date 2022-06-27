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
    [SerializeField] private bool canRegen = false;

    [Header("Player Invulnerable when hit")]
    [SerializeField] private float invulnerableTimer = .5f;
    [SerializeField] private bool canTakeDamage = true;

    [Header("Add the splatter image here")]
    [SerializeField] private Image redSplatterImage = null;

    [Header("Hurt Image Flash")]
    [SerializeField] private Image hurtImage = null;
    [SerializeField] public float hurtTimer = 0.1f;

    [Header("Heal timer")]
    [SerializeField] private float healCooldown = 3f;
    [SerializeField] private float maxHealCooldown = 3f;
    [SerializeField] private bool startCooldown = false;

    [Header("Audio")]
    [SerializeField] private AudioSource playerHitSource;
    [SerializeField] private AudioSource playerHealthSource;
    [SerializeField] private AudioSource BGMusic;
    [SerializeField] private AudioClip[] playerHit;
    [SerializeField] private AudioClip[] playerHeartbeat;
    [SerializeField] private AudioClip[] BGMusicTracks;

    private void Start()
    {
        hurtImage = GameObject.Find("RadialBloodHurt").GetComponent<Image>();
        redSplatterImage = GameObject.Find("RedSplatter").GetComponent<Image>();
        hurtImage.enabled = false;
        UpdateHealth();
        StartCoroutine(playBGMusic());
    }

    void UpdateHealth()
    {
        Color splatterAlpha = redSplatterImage.color;
        splatterAlpha.a = 1 - (currentPlayerHealth / maxPlayerHealth);
        redSplatterImage.color = splatterAlpha;
        PlayerHeartbeatAudio();
    }

    public void TakeDamage(float damage)
    {
        if (currentPlayerHealth >= 0 && canTakeDamage == true)
        {
            canTakeDamage = false;
            currentPlayerHealth -= damage;
            canRegen = false;
            StartCoroutine(HurtFlash());
            UpdateHealth();
            healCooldown = maxHealCooldown;
            startCooldown = true;
            StartCoroutine(Invulnerable());
        }
        else if(currentPlayerHealth <= 0)
        {

            //kill player
            //currentPlayerHealth = 0;
        }
    }

    private void PlayerHitAudio()
    {
        int clipToPlay = Random.Range(0, playerHit.Length);
        playerHitSource.clip = playerHit[clipToPlay];
        playerHitSource.Play();
    }

    private void PlayerHeartbeatAudio()
    {
        playerHealthSource.volume = Mathf.Abs(1 - (currentPlayerHealth / 100));
        playerHealthSource.volume = Mathf.Clamp(playerHealthSource.volume, 0, .8f);
        if (!playerHealthSource.isPlaying && currentPlayerHealth != maxPlayerHealth)
        {
            playerHealthSource.Play();
        }
    }

    private void ChangeHeartbeatAudio()
    {
        if (currentPlayerHealth > (maxPlayerHealth / 2) && currentPlayerHealth < maxPlayerHealth)
        {
            playerHealthSource.clip = playerHeartbeat[1];
        }
        else if (currentPlayerHealth <= maxPlayerHealth / 2)
        {
            playerHealthSource.clip = playerHeartbeat[0];
        }
    }

    IEnumerator HurtFlash()
    {
        hurtImage.enabled = true;
        PlayerHitAudio();
        yield return new WaitForSeconds(hurtTimer);
        hurtImage.enabled = false;
    }

    IEnumerator Invulnerable()
    {
        yield return new WaitForSeconds(invulnerableTimer);
        canTakeDamage = true;
    }

    IEnumerator playBGMusic()
    {
        int randomSong = Random.Range(0, BGMusicTracks.Length);
        BGMusic.clip = BGMusicTracks[randomSong];
        BGMusic.Play();
        yield return new WaitForSeconds(BGMusic.clip.length);
        StartCoroutine(playBGMusic());
    }

    private void Update()
    {
        UpdateHealth();
        ChangeHeartbeatAudio();

        if (startCooldown)
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
