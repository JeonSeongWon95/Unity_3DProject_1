using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "A.I/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager mAiCharacter)
    {

        if (mAiCharacter.mCharacterCombatManager.mCurrentTarget != null)
        {
            return this;
        }
        else 
        {
            mAiCharacter.mAICharacterCombatManager.FindTargetOfSight(mAiCharacter);
            return this;
        }
    }
}
