using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager mPlayerManager;

    [HideInInspector] public float mVerticalMovement;
    [HideInInspector] public float mHorizontalMovement;
    [HideInInspector] public float mMovementAmount;

    [Header("MOVEMENT SETTINGS")]
    private Vector3 mMoveDirection;
    private Vector3 mTargetLocation;
    [SerializeField] float mSpeed = 6;
    [SerializeField] float mSpeedSprint = 8.0f;
    [SerializeField] float mRotateSpeed = 15;
    [SerializeField] int mSprintStaminaCost = 2;

    [Header("JUMP")]
    private float mJumpStaminaCost = 10;
    [SerializeField] float mJumpHeight = 2.0f;
    [SerializeField] Vector3 mJumpDirection;
    [SerializeField] float mJumpForwardVelocity = 3;
    [SerializeField] float mJumpFallVelocity = 1.5f;

    [Header("DODGE")]
    private Vector3 mRollDirection;
    private float mDodgeStaminaCost = 25;

    protected override void Awake()
    {
        base.Awake();
        mPlayerManager = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (mPlayerManager.IsOwner)
        {
            mPlayerManager.mCharacterNetworkManager.mNetworkAnimatorHorizontalParameter.Value = mHorizontalMovement;
            mPlayerManager.mCharacterNetworkManager.mNetworkAnimatorVerticalParameter.Value = mVerticalMovement;
            mPlayerManager.mCharacterNetworkManager.mNetworkAnimatorMoveAmountParameter.Value = mMovementAmount;
        }
        else
        {
            mHorizontalMovement = mPlayerManager.mCharacterNetworkManager.mNetworkAnimatorHorizontalParameter.Value;
            mVerticalMovement = mPlayerManager.mCharacterNetworkManager.mNetworkAnimatorVerticalParameter.Value;
            mMovementAmount = mPlayerManager.mCharacterNetworkManager.mNetworkAnimatorMoveAmountParameter.Value;

            if (!mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value || mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value)
            {
                mPlayerManager.mPlayerAnimatorManager.UpdateAnimatorValues(0, mMovementAmount, mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value);
            }
            else
            {

                mPlayerManager.mPlayerAnimatorManager.UpdateAnimatorValues(mHorizontalMovement, mVerticalMovement,
                    mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value);
            }
        }
    }
    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotateMovement();
        HandleJumpingMovement();
        HandleFreeFallMovement();
    }

    private void GetMovementInputValues()
    {
        mVerticalMovement = PlayerInputManager.Instance.mVerticalInput;
        mHorizontalMovement = PlayerInputManager.Instance.mHorizontalInput;
        mMovementAmount = PlayerInputManager.Instance.mMovementAmount;
    }

    private void HandleGroundedMovement()
    {

        if (!mPlayerManager.CanMove)
            return;

        GetMovementInputValues();

        mMoveDirection = PlayerCamera.Instance.transform.forward * mVerticalMovement;
        mMoveDirection = mMoveDirection + PlayerCamera.Instance.transform.right * mHorizontalMovement;
        mMoveDirection.Normalize();
        mMoveDirection.y = 0;

        if (mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value)
        {
            mPlayerManager.mCharaterController.Move(mMoveDirection * mSpeedSprint * Time.deltaTime);
        }
        else
        {
            mPlayerManager.mCharaterController.Move(mMoveDirection * mSpeed * Time.deltaTime);
        }
    }

    private void HandleJumpingMovement() 
    {
        if (mPlayerManager.mPlayerNetworkManager.mNetworkIsJumping.Value) 
        {
            mPlayerManager.mCharaterController.Move(mJumpDirection * mJumpForwardVelocity * Time.deltaTime);
        }
    }

    private void HandleFreeFallMovement() 
    {
        if (!mPlayerManager.IsGround) 
        {
            Vector3 mFreeFallDirection = PlayerCamera.Instance.transform.forward * PlayerInputManager.Instance.mVerticalInput;
            mFreeFallDirection += PlayerCamera.Instance.transform.right * PlayerInputManager.Instance.mHorizontalInput;
            mFreeFallDirection.y = 0;

            mPlayerManager.mCharaterController.Move(mFreeFallDirection * mJumpFallVelocity * Time.deltaTime);
        }
    }

    private void HandleRotateMovement()
    {
        if (mPlayerManager.mIsDead.Value)
            return;

        if (!mPlayerManager.CanRotate)
            return;

        if (mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value)
        {
            if (mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value || mPlayerManager.mPlayerLocomotionManager.mIsRolling)
            {
                Vector3 TargetDirection = Vector3.zero;
                TargetDirection = PlayerCamera.Instance.mCamera.transform.forward * mVerticalMovement;
                TargetDirection += PlayerCamera.Instance.mCamera.transform.right * mHorizontalMovement;
                TargetDirection.Normalize();
                TargetDirection.y = 0;

                if (TargetDirection == Vector3.zero)
                {
                    TargetDirection = transform.forward;
                }

                Quaternion TargetRotation = Quaternion.LookRotation(TargetDirection);
                Quaternion FinalRotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.deltaTime);
                transform.rotation = FinalRotation;
            }
            else 
            {
                if (mPlayerManager.mPlayerCombatManager.mCurrentTarget == null)
                    return;

                Vector3 TargetDirection;
                TargetDirection = mPlayerManager.mPlayerCombatManager.mCurrentTarget.transform.position - transform.position;
                TargetDirection.Normalize();
                TargetDirection.y = 0;

                Quaternion TargetRotation = Quaternion.LookRotation(TargetDirection);
                Quaternion FinalRotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.deltaTime);
                transform.rotation = FinalRotation;
            }
        }
        else 
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

    public void ChangeSpeed(float NewSpeed)
    {
        mSpeed = NewSpeed;
    }

    public void AttemptToPerfotmDodge()
    {
        if (mPlayerManager.IsPerformingAction)
            return;

        if (mPlayerManager.mPlayerNetworkManager.mNetworkCurrentStamina.Value <= 0)
            return;

        if (mMovementAmount > 0)
        {
            mRollDirection = PlayerCamera.Instance.mCamera.transform.forward * PlayerInputManager.Instance.mVerticalInput;
            mRollDirection += PlayerCamera.Instance.mCamera.transform.right * PlayerInputManager.Instance.mHorizontalInput;
            mRollDirection.y = 0;
            mRollDirection.Normalize();

            Quaternion RollRotation = Quaternion.LookRotation(mRollDirection);
            mPlayerManager.transform.rotation = RollRotation;

            mPlayerManager.mPlayerAnimatorManager.PlayTargetActionAnimation("RollForward", true, true);

            mPlayerManager.mPlayerLocomotionManager.mIsRolling = true;
        }

        mPlayerManager.mPlayerNetworkManager.mNetworkCurrentStamina.Value -= mDodgeStaminaCost;
    }

    public void AttemptToPerfotmJump() 
    {
        if (mPlayerManager.IsPerformingAction)
            return;

        if (mPlayerManager.mPlayerNetworkManager.mNetworkCurrentStamina.Value <= 0)
            return;

        if (mPlayerManager.mPlayerNetworkManager.mNetworkIsJumping.Value || !mPlayerManager.IsGround)
            return;

        mPlayerManager.mPlayerAnimatorManager.PlayTargetActionAnimation("Jump_Up", false);
        mPlayerManager.mPlayerNetworkManager.mNetworkIsJumping.Value = true;
        mPlayerManager.mPlayerNetworkManager.mNetworkCurrentStamina.Value -= mJumpStaminaCost;

        mJumpDirection = PlayerCamera.Instance.mCamera.transform.forward * PlayerInputManager.Instance.mVerticalInput;
        mJumpDirection += PlayerCamera.Instance.mCamera.transform.right * PlayerInputManager.Instance.mHorizontalInput;

        if (mJumpDirection != Vector3.zero)
        {
            if (mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value)
            {
                mJumpDirection *= 1;
            }
            else if (PlayerInputManager.Instance.mMovementAmount > 0.5)
            {
                mJumpDirection *= 0.5f;
            }
            else if (PlayerInputManager.Instance.mMovementAmount <= 0.5)
            {
                mJumpDirection *= 0.25f;
            }
        }
    }

    public void HandleSprinting() 
    {
        if (mPlayerManager.IsPerformingAction) 
        {
            mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value = false;
        }

        if (mPlayerManager.mPlayerNetworkManager.mNetworkCurrentStamina.Value <= 0)
        {
            mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value = false;
            return;
        }

        if (mMovementAmount >= 0.5f)
        {
            mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value = true;
        }
        else 
        {
            mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value = false;
        }

        if (mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value) 
        {
            mPlayerManager.mPlayerNetworkManager.mNetworkCurrentStamina.Value -= mSprintStaminaCost * Time.deltaTime;
        }
    }

    public void ApplyJumpingVelocity() 
    {
        mYVelocity.y = Mathf.Sqrt(mJumpHeight * -2 * mGravityForce);
    }
}
