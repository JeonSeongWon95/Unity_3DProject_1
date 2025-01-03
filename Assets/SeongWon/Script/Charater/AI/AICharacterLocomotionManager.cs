using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterLocomotionManager : CharacterLocomotionManager
{
    public void RotateTowardsAgent(AICharacterManager mAICharacter) 
    {
        if (mAICharacter.mAICharacterNetworkManager.mNetworkIsMoving.Value) 
        {
            mAICharacter.transform.rotation = mAICharacter.mNavMeshAgent.transform.rotation;
        }
    }
}
