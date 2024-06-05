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
    public List<EnemyConfig> enemyConfigs;
    public NecromancerConfig necromancerConfig;
    public UnityEvent<GameObject> onPlayerSpawnEvent;
    public GameObject quitPanel;
    [SerializeField]
    private bool _gameOver = false;
    public bool GameOver
    {
        get
        { return _gameOver; }
        set
        {
            if (value)
            {
                onGameOverEvent?.Invoke();
            }
            _gameOver = value;
        }
    }
    public UnityEvent onGameOverEvent;
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
        enemyGenerator = GameObject.Find("EventSystem").GetComponent<EnemyGenerator>();
    }

    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscPressed();
        }
    }

    public void InitializedAssets()
    {
        playerConfig.ReadDataFromCSV();
        foreach (var config in gunConfigs)
        {
            config.ReadDataFromCSV();
        }
        foreach (var config in enemyConfigs)
        {
            config.ReadDataFormCSV();
        }
        necromancerConfig.ReadDataFromCSV();
    }

    public void OnDisable()
    {
        onPlayerSpawnEvent.RemoveAllListeners();
    }

    public void EscPressed()
    {
        quitPanel.SetActive(!quitPanel.activeSelf);
        Time.timeScale = quitPanel.activeSelf ? 0 : 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
