using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Strong Attack Action")]
public class StrongAttackAction : WeaponItemAction
{
    [SerializeField] string Strong_Attack_01 = "Main_Strong_Attack_01";
    public override void AttemptToPerformAction(PlayerManager PlayerPerformingAction, WeaponItem WeaponPerformingAction)
    {
        base.AttemptToPerformAction(PlayerPerformingAction, WeaponPerformingAction);

        if (!PlayerPerformingAction.IsOwner)
            return;


        if (PlayerPerformingAction.mPlayerNetworkManager.mNetworkCurrentStamina.Value <= 0)
            return;


        if (!PlayerPerformingAction.IsGround)
            return;

        PerformStrongAttack(PlayerPerformingAction, WeaponPerformingAction);
    }

    private void PerformStrongAttack(PlayerManager PlayerPerformingAction, WeaponItem WeaponPerformingAction)
    {
        if (PlayerPerformingAction.mPlayerNetworkManager.mIsUsingRightHand.Value)
        {
            PlayerPerformingAction.mPlayerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.StrongAttack01, Strong_Attack_01, true);
        }
        if (PlayerPerformingAction.mPlayerNetworkManager.mIsUsingLeftHand.Value)
        {

        }
    }
}
