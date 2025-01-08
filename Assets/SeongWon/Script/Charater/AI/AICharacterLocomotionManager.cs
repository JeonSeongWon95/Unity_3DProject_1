using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterLocomotionManager : CharacterLocomotionManager
{
    public void RotateTowardsAgent(AICharacterManager mAIcharacter) 
    {
        if (mAIcharacter.mAICharacterNetworkManager.mNetworkIsMoving.Value) 
        {
            mAIcharacter.transform.rotation = mAIcharacter.mNavMeshAgent.transform.rotation;
        }
    }
}
