using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class Room : MonoBehaviour
{
    public GameObject roomText;
    public GameObject doorUp, doorDown, doorLeft, doorRight;
    public GameObject wallUp, wallDown, wallLeft, wallRight;
    public GameObject vortexUp, vortexDown, vortexLeft, vortexRight;
    public bool roomUp, roomDown, roomLeft, roomRight;

    public int roomStep;

    public bool isStartRoom, isEndRoom, isBeforeEndRoom;
    public GameObject playerPrefab;
    public GameObject player;
    public CinemachineVirtualCamera virtualCamera;
    public bool isVisited;
    public bool isLocked;
    public int enemyCount;

    public EnemyGenerator enemyGenerator;
    public List<GameObject> enemies;

    void Awake()
    {
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyGenerator = GameObject.Find("EventSystem").GetComponent<EnemyGenerator>();
        doorUp.SetActive(roomUp);
        doorDown.SetActive(roomDown);
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
        wallUp.SetActive(isLocked);
        wallDown.SetActive(isLocked);
        wallLeft.SetActive(isLocked);
        wallRight.SetActive(isLocked);
        UpdateRoomText();

        if (isStartRoom)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        }

        enemyGenerator = GameObject.Find("EventSystem").GetComponent<EnemyGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        isLocked = enemyCount > 0;

        if (isLocked)
        {
            vortexUp.SetActive(false);
            vortexDown.SetActive(false);
            vortexLeft.SetActive(false);
            vortexRight.SetActive(false);
        }
        else
        {
            vortexUp.SetActive(roomUp);
            vortexDown.SetActive(roomDown);
            vortexLeft.SetActive(roomLeft);
            vortexRight.SetActive(roomRight);
        }
    }

    private void UpdateRoomText()
    {
        roomText.GetComponent<TextMeshProUGUI>().text = roomStep.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            virtualCamera.LookAt = this.transform;
            virtualCamera.Follow = this.transform;
            if (!isVisited)
            {
                isVisited = true;
                enemyGenerator.SpawnEnemiesForLevel(roomStep, this);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            enemyCount++;
        }
    }

    public void killThisRoomEnemy(GameObject enemy)
    {
        enemyCount--;
        enemies.Remove(enemy);
    }
}
