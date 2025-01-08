using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombieCombatManager : AICharacterCombatManager
{ 

    [Header("Damage Colliders")]
    [SerializeField] EnemyHandDamageCollider mRightHandCollider;
    [SerializeField] EnemyHandDamageCollider mLeftHandCollider;

    [Header("Damage")]
    [SerializeField] int mBaseDamage = 25;
    [SerializeField] float mNomalAttackDamageModifier = 1.0f;
    [SerializeField] float mStrongAttackDamageModifier = 1.4f;

    public void SetNomalAttackDamage() 
    {
        mRightHandCollider.mPhysicalDamage = mBaseDamage * mNomalAttackDamageModifier;
        mLeftHandCollider.mPhysicalDamage = mBaseDamage * mNomalAttackDamageModifier;
    }

    public void SetStrongAttackDamage()
    {
        mRightHandCollider.mPhysicalDamage = mBaseDamage * mStrongAttackDamageModifier;
        mLeftHandCollider.mPhysicalDamage = mBaseDamage * mStrongAttackDamageModifier;
    }

    public void EnableRightHandDamageCollider() 
    {
        mAICharacterManager.mCharacterSoundFXManager.PlayAttackGrunt();
        mRightHandCollider.EnableDamageCollider();
    }

    public void DisableRightHandDamageCollider() 
    {
        mRightHandCollider.DisableDamageCollider();
    }

    public void EnableLeftHandDamageCollider()
    {
        mAICharacterManager.mCharacterSoundFXManager.PlayAttackGrunt();
        mLeftHandCollider.EnableDamageCollider();
    }

    public void DisableLeftHandDamageCollider()
    {
        mLeftHandCollider.DisableDamageCollider();
    }
}
