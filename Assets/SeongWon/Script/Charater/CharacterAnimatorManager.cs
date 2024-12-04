using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager mCharaterManager;

    int vertical;
    int horizontal;

    protected virtual void Awake()
    {
        mCharaterManager = GetComponent<CharacterManager>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");

    }

    public void UpdateAnimatorValues(float horizontalvalue, float verticalvalue, bool IsSprint) 
    {
        if (mCharaterManager.mAnimator != null)
        {
            float horizontalAmount = horizontalvalue;
            float verticalAmount = verticalvalue;

            if (IsSprint) 
            {
                verticalAmount = 2;
            }

            mCharaterManager.mAnimator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            mCharaterManager.mAnimator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }
    }

    public virtual void PlayTargetActionAnimation(string AnimationName, bool IsPerformingAction, bool IsRootMotion = true, 
        bool CanRotate = false, bool CanMove = false) 
    {
        mCharaterManager.ApplyRootMotion = IsRootMotion;
        mCharaterManager.mAnimator.CrossFade(AnimationName, 0.2f);
        mCharaterManager.IsPerformingAction = IsPerformingAction;
        mCharaterManager.CanRotate = CanRotate;
        mCharaterManager.CanMove = CanMove;

        mCharaterManager.mCharaterNetworkManager.PlayTargetActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId
            ,AnimationName, IsRootMotion);
    }
}
