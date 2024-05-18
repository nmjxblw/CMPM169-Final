using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum BasicBehaviour
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
    [Header("Agent Information")]
    public float distanceToAgentDestination;
    public Vector3 destination;
    public Vector3 steeringTarget;
    [Header("State Machine")]
    public BasicBehaviour currentBehaviour = BasicBehaviour.idle;
    public EnemyState currentState;
    public List<EnemyState> states;
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyControl = enemyControl ?? GetComponent<EnemyControl>();
        StateMachineInitialization();
    }
    private void StateMachineInitialization()
    {
        states = new List<EnemyState>(){
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
            currentBehaviour = BasicBehaviour.chase;
        }
        SwitchState(currentBehaviour);
    }
    public void SwitchState(BasicBehaviour behaviour)
    {
        currentState?.OnExit();
        currentBehaviour = behaviour;
        currentState = states[(int)currentBehaviour];
        currentState.OnEnter();
    }

    public void Update()
    {
        agent.SetDestination(new Vector3(target.position.x, target.position.y, 0f));
        destination = agent.destination;
        distanceToAgentDestination = Vector3.Distance(transform.position, agent.destination);
        steeringTarget = agent.steeringTarget;
        currentState?.OnUpdate();
        // enemyControl.inputDirection = Input.GetAxisRaw("Horizontal") * Vector2.right + Input.GetAxisRaw("Vertical") * Vector2.up;
        Vector3 tempVector3 = steeringTarget - transform.position;
        enemyControl.inputDirection = distanceToAgentDestination >= agent.stoppingDistance ? new Vector2(tempVector3.x, tempVector3.y) : Vector2.zero;
    }
}
