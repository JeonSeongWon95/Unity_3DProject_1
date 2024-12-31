using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackWeaponAction : WeaponItemAction
{
    [SerializeField] string Light_Attack_01 = "Main_Light_Attack_01";
    [SerializeField] string Light_Attack_02 = "Main_Light_Attack_02";
    public override void AttemptToPerformAction(PlayerManager PlayerPerformingAction, WeaponItem WeaponPerformingAction)
    {
        base.AttemptToPerformAction(PlayerPerformingAction, WeaponPerformingAction);

        if (!PlayerPerformingAction.IsOwner)
            return;


        if (PlayerPerformingAction.mPlayerNetworkManager.mNetworkCurrentStamina.Value <= 0)
            return;


        if (!PlayerPerformingAction.IsGround)
            return;

        PerformLightAttack(PlayerPerformingAction, WeaponPerformingAction);
    }

    private void PerformLightAttack(PlayerManager PlayerPerformingAction, WeaponItem WeaponPerformingAction) 
    {
        if (PlayerPerformingAction.mPlayerCombatManager.mCanComboWithMainHandWeapon && PlayerPerformingAction.IsPerformingAction)
        {
            PlayerPerformingAction.mPlayerCombatManager.mCanComboWithMainHandWeapon = false;

            if (PlayerPerformingAction.mCharacterCombatManager.mLastAttackAnimationPerformed == Light_Attack_01)
            {
                PlayerPerformingAction.mPlayerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02, Light_Attack_02, true);
            }
            else 
            {
                PlayerPerformingAction.mPlayerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, Light_Attack_01, true);
            }

        }
        else if(!PlayerPerformingAction.IsPerformingAction)
        {
            PlayerPerformingAction.mPlayerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, Light_Attack_01, true);
        }

    }
}
