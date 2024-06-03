using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Events;

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
    public bool isLocked = true;
    public bool isBuffRoom;
    public bool isWeaponRoom;
    [SerializeField]
    private int _enemyCount;
    public int enemyCount
    {
        get
        {
            return _enemyCount;
        }
        private set
        {
            _enemyCount = value;
            if (_enemyCount <= 0)
            {
                enemyGenerator.SpawnNextWave(this);
            }
        }
    }

    public EnemyGenerator enemyGenerator;
    public List<GameObject> enemies;

    public Transform enemiesContainer;

    public UnityEvent onPlayerSpawnEvent;

    private void OnEnable()
    {
        enemiesContainer = enemiesContainer == null ? transform.Find("EnemiesContainer") : enemiesContainer;
    }

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
        vortexUp.SetActive(false);
        vortexDown.SetActive(false);
        vortexLeft.SetActive(false);
        vortexRight.SetActive(false);
        UpdateRoomText();

        if (isStartRoom)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            GameManager.Instance.onPlayerSpawnEvent?.Invoke(player);
        }

        enemyGenerator = GameObject.Find("EventSystem").GetComponent<EnemyGenerator>();
        player = GameObject.FindWithTag("Player");
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
    }

    public void killThisRoomEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        enemyCount = enemies.Count;
    }

    public void HandleEnemySpawnedDone()
    {
        isLocked = true;
        enemyCount = enemies.Count;
    }

    public void RoomIsEmpty()
    {
        vortexUp.SetActive(roomUp);
        vortexDown.SetActive(roomDown);
        vortexLeft.SetActive(roomLeft);
        vortexRight.SetActive(roomRight);

        Buff buff1;
        Buff buff2;
        if (isBuffRoom)
        {
            // applyBuff(BuffContainer.Instance.switchToAutomaticRifle, BuffContainer.Instance.addGunsDamage);

            if (player.GetComponent<Character>().hp >= player.GetComponent<Character>().maxHp)
            {
                BuffContainer.Instance.DecreaseBuffWeight(BuffContainer.Instance.healthRecoveryBuff, 50.0f);
            }
            else
            {
                if (BuffContainer.Instance.isBuffExist(BuffContainer.Instance.healthRecoveryBuff) == false)
                {
                    BuffContainer.Instance.IncreaseBuffWeight(BuffContainer.Instance.healthRecoveryBuff, 30.0f);
                }
            }

            if (isBeforeEndRoom)
            {
                buff1 = BuffContainer.Instance.healthRecoveryBuff;
            }

            if (roomStep >= 3)
            {
                BuffContainer.Instance.IncreaseBuffWeight(BuffContainer.Instance.addGunsDamage, 10.0f);
                BuffContainer.Instance.IncreaseBuffWeight(BuffContainer.Instance.reduceFireInterval, 10.0f);
            }

            buff1 = BuffContainer.Instance.GetRandomBuff();
            buff2 = BuffContainer.Instance.GetRandomBuff();
            while (buff2 == buff1)
            {
                buff2 = BuffContainer.Instance.GetRandomBuff();
            }

            if (buff1.name == "SwitchToAutomaticRifle" || buff2.name == "SwitchToAutomaticRifle")
            {
                BuffContainer.Instance.AddOnceOnlyBuff("SwitchToAutomaticRifle");

            }
            // BuffContainer.Instance.printBuffWeights();
            applyBuff(buff1, buff2);
        }


    }

    public void applyBuff(Buff buff1, Buff buff2)
    {
        Time.timeScale = 0;
        UIManager.Instance.choosePanel.SetActive(true);
        UIManager.Instance.buff1Name.GetComponent<TextMeshProUGUI>().text = buff1.name;
        UIManager.Instance.buff1Description.GetComponent<TextMeshProUGUI>().text = buff1.description;
        UIManager.Instance.buff1ApplyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        UIManager.Instance.buff1ApplyButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuffManager.Instance.AddBuff(buff1);
            UIManager.Instance.choosePanel.SetActive(false);
            //恢复游戏
            Time.timeScale = 1;
        });

        UIManager.Instance.buff2Name.GetComponent<TextMeshProUGUI>().text = buff2.name;
        UIManager.Instance.buff2Description.GetComponent<TextMeshProUGUI>().text = buff2.description;
        UIManager.Instance.buff2ApplyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        UIManager.Instance.buff2ApplyButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuffManager.Instance.AddBuff(buff2);
            UIManager.Instance.choosePanel.SetActive(false);
            //恢复游戏
            Time.timeScale = 1;
        });

    }
}


