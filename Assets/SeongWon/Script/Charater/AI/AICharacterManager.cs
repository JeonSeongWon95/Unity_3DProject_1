using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    [HideInInspector]public AICharacterCombatManager mAICharacterCombatManager;
    [HideInInspector]public AICharacterNetworkManager mAICharacterNetworkManager;
    [HideInInspector]public AICharacterLocomotionManager mAICharacterLocomotionManager;

    [Header("Navmesh Agent")]
    public NavMeshAgent mNavMeshAgent;

    [Header("Currnet State")]
    [SerializeField] AIState mCurrentState;

    [Header("States")]
    public IdleState mIdle;
    public PursueTargetState mPursueTarget;
    public CombatStanceState mCombatStance;
    public AttackState mAttack;

    protected override void Awake()
    {
        base.Awake();
        mAICharacterCombatManager = GetComponent<AICharacterCombatManager>();
        mAICharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
        mNavMeshAgent = GetComponentInChildren<NavMeshAgent>();
        mAICharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

        mIdle = Instantiate(mIdle);
        mPursueTarget = Instantiate(mPursueTarget);

        mCurrentState = mIdle;
    }

    protected override void Update()
    {
        base.Update();

        mAICharacterCombatManager.HandleActionRecovery(this);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsOwner)
        {
            ProcessStateMachine();
        }
    }

    private void ProcessStateMachine() 
    {

        AIState NextState = mCurrentState?.Tick(this);

        if (NextState != null)
        {
            mCurrentState = NextState;
        }

        mNavMeshAgent.transform.localPosition = Vector3.zero;
        mNavMeshAgent.transform.localRotation = Quaternion.identity;

        if (mAICharacterCombatManager.mCurrentTarget != null)
        {
            mAICharacterCombatManager.mTargetDirection = mAICharacterCombatManager.mCurrentTarget.transform.position -
                transform.position;

            mAICharacterCombatManager.mDistanceFromTarget = Vector3.Distance(transform.position,
                mAICharacterCombatManager.mCurrentTarget.transform.position);
        }

        if (mNavMeshAgent.enabled)
        {
            Vector3 AgentDestination = mNavMeshAgent.destination;
            float RemainDistance = Vector3.Distance(AgentDestination, transform.position);

            if (RemainDistance > mNavMeshAgent.stoppingDistance)
            {
                mAICharacterNetworkManager.mNetworkIsMoving.Value = true;
            }
            else
            {
                mAICharacterNetworkManager.mNetworkIsMoving.Value = false;

            }
        }
        else 
        {
            mAICharacterNetworkManager.mNetworkIsMoving.Value = false;
        }
    }
}
