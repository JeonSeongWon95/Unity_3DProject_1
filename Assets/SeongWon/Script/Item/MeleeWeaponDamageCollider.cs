using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{

    [Header("Attacking Character")]
    public CharacterManager mCharacterCausingDamage;

    [Header("Weapon Attack Modifiers")]
    public float Light_Attack_01_Modifier;
    public float Light_Attack_02_Modifier;
    public float Strong_Attack_01_Modifier;
    public float Strong_Attack_02_Modifier;
    public float Charge_Attack_01_Modifier;
    public float Charge_Attack_02_Modifier;

    protected override void Awake()
    {
        base.Awake();
        mDamageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {

        CharacterManager mDamageTarget = other.GetComponentInParent<CharacterManager>();

        if (mDamageTarget != null)
        {
            if (mDamageTarget == mCharacterCausingDamage)
                return;

            mContactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(mDamageTarget);
        }
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
        damageEffect.mAngleHitForm = Vector3.SignedAngle(mCharacterCausingDamage.transform.forward,
            DamageTarget.transform.forward, Vector3.up);

        switch (mCharacterCausingDamage.mCharacterCombatManager.mCurrentAttackType) 
        {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifiers(Light_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.LightAttack02:
                ApplyAttackDamageModifiers(Light_Attack_02_Modifier, damageEffect);
                break;
            case AttackType.StrongAttack01:
                ApplyAttackDamageModifiers(Strong_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.StrongAttack02:
                ApplyAttackDamageModifiers(Strong_Attack_02_Modifier, damageEffect);
                break;
            case AttackType.ChargeAttack01:
                ApplyAttackDamageModifiers(Charge_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.ChargeAttack02:
                ApplyAttackDamageModifiers(Charge_Attack_02_Modifier, damageEffect);
                break;
            default:
                break;
        }

        if (mCharacterCausingDamage.IsOwner) 
        {
            DamageTarget.mCharacterNetworkManager.NotifyTheServerOfCharacterDamageServerRPC(DamageTarget.NetworkObjectId,
                mCharacterCausingDamage.NetworkObjectId, damageEffect.mPhysicalDamage, damageEffect.mMagicDamage,
                damageEffect.mFireDamage, damageEffect.mHolyDamage, damageEffect.mPoiseDamage, damageEffect.mAngleHitForm,
                damageEffect.mContactPoint.x, damageEffect.mContactPoint.y, damageEffect.mContactPoint.z);
        }
    }

    private void ApplyAttackDamageModifiers(float modifier, TakeHealthDamageEffect Damage) 
    {
        Damage.mPhysicalDamage *= modifier;
        Damage.mMagicDamage *= modifier;
        Damage.mLightningDamage *= modifier;
        Damage.mHolyDamage *= modifier;
        Damage.mFireDamage *= modifier;
        Damage.mPoiseDamage *= modifier;
    }


}
