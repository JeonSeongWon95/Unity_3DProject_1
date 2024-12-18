using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Health Damage")]
public class TakeHealthDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager mCharacterCauingDamage;

    [Header("Damage")]
    public float mPhysicalDamage = 0;
    public float mMagicDamage = 0;
    public float mFireDamage = 0;
    public float mLightningDamage = 0;
    public float mHolyDamage = 0;

    [Header("Final Damage")]
    private int mFinalDamageDealt = 0;

    [Header("Poise")]
    public float mPoiseDamage = 0;
    public bool mPoiseIsBroken = false;

    [Header("Animation")]
    public bool mPlayDamageAnimation = true;
    public bool mManuallySelectDamageAnimation = false;
    public string mDamageAnimation;

    [Header("Sound FX")]
    public bool mWillPlayDamageSFX = true;
    public AudioClip mElementalDamageSoundFX;

    [Header("Direction Damage Taken Form")]
    public float mAngleHitForm;
    public Vector3 mContactPoint;
    public override void ProcessEffect(CharacterManager NewCharacterManager)
    {
        base.ProcessEffect(NewCharacterManager);

        if (NewCharacterManager.mIsDead.Value)
            return;

        CalculateDamage(NewCharacterManager);
    }

    private void CalculateDamage(CharacterManager NewCharacterManager) 
    {
        if (!NewCharacterManager.IsOwner)
            return;

        if (mCharacterCauingDamage != null) 
        {

        }

        mFinalDamageDealt = Mathf.RoundToInt(mPhysicalDamage + mMagicDamage + 
            mFireDamage + mLightningDamage + mHolyDamage);

        if (mFinalDamageDealt <= 0) 
        {
            mFinalDamageDealt = 1;
        }

        NewCharacterManager.mCharacterNetworkManager.mNetworkCurrentHealth.Value -= mFinalDamageDealt;
    }
}
