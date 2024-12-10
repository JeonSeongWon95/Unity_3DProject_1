using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
public class TakeStaminaDamageEffect : InstantCharacterEffect
{
    public float mStaminaDamage;

    public override void ProcessEffect(CharacterManager NewCharacterManager)
    {
        CalculateStaminaDamage(NewCharacterManager);
    }

    private void CalculateStaminaDamage(CharacterManager NewCharacterManager) 
    {
        if (NewCharacterManager.IsOwner) 
        {
            NewCharacterManager.mCharacterNetworkManager.mNetworkCurrentStamina.Value -= mStaminaDamage;
        }
    }
}
