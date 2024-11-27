using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager mCharaterManager;

    protected virtual void Awake()
    {
        mCharaterManager = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorValues(float horizontalvalue, float verticalvalue) 
    {
        if (mCharaterManager.mAnimator != null)
        {
            mCharaterManager.mAnimator.SetFloat("Horizontal", horizontalvalue, 0.1f, Time.deltaTime);
            mCharaterManager.mAnimator.SetFloat("Vertical", verticalvalue, 0.1f, Time.deltaTime);
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
