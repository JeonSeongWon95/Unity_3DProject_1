using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterAnimatorManager : CharacterAnimatorManager
{
    AICharacterManager mAiCharacter;

    protected override void Awake()
    {
        base.Awake();
        mAiCharacter = GetComponent<AICharacterManager>();
    }

    private void OnAnimatorMove()
    {
        if (mAiCharacter.IsOwner)
        {
            if (!mAiCharacter.mAICharacterLocomotionManager.IsGround)
                return;

            Vector3 Velocity = mAiCharacter.mAnimator.deltaPosition;
            mAiCharacter.mCharaterController.Move(Velocity);
            mAiCharacter.transform.rotation *= mAiCharacter.mAnimator.deltaRotation;
        }
        else 
        {
            if (!mAiCharacter.mAICharacterLocomotionManager.IsGround)
                return;


            Vector3 Velocity = mAiCharacter.mAnimator.deltaPosition;
            mAiCharacter.mCharaterController.Move(Velocity);
            mAiCharacter.transform.position = Vector3.SmoothDamp(transform.position,
                mAiCharacter.mCharacterNetworkManager.mNetworkPosition.Value,
                ref mAiCharacter.mCharacterNetworkManager.mNetworkPositionVelocity,
                mAiCharacter.mCharacterNetworkManager.mNetworkPositionSmoothTime);

            mAiCharacter.transform.rotation *= mAiCharacter.mAnimator.deltaRotation;
        }
    }

}
