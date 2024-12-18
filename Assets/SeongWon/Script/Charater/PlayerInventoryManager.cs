using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItem mCurrentRightWeapon;
    public WeaponItem mCurrentLeftWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] mWeaponsInRightHandSlots = new WeaponItem[3];
    public int mRightHandWeaponIndex = 0;
    public WeaponItem[] mWeaponInLeftHandSlots = new WeaponItem[3];
    public int mLeftHandWeaponIndex = 0;
}
