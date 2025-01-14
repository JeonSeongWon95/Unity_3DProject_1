using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossCombatManager : AICharacterCombatManager
{
    [Header("Damage Collider")]
    [SerializeField] BossJumpAttackDamageCollider mJumpAttackCollider;
    [SerializeField] BossMagicAttackDamageCollider mMagicAttackCollider;

    [Header("Damage")]
    [SerializeField] int mBaseDamage = 25;
    [SerializeField] float mMagicAttackDamageModifier = 1.0f;
    [SerializeField] float mJumpAttackDamageModifier = 1.4f;

    public void SetMagicAttackDamage()
    {
        mMagicAttackCollider.mPhysicalDamage = mBaseDamage * mMagicAttackDamageModifier;
    }

    public void SetJumpAttackDamage()
    {
        mJumpAttackCollider.mPhysicalDamage = mBaseDamage * mJumpAttackDamageModifier;
    }

    public void EnableJumpAttackDamageCollider()
    {
        mAICharacterManager.mCharacterSoundFXManager.PlayAttackGrunt();
        mJumpAttackCollider.EnableDamageCollider();
    }

    public void DisableJumpAttackDamageCollider()
    {
        mJumpAttackCollider.DisableDamageCollider();
    }

    public void EnableMagicAttackDamageCollider()
    {
        mAICharacterManager.mCharacterSoundFXManager.PlayAttackGrunt();
        mMagicAttackCollider.EnableDamageCollider();
    }

    public void DisableMagicAttackDamageCollider()
    {
        mMagicAttackCollider.DisableDamageCollider();
    }
}
