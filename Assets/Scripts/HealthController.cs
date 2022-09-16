using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Header("Player Health Amount")]
    public float currentPlayerHealth = 100f;
    [SerializeField] public float maxPlayerHealth = 100f;
    [SerializeField] private int regenRate = 1;
    [SerializeField] public bool canRegen = false;

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

    private GameObject playerMesh;
    private GameObject zombieHolder;
    private WaveSpawner waveSpawner;
    private PlayerInput playerInput;
    private CharacterController cc;
    private Canvas canvasUI;
    private Canvas timerUI;
    private Inventory inventory;
    private Camera mainCamera;
    private bool dead;
    public List<Target> enemies;

    private void Start()
    {
        hurtImage = GameObject.Find("RadialBloodHurt").GetComponent<Image>();
        redSplatterImage = GameObject.Find("RedSplatter").GetComponent<Image>();

        playerMesh = GameObject.FindGameObjectWithTag("PlayerMesh");
        waveSpawner = GameObject.FindGameObjectWithTag("waveSpawner").GetComponent<WaveSpawner>();
        zombieHolder = GameObject.FindGameObjectWithTag("ZombieHolder");
        canvasUI = GameObject.FindGameObjectWithTag("DamageBoostSlider").GetComponentInParent<Canvas>();
        timerUI = GameObject.FindGameObjectWithTag("CountdownTimer").GetComponentInParent<Canvas>();

        cc = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        inventory = GetComponentInChildren<Inventory>();
        mainCamera = GetComponentInChildren<Camera>();

        hurtImage.enabled = false;

        UpdateHealth();
        StartCoroutine(playBGMusic());
    }

    public void UpdateHealth()
    {
        Color splatterAlpha = redSplatterImage.color;
        splatterAlpha.a = 1 - (currentPlayerHealth / maxPlayerHealth);
        redSplatterImage.color = splatterAlpha;
        PlayerHeartbeatAudio();
    }

    public void TakeDamage(float damage)
    {
        if (currentPlayerHealth > 0 && canTakeDamage == true)
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
            //Make character smaller and closer to the ground to make it look like the character fell
            cc.center = cc.center / 2f;
            cc.height = .5f;
            cc.minMoveDistance = 1;

            //Kill all zombies
            Destroy(zombieHolder, 60f);
            for (int i = 0; i < zombieHolder.transform.childCount; i++)
            {
                if (zombieHolder.transform.GetChild(i).GetComponent<Target>().health > 0)
                {
                    zombieHolder.transform.GetChild(i).GetComponent<CapsuleCollider>().isTrigger = false;
                    zombieHolder.transform.GetChild(i).GetComponent<EnemyMovement>().triggerEntered = false;
                }
            }

            //Remove player hands
            playerMesh.SetActive(false);
            //Stop wave spawner
            waveSpawner.enabled = false;

            //Disable Input
            playerInput.actions.Disable();

            //Remove weapons
            inventory.gameObject.SetActive(false);
            inventory.weapons.Clear();

            //Get all UI and disable it
            canvasUI.enabled = false;
            timerUI.enabled = false;

            dead = true;
            /*
            string sceneToLoad = SceneManager.GetActiveScene().path;
            #if UNITY_EDITOR
            //Load the scene.
            UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
            #else
            //Load the scene.
            SceneManager.LoadSceneAsync(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
            #endif
            */
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

    private void moveCameraUp()
    {
        mainCamera.transform.LookAt(playerMesh.transform);
        mainCamera.transform.Translate(.1f * Vector3.up / 2, Space.World);
    }

    private void Update()
    {
        if(currentPlayerHealth <= 0 && dead == true)
        {
            moveCameraUp();
        }
       
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
