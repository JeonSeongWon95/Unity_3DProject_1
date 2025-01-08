using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Charge Attack Action")]
public class ChargeAttackAction : WeaponItemAction
{
    [SerializeField] string Charge_Attack_01 = "Main_Charge_Attack_01";
    [SerializeField] string Charge_Attack_02 = "Main_Charge_Attack_02";
    public override void AttemptToPerformAction(PlayerManager PlayerPerformingAction, WeaponItem WeaponPerformingAction)
    {
        base.AttemptToPerformAction(PlayerPerformingAction, WeaponPerformingAction);

        if (!PlayerPerformingAction.IsOwner)
            return;


        if (PlayerPerformingAction.mPlayerNetworkManager.mNetworkCurrentStamina.Value <= 0)
            return;


        if (!PlayerPerformingAction.mCharacterLocomotionManager.IsGround)
            return;

        PerformChargeAttack(PlayerPerformingAction, WeaponPerformingAction);
    }

    private void PerformChargeAttack(PlayerManager PlayerPerformingAction, WeaponItem WeaponPerformingAction)
    {
        if (PlayerPerformingAction.mPlayerCombatManager.mCanComboWithMainHandWeapon && PlayerPerformingAction.IsPerformingAction)
        {
            PlayerPerformingAction.mPlayerCombatManager.mCanComboWithMainHandWeapon = false;

            if (PlayerPerformingAction.mCharacterCombatManager.mLastAttackAnimationPerformed == Charge_Attack_01)
            {
                PlayerPerformingAction.mPlayerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.ChargeAttack02, Charge_Attack_02, true);
            }
            else
            {
                PlayerPerformingAction.mPlayerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.ChargeAttack01, Charge_Attack_01, true);
            }

        }
        else if (!PlayerPerformingAction.IsPerformingAction)
        {
            PlayerPerformingAction.mPlayerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.ChargeAttack01, Charge_Attack_01, true);
        }

    }
}
