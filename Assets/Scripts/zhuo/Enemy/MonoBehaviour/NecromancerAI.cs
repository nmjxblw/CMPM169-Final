using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NercomancerAI : MonoBehaviour
{
    public bool isAwake = false;
    [Header("Component Settings")]
    public NavMeshAgent agent;
    public EnemyCharacter enemyCharacter;
    public NecromancerControl necromancerControl;
    [Header("Agent Data")]
    public GameObject target;
    public Vector3 destination;
    public Vector3 steeringTarget;
    public float distanceToAgentDestination;
    public float distanceToTarget;
    public float moveInterval = 4f;
    public float moveTimer;
    public float stackTimer = 2f;
    public Vector3 lastPos;
    [Header("Random Factor")]
    public int randomFactor;
    public float decisionInterval = 3f;
    public float decisionTimer;
    public enum AttackModel
    {
        nothing,
        attack,
        skill1,
        skill2
    }
    [Header("Attack Model")]
    public AttackModel currentAttackModel = AttackModel.nothing;
    public enum FuryModel
    {
        normal,
        fury
    }
    [Header("Fury Model")]
    [Range(0f, 1f)] public float furyThreshold = 1f / 3f;
    public FuryModel currentFuryModel = FuryModel.normal;
    public void OnEnable()
    {
        target = target == null ? GameObject.FindGameObjectWithTag("Player") : target;

        IEnumerator AwakeInvoke()
        {
            float timer = 2f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            isAwake = true;
            SetDestination();
        }
        StartCoroutine(AwakeInvoke());
    }

    public void SetDestination()
    {
        moveTimer = moveInterval;
        destination = target.transform.position + (Vector3)Random.insideUnitCircle * 20f;
        agent.SetDestination(destination);
        int runtime = 0;
        while (!agent.hasPath)
        {
            runtime++;
            if (runtime >= 20)
            {
                destination = target.transform.position;
                agent.SetDestination(destination);
                break;
            }
            destination = target.transform.position + (Vector3)Random.insideUnitCircle * 20f;
            agent.SetDestination(destination);
        }
    }
    public void Start()
    {
        agent = agent ?? GetComponent<NavMeshAgent>();
        enemyCharacter = enemyCharacter ?? GetComponent<EnemyCharacter>();
        necromancerControl = necromancerControl ?? GetComponent<NecromancerControl>();
        enemyCharacter.onTakenDamage.AddListener(OnTakenDamage);
    }

    public void OnTakenDamage(DamageDealer damageDealer)
    {
        if (currentFuryModel == FuryModel.normal)
            return;
        if (enemyCharacter.hp / enemyCharacter.maxHp <= furyThreshold)
        {
            currentFuryModel = FuryModel.fury;
            decisionInterval = 1f;
        }
    }
    public void Update()
    {
        if (!isAwake) return;
        distanceToAgentDestination = Vector3.Distance(transform.position, destination);
        if (distanceToAgentDestination <= agent.stoppingDistance)
        {
            necromancerControl.inputDirection = Vector2.zero;
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
                SetDestination();
        }
        else
        {
            stackTimer -= Time.deltaTime;
            if (stackTimer <= 0)
            {
                if (Vector3.Distance(transform.position, lastPos) <= 1f)
                {
                    SetDestination();
                }
                else
                {
                    lastPos = transform.position;
                    stackTimer = 2f;
                }
            }
            steeringTarget = agent.steeringTarget;
            Vector3 inputDirectionVector3 = steeringTarget - transform.position;
            necromancerControl.inputDirection = agent.hasPath ? new Vector2(inputDirectionVector3.x, inputDirectionVector3.y).normalized : Vector2.zero;
        }
        if (!necromancerControl.isAttack && !necromancerControl.isSkill)
        {
            if (decisionTimer <= 0)
            {
                //TODO:make a decision
                decisionTimer = decisionInterval;
                MakeDecision();
                ExecuteDecision();
            }
            else
            {
                decisionTimer -= Time.deltaTime;
            }
        }
    }

    public void MakeDecision()
    {
        randomFactor = Random.Range(0, 100);
        if (currentFuryModel == FuryModel.fury)
        {
            if (randomFactor >= 80)
                currentAttackModel = AttackModel.skill2;
            else if (randomFactor >= 50)
                currentAttackModel = AttackModel.skill1;
            else
                currentAttackModel = AttackModel.attack;
        }
        else
        {
            if (randomFactor >= 90)
                currentAttackModel = AttackModel.skill2;
            else if (randomFactor >= 70)
                currentAttackModel = AttackModel.skill1;
            else if (randomFactor >= 40)
                currentAttackModel = AttackModel.attack;
            else
                currentAttackModel = AttackModel.nothing;
        }
    }
    public void ExecuteDecision()
    {
        switch (currentAttackModel)
        {
            case AttackModel.nothing:
                break;
            case AttackModel.attack:
                necromancerControl.HandleAttack();
                break;
            case AttackModel.skill1:
                necromancerControl.HandleSkill1();
                break;
            case AttackModel.skill2:
                necromancerControl.HandleSkill2();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (target != null)
        {
            Gizmos.DrawSphere(target.transform.position, 20f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(destination, 1f);
        }
    }
}
