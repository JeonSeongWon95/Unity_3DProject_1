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

    [Header("Weapon Bae Damage")]
    public int mPhysicalDamage = 0;
    public int mMagicDamage = 0;
    public int mFireDamage = 0;
    public int mHolyDamage = 0;
    public int mLightningDamage = 0;

    [Header("Weapon Poise")]
    public float mPoiseDamage = 10;

    [Header("Stamina Cost")]
    public int mBaseStaminaCost = 20;
}
