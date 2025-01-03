using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AICharacterManager mAiCharacter) 
    {
        return this;
    }

    protected virtual AIState SwitchState(AICharacterManager mAiCharacter, AIState NewState)
    {
        ResetStateFlags(mAiCharacter);
        return NewState;
    }

    protected virtual void ResetStateFlags(AICharacterManager mAiCharacter) 
    {

    }
}
