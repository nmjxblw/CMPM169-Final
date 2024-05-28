using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyGenerator : MonoBehaviour
{
    public LevelEnemyConfig config;
    public int NumberOfPresentEnemies = 0;
    public int currentLevel = 0;

    private static EnemyGenerator _instance;
    public static EnemyGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<EnemyGenerator>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("EnemyGenerator");
                    _instance = obj.AddComponent<EnemyGenerator>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        // Check if the 'K' key is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            DeleteOneEnemy();
        }
    }

    void DeleteOneEnemy()
    {
        // Find all game objects tagged as Enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check if there is at least one enemy
        if (enemies.Length > 0)
        {
            // Destroy the first enemy found
            Destroy(enemies[0]);
        }
    }

    public void SpawnEnemiesForLevel(int level, Room room)
    {
        Vector3 roomPosition = room.transform.position;
        Debug.Log("Spawning enemies for level " + level);
        foreach (var levelData in config.levels)
        {
            // if (levelData.levelNumber == level)
            // {
            foreach (var enemyData in levelData.enemies)
            {
                // for (int i = 0; i < enemyData.count; i++)
                // {
                //     Vector3 spawnPosition = roomPosition + GenerateSpawnPosition();
                //     Instantiate(enemyData.enemyPrefab, spawnPosition, Quaternion.identity);
                //     NumberOfPresentEnemies++;
                // }
                for (int i = 0; i < level; i++)
                {
                    Vector3 spawnPosition = roomPosition + GenerateSpawnPosition();
                    GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPosition, Quaternion.identity);
                    enemy.transform.parent = room.enemiesContainer;
                    enemy.GetComponent<EnemyCharacter>().generateRoom = room;
                    room.enemies.Add(enemy);
                    NumberOfPresentEnemies++;
                }
            }
            break;
            // }
        }
        room.HandleEnemySpawnedDone();
    }

    private Vector3 GenerateSpawnPosition()
    {
        return new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
    }

    public void decreaseNumberOfPresentEnemies()
    {
        NumberOfPresentEnemies--;
    }
}
