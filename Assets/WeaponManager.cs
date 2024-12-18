using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] MeleeWeaponDamageCollider mMeleeDamageCollider;

    private void Awake()
    {
        mMeleeDamageCollider = GetComponent<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager DamageCasuingCharacter, WeaponItem NewWeaponItem) 
    {
        mMeleeDamageCollider.mCharacterCausingDamage = DamageCasuingCharacter;
        mMeleeDamageCollider.mPhysicalDamage = NewWeaponItem.mPhysicalDamage;
        mMeleeDamageCollider.mMagicDamage = NewWeaponItem.mMagicDamage;
        mMeleeDamageCollider.mHolyDamage = NewWeaponItem.mHolyDamage;
        mMeleeDamageCollider.mFireDamage = NewWeaponItem.mFireDamage;
        mMeleeDamageCollider.mLightningDamage = NewWeaponItem.mLightningDamage;
    }
}
