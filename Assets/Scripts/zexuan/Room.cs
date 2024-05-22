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

    public bool roomUp, roomDown, roomLeft, roomRight;

    public int roomStep;

    public bool isStartRoom, isEndRoom, isBeforeEndRoom;
    public GameObject playerPrefab;
    public GameObject player;
    public CinemachineVirtualCamera virtualCamera;
    public bool isVisited;

    public EnemyGenerator enemyGenerator;

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
        wallUp.SetActive(!roomUp);
        wallDown.SetActive(!roomDown);
        wallLeft.SetActive(!roomLeft);
        wallRight.SetActive(!roomRight);
        UpdateRoomText();

        if (isStartRoom)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
                enemyGenerator.SpawnEnemiesForLevel(roomStep, transform.position);
            }
        }
    }
}
