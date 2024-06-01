using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        CreateInvincibleLongerBuff();
        CreateAddGunsDamage(1);
        CreateReduceFireInterval(0.2f);
        CreateSwitchToAutomaticRifle();
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
}
