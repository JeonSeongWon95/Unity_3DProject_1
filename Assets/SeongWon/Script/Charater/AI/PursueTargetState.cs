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

        if (mAiCharacter.mAICharacterCombatManager.mViewableAngle <
            mAiCharacter.mAICharacterCombatManager.mMinmumDetectionAngle ||
            mAiCharacter.mAICharacterCombatManager.mViewableAngle >
            mAiCharacter.mAICharacterCombatManager.mMaximumDetectionAngle)
        {
            mAiCharacter.mAICharacterCombatManager.PivotTowardsTarget(mAiCharacter);
        }

        mAiCharacter.mAICharacterLocomotionManager.RotateTowardsAgent(mAiCharacter);

        NavMeshPath mPath = new NavMeshPath();

        mAiCharacter.mNavMeshAgent.CalculatePath(
            mAiCharacter.mAICharacterCombatManager.mCurrentTarget.transform.position, mPath);

        mAiCharacter.mNavMeshAgent.SetPath(mPath);

        return this;

    }
}
