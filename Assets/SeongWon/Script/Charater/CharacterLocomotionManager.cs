using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager mCharacterManager;

    [Header("Velocity")]
    [SerializeField] LayerMask mGroundLayer;
    [SerializeField] float mGravityForce = -5.55f;
    [SerializeField] float mGroundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 mYVelocity;
    [SerializeField] protected float mGroundYVelocity = -20;
    [SerializeField] protected float mFallStartYVelocity = -5;
    protected bool IsUsedFallVelocity = false;
    protected float mInAirTimer = 0;
    protected virtual void Awake() 
    {
        mCharacterManager = GetComponent<CharacterManager>();
    }

    protected virtual void Update() 
    {
        HandleGroundCheck();

        if (mCharacterManager.IsGround)
        {
            if (mYVelocity.y < 0)
            {
                mInAirTimer = 0;
                IsUsedFallVelocity = false;
                mYVelocity.y = mGroundYVelocity;
            }
        }
        else 
        {
            if (!mCharacterManager.IsJumping && !IsUsedFallVelocity) 
            {
                IsUsedFallVelocity = true;
                mYVelocity.y = mFallStartYVelocity;
            }

            mInAirTimer += Time.deltaTime;
            mCharacterManager.mAnimator.SetFloat("InAirTimer", mInAirTimer);

            mYVelocity.y += mGravityForce * Time.deltaTime;
        }

        mCharacterManager.mCharaterController.Move(mYVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck() 
    {
        mCharacterManager.IsGround = Physics.CheckSphere(mCharacterManager.transform.position,
            mGroundCheckSphereRadius, mGroundLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(mCharacterManager.transform.position, mGroundCheckSphereRadius);
    }
}
