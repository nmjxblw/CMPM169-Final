using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffContainer : MonoBehaviour
{
    public GameObject Player;
    private BuffManager buffManager;

    public Buff attackBuff; //攻击力buff
    public Buff defenseBuff; //防御力buff
    public Buff speedBuff; //速度buff
    public Buff healthBuff; //生命值buff
    public Buff shootSpeedBuff; //射击buff
    public Buff healthRecoveryBuff; //生命恢复buff

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
        

        creatHealthBuff();
        creatRecoveryBuff();
    }

    private void Update()
    {
        Player = GameObject.FindWithTag("Player");
    }

    private void creatHealthBuff()
    {
        healthBuff = new Buff("HealthBuff", 0, true, "Increases maximum health by 20%");
        healthBuff.ApplyEffect = () =>
        {
            int maxHp = Player.GetComponent<Character>().maxHp;
            int targetMaxHp = (int)(maxHp * 1.2);
            Player.GetComponent<Character>().SetMaxHp(targetMaxHp);
            int hp = Player.GetComponent<Character>().hp;
            int targetHp = (int)(hp * 1.2);
            Player.GetComponent<Character>().hp = targetHp;
        };
        healthBuff.RemoveEffect = () =>
        {
            int maxHp = Player.GetComponent<Character>().maxHp;
            int targetMaxHp = (int)(maxHp / 1.2);
            Player.GetComponent<Character>().SetMaxHp(targetMaxHp);
            int hp = Player.GetComponent<Character>().hp;
            int targetHp = (int)(hp / 1.2);
            Player.GetComponent<Character>().hp = targetHp;
        };
    }

    private void creatRecoveryBuff()
    {
        healthRecoveryBuff = new Buff("HealthRecoveryBuff", 1, false, "Recovery 50% of maximum health");
        healthRecoveryBuff.ApplyEffect = () =>
        {
            int maxHp = Player.GetComponent<Character>().maxHp;
            int targetHp = (int)(maxHp * 0.5);
            Player.GetComponent<Character>().hp += targetHp;
            if(Player.GetComponent<Character>().hp > maxHp)
            {
                Player.GetComponent<Character>().hp = maxHp;
            }
        };
        healthRecoveryBuff.RemoveEffect = () =>
        {
            
        };
    }




}
