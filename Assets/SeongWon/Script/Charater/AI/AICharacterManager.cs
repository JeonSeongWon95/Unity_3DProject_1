using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterManager : CharacterManager
{
    public AICharacterCombatManager mAICharacterCombatManager;

    [Header("Currnet State")]
    [SerializeField] AIState mCurrentState;

    protected override void Awake()
    {
        base.Awake();
        mAICharacterCombatManager = GetComponent<AICharacterCombatManager>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ProcessStateMachine();
    }

    private void ProcessStateMachine() 
    {
        AIState NextState = mCurrentState?.Tick(this);

        if (NextState != null)
        {
            mCurrentState = NextState;
        }
    }
}
