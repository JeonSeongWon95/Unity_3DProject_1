using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
public class PursueTargetState : AIState
{
    public override AIState Tick(AICharacterManager mAiCharacter)
    {
        if (mAiCharacter.IsPerformingAction)
            return this;

        if (mAiCharacter.mAICharacterCombatManager.mCurrentTarget == null)
            return SwitchState(mAiCharacter, mAiCharacter.mIdle);

        if (!mAiCharacter.mNavMeshAgent.enabled)
            mAiCharacter.mNavMeshAgent.enabled = true;

        mAiCharacter.mAICharacterLocomotionManager.RotateTowardsAgent(mAiCharacter);

        if(mAiCharacter.mAICharacterCombatManager.mDistanceFromTarget <= mAiCharacter.mNavMeshAgent.stoppingDistance)
            return SwitchState(mAiCharacter, mAiCharacter.mCombatStance);

        NavMeshPath mPath = new NavMeshPath();

        mAiCharacter.mNavMeshAgent.CalculatePath(
            mAiCharacter.mAICharacterCombatManager.mCurrentTarget.transform.position, mPath);

        mAiCharacter.mNavMeshAgent.SetPath(mPath);

        return this;

    }
}
