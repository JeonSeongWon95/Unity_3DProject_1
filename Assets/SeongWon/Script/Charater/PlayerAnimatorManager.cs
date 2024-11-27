using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager mPlayerManager;
    protected override void Awake()
    {
        base.Awake();
        mPlayerManager = GetComponent<PlayerManager>();
    }
    private void OnAnimatorMove()
    {
        if (mPlayerManager.ApplyRootMotion)
        {
            Vector3 Velocity = mPlayerManager.mAnimator.deltaPosition;
            mPlayerManager.mCharaterController.Move(Velocity);
            mPlayerManager.transform.rotation *= mPlayerManager.mAnimator.deltaRotation;
        }
    }
}
