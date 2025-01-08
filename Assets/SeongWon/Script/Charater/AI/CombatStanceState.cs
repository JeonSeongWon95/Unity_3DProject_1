using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName ="A.I/States/Combat Stance")]
public class CombatStanceState : AIState
{
    [Header("Attacks")]
    public List<AICharacterAttackAction> mAICharacterAttacks;
    protected List<AICharacterAttackAction> mPotentialAttacks;
    private AICharacterAttackAction mChooseAttack;
    private AICharacterAttackAction mPreviousAttack;
    protected bool HasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool CanPerformCombo = false;
    [SerializeField] protected int ChanceToPerformCombo = 25;
    protected bool HasRolledForComboChance = false;

    [Header("Engagement Distance")]
    [SerializeField] public float MaximumEngagementDistance = 5;

    public override AIState Tick(AICharacterManager mAiCharacter)
    {
        if (mAiCharacter.IsPerformingAction)
            return this;

        if (!mAiCharacter.mNavMeshAgent.enabled)
        {
            mAiCharacter.mNavMeshAgent.enabled = true;
        }

        if (mAiCharacter.mAICharacterCombatManager.mCurrentTarget == null)
            return SwitchState(mAiCharacter, mAiCharacter.mIdle);

        if (!HasAttack)
        {
            GetNewAttack(mAiCharacter);
        }
        else 
        {
            mAiCharacter.mAttack.mCurrentAttack = mChooseAttack;
            return SwitchState(mAiCharacter, mAiCharacter.mAttack);
        }

        if (mAiCharacter.mAICharacterCombatManager.mDistanceFromTarget > MaximumEngagementDistance)
            return SwitchState(mAiCharacter, mAiCharacter.mPursueTarget);

        NavMeshPath Path = new NavMeshPath();
        mAiCharacter.mNavMeshAgent.CalculatePath(mAiCharacter.mAICharacterCombatManager.mCurrentTarget.transform.position, Path);
        mAiCharacter.mNavMeshAgent.SetPath(Path);
        return this;

    }

    protected virtual void GetNewAttack(AICharacterManager mAiCharacter) 
    {
        mPotentialAttacks = new List<AICharacterAttackAction>();
       

        foreach (var PotentialAttacks in mAICharacterAttacks)
        {
            if (PotentialAttacks.mMinmumAttackDistance > mAiCharacter.mAICharacterCombatManager.mDistanceFromTarget)
                continue;

            if (PotentialAttacks.mMaxmumAttackDistnace < mAiCharacter.mAICharacterCombatManager.mDistanceFromTarget)
                continue;

            if (PotentialAttacks.mMinmumAttackAngle > mAiCharacter.mAICharacterCombatManager.mViewableAngle)
                continue;

            if (PotentialAttacks.mMaxmumAttackAngle < mAiCharacter.mAICharacterCombatManager.mViewableAngle)
                continue;

            mPotentialAttacks.Add(PotentialAttacks);
        }

        if (mPotentialAttacks.Count <= 0)
            return;

        var TotalWeight = 0;

        foreach (var attack in mPotentialAttacks)
        {
            TotalWeight += attack.mAttackWeight;  
        }

        var RandomWeightValue = Random.Range(1, TotalWeight + 1);
        var ProcessedWeight = 0;

        foreach (var attack in mPotentialAttacks)
        {
            ProcessedWeight  += attack.mAttackWeight;

            if (RandomWeightValue <= ProcessedWeight) 
            {
                mChooseAttack = attack;
                mPreviousAttack = mChooseAttack;
                HasAttack = true;
                return;
            }
        }
    }


    protected virtual bool RollForOutComeChance(int OutComeChance) 
    {
        bool OutComeWillBePerformed = false;

        int RandomPercentage = Random.Range(0, 100);

        if (RandomPercentage < OutComeChance) 
        {
            OutComeWillBePerformed = true;
        }

        return OutComeWillBePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager mAiCharacter)
    {
        base.ResetStateFlags(mAiCharacter);

        HasRolledForComboChance = false;
        HasAttack = false;
    }

}
