using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager mPlayerManager;
    public WeaponItem mCurrentWeaponBeingUsed;

    [Header("Flags")]
    public bool mCanComboWithMainHandWeapon = false;

    protected override void Awake()
    {
        base.Awake();
        mPlayerManager = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction NewWeaponItemAction, WeaponItem NewWeaponItem) 
    {
        if (mPlayerManager.IsOwner) 
        {
            NewWeaponItemAction.AttemptToPerformAction(mPlayerManager, NewWeaponItem);

            mPlayerManager.mPlayerNetworkManager.NotifyTheServerOfWeaponActionServerRPC(
                NetworkManager.Singleton.LocalClientId, NewWeaponItemAction.mActionID, NewWeaponItem.mItemID);
        }
;
    }

    public virtual void DrainStaminaBasedOnAttack() 
    {
        if (!mPlayerManager.IsOwner)
            return;

        if (mCurrentWeaponBeingUsed == null)
            return;

        float StaminaDeducted = 0;

        switch (mCurrentAttackType)
        {
            case AttackType.LightAttack01:
                StaminaDeducted = mCurrentWeaponBeingUsed.mBaseStaminaCost * 
                    mCurrentWeaponBeingUsed.mLightAttackStaminaCostMultiplier;
                break;
            default:
                break;
        }

        mPlayerManager.mPlayerNetworkManager.mNetworkCurrentStamina.Value -= Mathf.RoundToInt(StaminaDeducted);
    }

    public override void SetTarget(CharacterManager NewTarget)
    {
        base.SetTarget(NewTarget);

        if (mPlayerManager.IsOwner) 
        {
            PlayerCamera.Instance.SetLockCameraHeight();
        }
    }
}
