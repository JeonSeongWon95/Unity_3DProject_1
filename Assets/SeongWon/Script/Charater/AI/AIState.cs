using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AICharacterManager mAiCharacter) 
    {
        return this;
    }
}
