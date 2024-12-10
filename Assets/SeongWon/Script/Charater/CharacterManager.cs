using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{

    [HideInInspector] public CharacterController mCharaterController;
    [HideInInspector] public Animator mAnimator;
    [HideInInspector] public CharacterNetworkManager mCharacterNetworkManager;

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
        mCharacterNetworkManager = GetComponent<CharacterNetworkManager>();
        mAnimator = GetComponent<Animator>();
    }

    protected virtual void Update() 
    {
        mAnimator.SetBool("IsGround", IsGround);

        if (IsOwner)
        {
            mCharacterNetworkManager.mNetworkPosition.Value = transform.position;
            mCharacterNetworkManager.mNetworkRotation.Value = transform.rotation;
        }
        else 
        {
            transform.position = Vector3.SmoothDamp(transform.position, mCharacterNetworkManager.mNetworkPosition.Value,
                ref mCharacterNetworkManager.mNetworkPositionVelocity, mCharacterNetworkManager.mNetworkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, mCharacterNetworkManager.mNetworkRotation.Value,
                mCharacterNetworkManager.mNetworkRotateSmoothTime);
        }
    }

    protected virtual void LateUpdate() 
    {

    }

}
