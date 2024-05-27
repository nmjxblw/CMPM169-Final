using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public LevelEnemyConfig config;
    public int NumberOfPresentEnemies = 0;
    public int currentLevel = 0;

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
                    for (int i = 0; i< level; i++){
                        Vector3 spawnPosition = roomPosition + GenerateSpawnPosition();
                        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPosition, Quaternion.identity);
                        enemy.GetComponent<Character>().generateRoom = room;
                        room.enemies.Add(enemy);
                        NumberOfPresentEnemies++;
                    }
                }
                break;
            // }
        }
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
