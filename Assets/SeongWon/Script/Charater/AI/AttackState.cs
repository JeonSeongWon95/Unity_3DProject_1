using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="A.I/States/Attack")]
public class AttackState : AIState
{
    [Header("Current Attack")]
    [HideInInspector] public AICharacterAttackAction mCurrentAttack;
    [HideInInspector] public bool mWillPerformCombo = false;

    [Header("State Flags")]
    protected bool mHasPerformedAttack = false;
    protected bool mHasPerformedCombo = false;

    [Header("Pivot After Attack")]
    [SerializeField] protected bool mPivotAfterAttack = false;

    public override AIState Tick(AICharacterManager mAiCharacter)
    {
        if (mAiCharacter.mAICharacterCombatManager.mCurrentTarget == null)
            return SwitchState(mAiCharacter, mAiCharacter.mIdle);

        if (mAiCharacter.mAICharacterCombatManager.mCurrentTarget.mIsDead.Value)
            return SwitchState(mAiCharacter, mAiCharacter.mIdle);

        mAiCharacter.mCharacterAnimatorManager.UpdateAnimatorValues(0, 0, false);

        if (mWillPerformCombo && !mHasPerformedCombo) 
        {
            if (mCurrentAttack.mComboAction != null)
            {
                mHasPerformedCombo = true;
                mCurrentAttack.mComboAction.AttemToPerformAction(mAiCharacter);
            }
        }

        if (!mHasPerformedAttack) 
        {
            if (mAiCharacter.mAICharacterCombatManager.mActionRecoveryTimer > 0)
                return this;

            if (mAiCharacter.IsPerformingAction)
                return this;

            PerformAttack(mAiCharacter);

            return this;
        }

        return SwitchState(mAiCharacter, mAiCharacter.mCombatStance);

    }

    protected void PerformAttack(AICharacterManager mAICharacterManager)
    { 
        mHasPerformedAttack = true;
        mCurrentAttack.AttemToPerformAction(mAICharacterManager);
        mAICharacterManager.mAICharacterCombatManager.mActionRecoveryTimer = mCurrentAttack.mActionRecoveryTime;
    }

    protected override void ResetStateFlags(AICharacterManager mAiCharacter)
    {
        base.ResetStateFlags(mAiCharacter);
        mHasPerformedAttack = false;
        mHasPerformedCombo |= false;
    }
}
