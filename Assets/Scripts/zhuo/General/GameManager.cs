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
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        InitializedAssets();
    }

    public void InitializedAssets()
    {
        playerConfig.ReadDataFromCSV();
        foreach (var config in gunConfigs)
        {
            config.ReadDataFromCSV();
        }
    }

    void Update()
    {
        
    }
}
