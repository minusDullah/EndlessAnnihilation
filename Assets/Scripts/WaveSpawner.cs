using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    //https://www.youtube.com/watch?v=7T-MTo8Uaio

    [Header("Enemy Stats")]
    [SerializeField] public List<Enemy> enemies = new List<Enemy>();
    [SerializeField] public int currWave;
    [SerializeField] private int waveValue;
    [SerializeField] private int waveMultiplier = 10;
    [SerializeField] public int totalKills;
    [SerializeField] public int buffKillCounter = 50;
    [SerializeField] public float timeRemaining = 60;
    [SerializeField] public float timeTotal;
    [SerializeField] public List<GameObject> enemiesToSpawn = new List<GameObject>();

    [Header("Wave Stats")]
    [SerializeField] public List<Transform> spawnLocation = new List<Transform>();
    [SerializeField] public int waveDuration;
    [SerializeField] public float waveTimer;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnTimer;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Zombie References")]
    [SerializeField] public GameObject zombieHolder;
    [SerializeField] public GameObject[] powerUps;
    [SerializeField] public bool enemiesFrozen;
    [SerializeField] public bool enemiesGravity;

    

    // Start is called before the first frame update
    void Start()
    {
        zombieHolder = GameObject.FindGameObjectWithTag("ZombieHolder");
        timerText = GameObject.FindGameObjectWithTag("CountdownTimer").GetComponent<TextMeshProUGUI>();
        GenerateWave();
        SetTimer(timeRemaining);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeRemaining -= Time.deltaTime;
        timeTotal += Time.deltaTime;
        SetTimer(timeRemaining);

        if (!enemiesFrozen || !enemiesGravity)
        {
            if (spawnTimer <= 0)
            {
                //spawn an enemy
                if (enemiesToSpawn.Count > 0)
                {
                    int spawnPos = Random.Range(0, spawnLocation.Count);
                    var enemy = Instantiate(enemiesToSpawn[0], spawnLocation[spawnPos].position, Quaternion.identity); // spawn first enemy in our list
                    enemy.transform.parent = zombieHolder.transform;
                    enemiesToSpawn.RemoveAt(0); // and remove it
                    spawnTimer = spawnInterval;
                }
                else
                {
                    waveTimer = 0; // if no enemies remain, end wave
                    currWave++;
                    GenerateWave();
                }
            }
            else
            {
                spawnTimer -= Time.fixedDeltaTime;
                waveTimer -= Time.fixedDeltaTime;
            }
        }
    }

    private void SetTimer(float currentTime)
    {
        if(currentTime <= 0)
        {
            string sceneToLoad = SceneManager.GetActiveScene().path;

            #if UNITY_EDITOR
            //Load the scene.
            UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
            #else
            //Load the scene.
            SceneManager.LoadSceneAsync(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
            #endif
        }

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void GenerateWave()
    {
        waveValue = currWave * waveMultiplier;
        waveValue = Mathf.Clamp(waveValue, 0, waveDuration);
        GenerateEnemies();

        //spawnInterval = Random.Range(.5f, 1.5f);
        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only
    }

    public void GenerateEnemies()
    {
        // Create a temporary list of enemies to generate
        // 
        // in a loop grab a random enemy 
        // see if we can afford it
        // if we can, add it to our list, and deduct the cost.

        // repeat... 

        //  -> if we have no points left, leave the loop

        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}
