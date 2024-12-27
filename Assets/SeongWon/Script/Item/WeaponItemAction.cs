using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
public class WeaponItemAction : ScriptableObject
{
    public int mActionID;

    public virtual void AttemptToPerformAction(PlayerManager PlayerPerformingAction, WeaponItem WeaponPerformingAction) 
    {
        if (PlayerPerformingAction.IsOwner)
        {
            PlayerPerformingAction.mPlayerNetworkManager.mCurrentWeaponBeingUsed.Value = WeaponPerformingAction.mItemID;
        }
    }
}
