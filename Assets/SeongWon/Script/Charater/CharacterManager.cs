using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{

    [HideInInspector] public CharacterController mCharaterController;
    [HideInInspector] public Animator mAnimator;
    [HideInInspector] public CharaterNetworkManager mCharaterNetworkManager;

    [Header("FLAGS")]
    public bool IsPerformingAction = false;
    public bool ApplyRootMotion = false;
    public bool IsJumping = false;
    public bool IsGround = true;
    public bool CanRotate = true;
    public bool CanMove = true;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        mCharaterController = GetComponent<CharacterController>();
        mCharaterNetworkManager = GetComponent<CharaterNetworkManager>();
        mAnimator = GetComponent<Animator>();
    }

    protected virtual void Update() 
    {
        mAnimator.SetBool("IsGround", IsGround);

        if (IsOwner)
        {
            mCharaterNetworkManager.mNetworkPosition.Value = transform.position;
            mCharaterNetworkManager.mNetworkRotation.Value = transform.rotation;
        }
        else 
        {
            transform.position = Vector3.SmoothDamp(transform.position, mCharaterNetworkManager.mNetworkPosition.Value,
                ref mCharaterNetworkManager.mNetworkPositionVelocity, mCharaterNetworkManager.mNetworkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, mCharaterNetworkManager.mNetworkRotation.Value,
                mCharaterNetworkManager.mNetworkRotateSmoothTime);
        }
    }

    protected virtual void LateUpdate() 
    {

    }

}
