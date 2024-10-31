using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaveSpawner : MonoBehaviour
{
    [Header("Level Components")]
    LevelManager levelManager;

    [Header("Waves")]
    Wave currentWave;
    [HideInInspector] public int currentWaveInOrder {get; private set;} = 1;
    bool waveSpawnEnded;

    [Header("Enemies")]
    [HideInInspector] int enemiesAlive;
    public List<WaveEnemyStateMachine> enemies;

    [Header("Spawn")]
    [HideInInspector] public Grid3D grid;

    [Header("Canva")]
    [SerializeField] CanvaManager canvaManager;

    [Header("Debug")]
    [SerializeField] InputAction spawnEnemy;
    public GameObject enemyToSpawn;
    public Transform spawnLocation;

    void Awake()
    {
        GetComponents();

        spawnEnemy.Enable();
        spawnEnemy.performed += context => SpawnEnemy();
    }

    void GetComponents()
    {
        if(grid == null)
        {
            grid = GameObject.Find("PathfindingManager").GetComponent<Grid3D>();
        }
        if(canvaManager == null)
        {
            canvaManager = GetComponent<CanvaManager>();
        }
        if(levelManager == null)
        {
            levelManager = GetComponent<LevelManager>();
        }
    }

    public void StartWave(Wave wave)
    {
        waveSpawnEnded = false;

        currentWave = wave;
        currentWave.currentSubWaveIndex = 0;

        StartCoroutine(SpawnEnemies());
    }

    void SpawnEnemy()
    {
        GameObject enemySpawned = Instantiate(enemyToSpawn, 
                    spawnLocation.position, 
                    Quaternion.identity);
    }


    public IEnumerator SpawnEnemies()
    {
        waveSpawnEnded = false;

        if(currentWave.subwaves == null || currentWave.subwaves.Count == 0)
        {
            Debug.Log("The wave " + currentWave + " doesnt have subwaves");
        }
        else
        {
            for(int i = 0; i < currentWave.subwaves.Count; i++)
            {
                int subwaveEnemiesAmount = currentWave.subwaves[currentWave.currentSubWaveIndex].enemies.Count;

                for(int o = 0; o < subwaveEnemiesAmount; o++)
                {
                    yield return new WaitForSeconds(currentWave.subwaves[currentWave.currentSubWaveIndex].timeToNextEnemy);
                    
                    if(currentWave.useSpecificSpawnArea)
                    {
                        GameObject enemySpawned = Instantiate(currentWave.subwaves[currentWave.currentSubWaveIndex].enemies[o], 
                        GetSpawnPosition(currentWave.spawnPosition, currentWave.spawnRange), 
                        Quaternion.identity, 
                        GameObject.Find("InstantiatedObjects").transform);
                    }
                    else
                    {
                        Debug.Log("Fazer spawns de inimigos predeterminados");
                    }
                    
                    // enemySpawned.name = "W" + i + " (" + o + ") " + currentWave.subwaves[currentWave.currentSubWaveIndex].enemies[o].name;
                }

                currentWave.currentSubWaveIndex++;

                if(i != (currentWave.subwaves.Count - 1))
                {
                    yield return new WaitForSeconds(currentWave.timeBetweenSubwaves);
                }
            }

            waveSpawnEnded = true;
        }
    }

    public void EnemySpawned(WaveEnemyStateMachine enemyStateMachine)
    {
        enemies.Add(enemyStateMachine);
        enemiesAlive++;
    }

    Vector3 GetSpawnPosition(Vector3 spawnPosition, float spawnRange)
    {
        Vector3 finalSpawnPosition = spawnPosition;

        finalSpawnPosition.x += Random.Range(-spawnRange, spawnRange);
        finalSpawnPosition.z += Random.Range(spawnRange, -spawnRange);
        
        if(grid.ValidatePosition(finalSpawnPosition))
        {
            return finalSpawnPosition;
        }
        else
        {
            return GetSpawnPosition(spawnPosition, spawnRange);
        }
    }

    public void EnemyDied(WaveEnemyStateMachine enemyStateMachine)
    {
        enemies.Remove(enemyStateMachine);
        enemiesAlive--;
        
        CheckEnemiesLeft();
    }

    public void CheckEnemiesLeft()
    {
        if(enemiesAlive <= 0 && waveSpawnEnded)
        {
            WaveClear();
        }
    }

    void WaveClear()
    {   
        currentWaveInOrder++;
        levelManager.LevelEventEnd(currentWave);
    }
}
