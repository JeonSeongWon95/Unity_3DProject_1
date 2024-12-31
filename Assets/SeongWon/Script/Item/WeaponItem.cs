using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject mWeaponModel;

    [Header("Weapon Requirements")]
    public int mStrengthREQ = 0;
    public int mDexREQ = 0;
    public int mIntREQ = 0;
    public int mFaitREQ = 0;

    [Header("Weapon Base Damage")]
    public int mPhysicalDamage = 0;
    public int mMagicDamage = 0;
    public int mFireDamage = 0;
    public int mHolyDamage = 0;
    public int mLightningDamage = 0;

    [Header("Weapon Poise")]
    public float mPoiseDamage = 10;

    [Header("Attacks Modifiers")]
    public float Light_Attack_01_Modifier = 1.1f;
    public float Light_Attack_02_Modifier = 1.3f;
    public float Strong_Attack_01_Modifier = 1.5f;
    public float Strong_Attack_02_Modifier = 2.0f;
    public float Charge_Attack_01_Modifier = 1.3f;
    public float Charge_Attack_02_Modifier = 1.6f;


    [Header("Weapon Actions")]
    public WeaponItemAction OH_NomalAction;
    public WeaponItemAction OH_StrongAction;
    public WeaponItemAction OH_ChargeAction;


    [Header("Stamina Cost")]
    public int mBaseStaminaCost = 20;
    public float mLightAttackStaminaCostMultiplier = 0.9f;
}
