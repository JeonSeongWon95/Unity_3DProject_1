using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager mPlayerManamger;

    [HideInInspector] public float mVerticalMovement;
    [HideInInspector] public float mHorizontalMovement;
    [HideInInspector] public float mMovementAmount;

    [Header("MOVEMENT SETTINGS")]
    private Vector3 mMoveDirection;
    private Vector3 mTargetLocation;
    [SerializeField] float mSpeed = 6;
    [SerializeField] float mRotateSpeed = 15;

    [Header("DODGE")]
    private Vector3 mRollDirection;

    protected override void Awake()
    {
        base.Awake();
        mPlayerManamger = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (mPlayerManamger.IsOwner)
        {
            mPlayerManamger.mCharaterNetworkManager.mNetworkAnimatorHorizontalParameter.Value = mHorizontalMovement;
            mPlayerManamger.mCharaterNetworkManager.mNetworkAnimatorVerticalParameter.Value = mVerticalMovement;
            mPlayerManamger.mCharaterNetworkManager.mNetworkAnimatorMoveAmountParameter.Value = mMovementAmount;
        }
        else
        {
            mHorizontalMovement = mPlayerManamger.mCharaterNetworkManager.mNetworkAnimatorHorizontalParameter.Value;
            mVerticalMovement = mPlayerManamger.mCharaterNetworkManager.mNetworkAnimatorVerticalParameter.Value;
            mMovementAmount = mPlayerManamger.mCharaterNetworkManager.mNetworkAnimatorMoveAmountParameter.Value;

            mPlayerManamger.mPlayerAnimatorManager.UpdateAnimatorValues(0, mMovementAmount);
        }
    }
    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotateMovement();
    }

    private void GetMovementInputValues()
    {
        mVerticalMovement = PlayerInputManager.Instance.mVerticalInput;
        mHorizontalMovement = PlayerInputManager.Instance.mHorizontalInput;
        mMovementAmount = PlayerInputManager.Instance.mMovementAmount;
    }

    private void HandleGroundedMovement()
    {

        if (!mPlayerManamger.CanMove)
            return;

        GetMovementInputValues();

        mMoveDirection = PlayerCamera.Instance.transform.forward * mVerticalMovement;
        mMoveDirection = mMoveDirection + PlayerCamera.Instance.transform.right * mHorizontalMovement;
        mMoveDirection.Normalize();
        mMoveDirection.y = 0;

        mPlayerManamger.mCharaterController.Move(mMoveDirection * mSpeed * Time.deltaTime);
    }

    private void HandleRotateMovement()
    {
        if (!mPlayerManamger.CanRotate)
            return;

        mTargetLocation = Vector3.zero;
        mTargetLocation = PlayerCamera.Instance.mCamera.transform.forward * mVerticalMovement;
        mTargetLocation = mTargetLocation + PlayerCamera.Instance.transform.right * mHorizontalMovement;
        mTargetLocation.Normalize();
        mTargetLocation.y = 0;

        if (mTargetLocation == Vector3.zero)
        {
            mTargetLocation = transform.forward;
        }

        Quaternion NewRotation = Quaternion.LookRotation(mTargetLocation);
        Quaternion TargetRotation = Quaternion.Slerp(transform.rotation, NewRotation, mRotateSpeed * Time.deltaTime);

        transform.rotation = TargetRotation;

    }

    public void ChangeSpeed(float NewSpeed)
    {
        mSpeed = NewSpeed;
    }

    public void AttemptToPerfotmDodge()
    {
        if (mPlayerManamger.IsPerformingAction)
            return;

        if (mMovementAmount > 0)
        {
            mRollDirection = PlayerCamera.Instance.mCamera.transform.forward * PlayerInputManager.Instance.mVerticalInput;
            mRollDirection += PlayerCamera.Instance.mCamera.transform.right * PlayerInputManager.Instance.mHorizontalInput;
            mRollDirection.y = 0;
            mRollDirection.Normalize();

            Quaternion RollRotation = Quaternion.LookRotation(mRollDirection);
            mPlayerManamger.transform.rotation = RollRotation;

            mPlayerManamger.mPlayerAnimatorManager.PlayTargetActionAnimation("RollForward", true, true);
        }
    }

}
