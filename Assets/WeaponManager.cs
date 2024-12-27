using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider mMeleeDamageCollider;

    private void Awake()
    {
        mMeleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager DamageCasuingCharacter, WeaponItem NewWeaponItem) 
    {
        mMeleeDamageCollider.mCharacterCausingDamage = DamageCasuingCharacter;
        mMeleeDamageCollider.mPhysicalDamage = NewWeaponItem.mPhysicalDamage;
        mMeleeDamageCollider.mMagicDamage = NewWeaponItem.mMagicDamage;
        mMeleeDamageCollider.mHolyDamage = NewWeaponItem.mHolyDamage;
        mMeleeDamageCollider.mFireDamage = NewWeaponItem.mFireDamage;
        mMeleeDamageCollider.mLightningDamage = NewWeaponItem.mLightningDamage;
        mMeleeDamageCollider.Light_Attack_01_Modifier = NewWeaponItem.Light_Attack_01_Modifier;
    }
}
