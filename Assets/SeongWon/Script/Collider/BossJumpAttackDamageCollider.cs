using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJumpAttackDamageCollider : DamageCollider
{
    [SerializeField] AIBossCharacterManager mAICharacterCasuingDamage;
    protected override void Awake()
    {
        base.Awake();

        mDamageCollider = GetComponent<Collider>();
        mAICharacterCasuingDamage = GetComponent<AIBossCharacterManager>();
    }

    protected override void DamageTarget(CharacterManager DamageTarget)
    {
        if (mCahractersDamaged.Contains(DamageTarget))
            return;

        mCahractersDamaged.Add(DamageTarget);

        TakeHealthDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.mTakeHealthDamageEffect);
        damageEffect.mPhysicalDamage = mPhysicalDamage;
        damageEffect.mMagicDamage = mMagicDamage;
        damageEffect.mLightningDamage = mLightningDamage;
        damageEffect.mHolyDamage = mHolyDamage;
        damageEffect.mLightningDamage = mLightningDamage;
        damageEffect.mContactPoint = mContactPoint;
        damageEffect.mAngleHitForm = Vector3.SignedAngle(mAICharacterCasuingDamage.transform.forward,
            DamageTarget.transform.forward, Vector3.up);

        if (DamageTarget.IsOwner)
        {
            DamageTarget.mCharacterNetworkManager.NotifyTheServerOfCharacterDamageServerRPC(DamageTarget.NetworkObjectId,
                mAICharacterCasuingDamage.NetworkObjectId, damageEffect.mPhysicalDamage, damageEffect.mMagicDamage,
                damageEffect.mFireDamage, damageEffect.mHolyDamage, damageEffect.mPoiseDamage, damageEffect.mAngleHitForm,
                damageEffect.mContactPoint.x, damageEffect.mContactPoint.y, damageEffect.mContactPoint.z);
        }
    }
}
