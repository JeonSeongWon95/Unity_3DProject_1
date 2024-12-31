using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHUDManager : MonoBehaviour
{
    [Header("STAT BARS")]
    [SerializeField] UI_StatBar mHealthBar;
    [SerializeField] UI_StatBar mStaminaBar;

    [Header("QUICK SLOTS")]
    [SerializeField] Image mRightWeaponQuickSlotIcon;
    [SerializeField] Image mLeftWeaponQuickSlotIcon;
    public void SetStaminaValue(float OldValue, float NewValue) 
    {
        mStaminaBar.SetStats(Mathf.RoundToInt(NewValue));
    }

    public void SetMaxStaminaValue(int MaxStamina) 
    {
        mStaminaBar.SetMaxStats(MaxStamina);
    }

    public void SetHealthValue(float OldValue, float NewValue)
    {
        mHealthBar.SetStats(Mathf.RoundToInt(NewValue));
    }

    public void SetMaxHealthValue(int MaxHealth)
    {
        mHealthBar.SetMaxStats(MaxHealth);
    }

    public void RefeshHUD() 
    {
        mHealthBar.gameObject.SetActive(false);
        mHealthBar.gameObject.SetActive(true);
        mStaminaBar.gameObject.SetActive(false);
        mStaminaBar.gameObject.SetActive(true);
    }

    public void SetRightWeaponQuickSlotIcon(int WeaponID) 
    {
        WeaponItem Weapon = WorldItemDataBase.Instance.GetWeaponByID(WeaponID);

        if (Weapon == null) 
        {
            mRightWeaponQuickSlotIcon.enabled = false;
            mRightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (Weapon.mItemIcon == null) 
        {
            mRightWeaponQuickSlotIcon.enabled = false;
            mRightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        mRightWeaponQuickSlotIcon.sprite = Weapon.mItemIcon;
        mRightWeaponQuickSlotIcon.enabled = true;
    }

    public void SetLeftWeaponQuickSlotIcon(int WeaponID)
    {
        WeaponItem Weapon = WorldItemDataBase.Instance.GetWeaponByID(WeaponID);

        if (Weapon == null)
        {
            mLeftWeaponQuickSlotIcon.enabled = false;
            mLeftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (Weapon.mItemIcon == null)
        {
            mLeftWeaponQuickSlotIcon.enabled = false;
            mLeftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        mLeftWeaponQuickSlotIcon.sprite = Weapon.mItemIcon;
        mLeftWeaponQuickSlotIcon.enabled = true;
    }
}
