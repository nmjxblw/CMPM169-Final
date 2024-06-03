using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuffContainer : MonoBehaviour
{
    public GameObject Player;
    public PlayerConfig playerConfig;
    public List<GunConfig> gunConfigs;
    private BuffManager buffManager;

    public Buff attackBuff; //攻击力buff
    public Buff defenseBuff; //防御力buff
    public Buff speedBuff; //速度buff
    public Buff healthBuff; //生命值buff
    public Buff shootSpeedBuff; //射击buff
    public Buff healthRecoveryBuff; //生命恢复buff
    public Buff invincibleLongerBuff; //无敌延长buff
    public Buff addGunsDamage; //增加枪械伤害buff
    public Buff reduceFireInterval;// 减少开火间隔
    public Buff switchToAutomaticRifle;// 切换到自动步枪


    private Dictionary<Buff, float> buffWeights;
    private HashSet<string> onceOnlyBuffs;

    //单例模式
    private static BuffContainer _instance;
    public static BuffContainer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BuffContainer>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("BuffContainer");
                    _instance = obj.AddComponent<BuffContainer>();
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
        buffManager = BuffManager.Instance;


        CreateHealthBuff();
        CreateRecoveryBuff();
        CreatDefenceBuff();
        CreateSpeedBuff();
        CreateInvincibleLongerBuff();
        CreateAddGunsDamage(1);
        CreateReduceFireInterval(0.2f);
        CreateSwitchToAutomaticRifle();
        InitializeBuffWeights();
    }

    private void Update()
    {
        Player = GameObject.FindWithTag("Player");
    }

    private void CreateHealthBuff()
    {
        healthBuff = new Buff("HealthBuff", 0, true, "Increases maximum health by 20%");
        healthBuff.ApplyEffect = () =>
        {
            int maxHp = Player.GetComponent<Character>().maxHp;
            int targetMaxHp = (int)(maxHp * 0.2);
            Player.GetComponent<Character>().maxHp += targetMaxHp;
            Player.GetComponent<Character>().hp += targetMaxHp;
        };
        healthBuff.RemoveEffect = () =>
        {
            int maxHp = Player.GetComponent<Character>().maxHp;
            int targetMaxHp = (int)(maxHp * 0.2);
            Player.GetComponent<Character>().maxHp -= targetMaxHp;
            Player.GetComponent<Character>().hp -= targetMaxHp;
        };
    }

    private void CreateRecoveryBuff()
    {
        healthRecoveryBuff = new Buff("HealthRecoveryBuff", 1, false, "Recovery 50% of maximum health");
        healthRecoveryBuff.ApplyEffect = () =>
        {
            int maxHp = Player.GetComponent<Character>().maxHp;
            int targetHp = (int)(maxHp * 0.5);
            Player.GetComponent<Character>().hp += targetHp;
        };
        healthRecoveryBuff.RemoveEffect = () =>
        {

        };
    }

    private void CreateInvincibleLongerBuff()
    {
        invincibleLongerBuff = new Buff("InvincibleLongerBuff", 0, true, "Extend invincibility time by 0.5 seconds");
        invincibleLongerBuff.ApplyEffect = () =>
        {
            playerConfig.invincibleDuration += 0.5f;
        };

        invincibleLongerBuff.RemoveEffect = () =>
        {
            playerConfig.invincibleDuration -= 0.5f;
        };
    }

    private void CreateAddGunsDamage(int damage)
    {
        addGunsDamage = new Buff("AddGunsDamage", 0, true, $"Increase {damage} damages of all guns");
        addGunsDamage.ApplyEffect = () =>
        {
            foreach (GunConfig gunConfig in gunConfigs)
            {
                gunConfig.damage += damage;
            }
        };
        addGunsDamage.RemoveEffect = () =>
        {
            foreach (GunConfig gunConfig in gunConfigs)
            {
                gunConfig.damage -= damage;
            }
        };
    }

    private void CreateReduceFireInterval(float rate)
    {
        reduceFireInterval = new Buff("ReduceFireInterval", 0, true, $"Reduce fire interval by {rate * 100f} %");
        reduceFireInterval.ApplyEffect = () =>
        {
            foreach (GunConfig gunConfig in gunConfigs)
            {
                gunConfig.fireInterval *= (1f - rate);
            }
        };
        reduceFireInterval.RemoveEffect = () =>
        {
            foreach (GunConfig gunConfig in gunConfigs)
            {
                gunConfig.fireInterval /= (1f - rate);
            }
        };
    }

    private void CreatDefenceBuff()
    {
        defenseBuff = new Buff("DefenceBuff", 0, true, "All damage received is reduced by 1");
        defenseBuff.ApplyEffect = () =>
        {
            Player.GetComponent<PlayerConfig>().defense += 1;
        };
        defenseBuff.RemoveEffect = () =>
        {
            Player.GetComponent<PlayerConfig>().defense -= 1;
        };
    }

    private void CreateSpeedBuff()
    {
        speedBuff = new Buff("SpeedBuff", 0, true, "Movement speed increased by 20%");
        speedBuff.ApplyEffect = () =>
        {
            Player.GetComponent<PlayerConfig>().moveSpeed += 10;
        };
        speedBuff.RemoveEffect = () =>
        {
            Player.GetComponent<PlayerConfig>().moveSpeed -= 10;
        };
    }

    public void CreateSwitchToAutomaticRifle()
    {
        switchToAutomaticRifle = new Buff("SwitchToAutomaticRifle", 0, true, "Switch to automatic rifle");
        switchToAutomaticRifle.ApplyEffect = () =>
        {
            Player.transform.Find("GunParent").GetComponent<GunController>().SelectGun(GunType.AutomaticRifle);
        };
        switchToAutomaticRifle.RemoveEffect = () =>
        {

        };
    }

    private void InitializeBuffWeights()
    {
        buffWeights = new Dictionary<Buff, float>
        {
            { healthBuff, 30 },
            { healthRecoveryBuff, 30 },
            { invincibleLongerBuff, 30 },
            { addGunsDamage, 10 },
            { reduceFireInterval, 10 },
            // { switchToAutomaticRifle, 25 }
        };

        onceOnlyBuffs = new HashSet<string> {  };
    }

    public void IncreaseBuffWeight(Buff buff, float increment)
    {
        if (buffWeights.ContainsKey(buff))
        {
            buffWeights[buff] += increment;
        }
        else
        {
            buffWeights[buff] = increment;
        }
    }

    public void DecreaseBuffWeight(Buff buff, float decrement)
    {
        if (buffWeights.ContainsKey(buff) && buffWeights[buff] > decrement)
        {
            buffWeights[buff] -= decrement;
        }
        else
        {
            buffWeights.Remove(buff);
        }
    }

    public bool isBuffExist(Buff buff)
    {
        return buffWeights.ContainsKey(buff);
    }

    public void SetBuffWeight(Buff buff, float newWeight)
    {
        if (newWeight > 0)
        {
            buffWeights[buff] = newWeight;
        }
        else
        {
            buffWeights.Remove(buff);
        }
    }

    public void AddOnceOnlyBuff(string buffName)
    {
        onceOnlyBuffs.Add(buffName);
    }

    public Buff GetRandomBuff()
    {
        if (onceOnlyBuffs.Count > 0)
        {
            buffWeights.Remove(buffWeights.Keys.FirstOrDefault(b => onceOnlyBuffs.Contains(b.name)));
            onceOnlyBuffs.Clear();
        }


        float totalWeight = buffWeights.Values.Sum();
        float randomPoint = Random.value * totalWeight;

        foreach (var buff in buffWeights)
        {
            if (randomPoint < buff.Value)
            {
                return buff.Key;
            }
            randomPoint -= buff.Value;   
        }
        return null;
    }

    public void printBuffWeights()
    {
        foreach (var buff in buffWeights)
        {
            Debug.Log(buff.Key.name + ": " + buff.Value);
        }
    }
}
