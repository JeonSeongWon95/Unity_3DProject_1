using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager mPlayerManamger;

    public float mVerticalMovement;
    public float mHorizontalMovement;

    private Vector3 mMoveDirection;
    private Vector3 mTargetLocation;

    [SerializeField] float mWalkSpeed = 2;
    [SerializeField] float mRunSpeed = 5;
    [SerializeField] float mRotateSpeed = 15;

    protected override void Awake()
    {
        base.Awake();
        mPlayerManamger = GetComponent<PlayerManager>();
    }
    public void HandleAllMovement() 
    {
        HandleGroundedMovement();
        HandleRotateMovement();
    }

    private void GetVerticalAndHorizontalInputs() 
    {
        mVerticalMovement = PlayerInputManager.Instance.mVerticalInput;
        mHorizontalMovement = PlayerInputManager.Instance.mHorizontalInput;
    }

    private void HandleGroundedMovement() 
    {
        GetVerticalAndHorizontalInputs();

        mMoveDirection = PlayerCamera.Instance.transform.forward * mVerticalMovement;
        mMoveDirection = mMoveDirection + PlayerCamera.Instance.transform.right * mHorizontalMovement;
        mMoveDirection.Normalize();
        mMoveDirection.y = 0;

        mPlayerManamger.mCharaterController.Move(mMoveDirection * mRunSpeed * Time.deltaTime);
    }

    private void HandleRotateMovement() 
    {
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

}
