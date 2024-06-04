using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField]
    public static int levelDifficulty = 0;
    public PlayerConfig playerConfig;
    public List<GunConfig> gunConfigs;
    public UnityEvent<GameObject> onPlayerSpawnEvent;
    bool GameOver = false;
    public GameObject player;
    public EnemyGenerator enemyGenerator;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
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
        InitializedAssets();
    }

    void Start()
    {
        onPlayerSpawnEvent.AddListener((player) =>
        {
            this.player = player;
        });

        enemyGenerator = GameObject.Find("EventSystem").GetComponent<EnemyGenerator>();
    }

    void Update()
    {
        // if(player != null)
        // {
        //     GameObject.FindGameObjectWithTag("Player");
        // }

        if (player.GetComponent<Character>().hp <= 0)
        {
            GameOver = true;

        }

        if (GameOver && Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.InitializedAssets();
            //关掉enemyGenerator脚本
            if (enemyGenerator != null)
            {
                enemyGenerator.enabled = false; // 禁用 EnemyGenerator 脚本
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void InitializedAssets()
    {
        playerConfig.ReadDataFromCSV();
        foreach (var config in gunConfigs)
        {
            config.ReadDataFromCSV();
        }
    }

    public void OnDisable(){
        onPlayerSpawnEvent.RemoveAllListeners();
    }
}
