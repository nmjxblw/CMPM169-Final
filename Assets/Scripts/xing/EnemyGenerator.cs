using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyGenerator : MonoBehaviour
{
    public LevelEnemyConfig config;
    public int NumberOfPresentEnemies = 0;
    public int currentDifficulty = 0;
    public int remainingWaves = 0;
    public Room currentRoom;
    public GameObject player;

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
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SpawnEnemiesForLevel(int level, Room room)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentRoom = room;
        int difficulty = 0;
        if (room.isEndRoom)
        {
            difficulty = -1;
        }
        else if (room.isStartRoom)
        {
            room.isBuffRoom = true;
            room.isLocked = false;
            room.RoomIsEmpty();
            
            return;
        }
        else if (room.isBeforeEndRoom)
        {
            StartCoroutine(beforEnd(room));
            return;
        }
        else
        {
            difficulty = GetWeightedRandomDifficulty(level);
        }
        currentDifficulty = difficulty;
        remainingWaves = GetWaveCountForDifficulty(difficulty);
        SpawnNextWave(room);
    }

    IEnumerator beforEnd(Room room)
    {
        yield return new WaitForSeconds(2f);
        room.isBuffRoom = true;
        room.isLocked = false;
        room.RoomIsEmpty();

    }

    public void SpawnNextWave(Room room)
    {

        if (remainingWaves <= 0){
            room.isLocked = false;
            room.RoomIsEmpty();
            return;
        }

        remainingWaves--;

        Vector3 roomPosition = room.transform.position;
        Vector3 playerPosition = player.transform.position;
        float minDistance = 80f;
        Debug.Log("Spawning enemies for level " + room.roomStep + " Difficulty is: " + currentDifficulty + ", wave " + (GetWaveCountForDifficulty(currentDifficulty) - remainingWaves));

        foreach (var levelData in config.levels)
        {
            if (levelData.levelNumber == currentDifficulty)
            {
                foreach (var enemyData in levelData.enemies)
                {
                    room.isBuffRoom = levelData.isBuffGift;
                    room.isWeaponRoom = levelData.isWeaponGift;
                    int enemyCount = Random.Range(1, enemyData.count + 1);
                    for (int i = 0; i < enemyCount; i++)
                    {
                        Vector3 spawnPosition;
                        if(currentDifficulty == -1){
                            spawnPosition = roomPosition;
                        }else{
                            spawnPosition = GenerateSpawnPosition(roomPosition, playerPosition, minDistance);
                        }
                        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPosition, Quaternion.identity);
                        enemy.transform.parent = room.enemiesContainer;
                        enemy.GetComponent<EnemyCharacter>().generateRoom = room;
                        room.enemies.Add(enemy);
                        NumberOfPresentEnemies++;
                    }
                }
                room.HandleEnemySpawnedDone();
                break;
            }
        }
    }

    private int GetWeightedRandomDifficulty(int level)
    {
        float[] weights = { 0.15f, 0.55f, 0.15f, 0.15f };
        float totalWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if (i <= level)
                totalWeight += weights[i];
        }

        float randomPoint = Random.value * totalWeight;

        for (int i = 0; i < weights.Length; i++)
        {
            if (i <= level)
            {
                if (randomPoint < weights[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= weights[i];
                }
            }
        }

        return 0;
    }

    private int GetWaveCountForDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                return 1;
            case 1:
            case 2:
                return 2;
            case 3:
                return 3; 
            default:
                return 1; 
        }
    }

    private Vector3 GenerateSpawnPosition(Vector3 roomPosition, Vector3 playerPosition, float minDistance)
    {
        Vector3 spawnPosition;
        do
        {
            spawnPosition = roomPosition + new Vector3(Random.Range(-200, 200), Random.Range(-100, 100), 0);
        } while (Vector3.Distance(spawnPosition, playerPosition) < minDistance);

        return spawnPosition;
    }

    public void decreaseNumberOfPresentEnemies()
    {
        NumberOfPresentEnemies--;
    }
}
