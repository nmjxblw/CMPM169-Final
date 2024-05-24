using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum Logic
{
    idle = 0,
    chase = 1,
    attack = 2,
    skill = 3,
    hurt = 4,
    dead = 5
}
public class EnemyAI : MonoBehaviour
{
    [Header("Basic Components")]
    public NavMeshAgent agent;
    public Transform target;
    public EnemyControl enemyControl;
    public Character enemyCharacter;
    [Header("Agent Information")]
    public float distanceToAgentDestination;
    public Vector3 destination;
    public Vector3 steeringTarget;
    public Vector2 inputDirection = new Vector2();
    [Header("State Machine")]
    public Logic currentLogic = Logic.idle;
    public AIState currentState;
    public List<AIState> states;
    [Header("Attack Logic")]
    public float skillRange = 2f;
    public bool targetInSkillRange;
    public GameObject player;
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyControl = enemyControl ?? GetComponent<EnemyControl>();
        enemyControl.isSkillChanged.AddListener(HandleIsSkillChanged);
        enemyCharacter = enemyCharacter ?? GetComponent<Character>();
        enemyCharacter.onTakenDamage.AddListener(HandleTakeDamage);
        enemyCharacter.onDead.AddListener(HandleDead);
        StateMachineInitialization();
        SwitchState(currentLogic);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void StateMachineInitialization()
    {
        states = new List<AIState>(){
            new Idle(this),
            new Chase(this),
            new Attack(this),
            new Skill(this),
            new Hurt(this),
            new Dead(this),
        };
    }

    public void Spawned()
    {
        agent.SetDestination(target.position);
        if (agent.hasPath)
        {
            currentLogic = Logic.chase;
        }
        else
        {
            currentLogic = Logic.idle;
        }
        SwitchState(currentLogic);
    }
    public void SwitchState(Logic behaviour)
    {
        currentState?.OnExit();
        currentLogic = behaviour;
        currentState = states[(int)currentLogic];
        currentState.OnEnter();
    }

    public void Update()
    {
        target = player.transform;
        agent.SetDestination(new Vector3(target.position.x, target.position.y, 0));
        destination = agent.destination;
        distanceToAgentDestination = Vector3.Distance(transform.position, agent.destination);
        if (distanceToAgentDestination > agent.stoppingDistance)
        {
            steeringTarget = agent.steeringTarget;
            Vector3 inputDirectionVector3 = steeringTarget - transform.position;
            inputDirection = new Vector2(inputDirectionVector3.x, inputDirectionVector3.y).normalized;
        }
        else
        {
            inputDirection = Vector2.zero;
        }
        currentState?.OnUpdate();
        enemyControl.inputDirection = inputDirection;
    }

    public void HandleIsSkillChanged(bool isSkill)
    {
        if (isSkill) return;
        SwitchState(Logic.chase);
    }

    public void HandleTakeDamage(DamageDealer damageDealer)
    {
        if (currentLogic == Logic.idle)
            SwitchState(Logic.chase);
        return;
    }

    public void HandleDead()
    {
        SwitchState(Logic.dead);
    }
}
