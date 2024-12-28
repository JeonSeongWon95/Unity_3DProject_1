using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    public Camera mCamera;
    public PlayerManager mPlayerManager;
    [SerializeField] Transform mCameraPivot;

    [Header("Camera Settings")]
    private float mCameraSmoothSpeed = 1;
    [SerializeField] float mUpAndDownRotationSpeed = 100;
    [SerializeField] float mLeftAndRightRotationSpeed = 100;
    [SerializeField] float mMaximumPivot = 60;
    [SerializeField] float mMinimumPivot = -30;
    [SerializeField] float mCameraCollsionRadius = 0.2f;
    [SerializeField] LayerMask mCollisionLayer;
    [SerializeField] float mCameraSmoothTime = 0.2f;

    [Header("Camera Values")]
    private Vector3 mCameraVelocity;
    private Vector3 mCameraObjectPosition;
    [SerializeField] float mUpAndDownAngle;
    [SerializeField] float mLeftAndRightAngle;
    private float mCameraZPosition;
    private float mTargetZPosition;

    [Header("Lock On")]
    [SerializeField] float mLockOnRadius = 20;
    [SerializeField] float mMinimumViewableAngle = -50;
    [SerializeField] float mMaximumViewableAngle = 50;
    [SerializeField] float LockOnTargetFollowCameraSpeed = 0.2f;
    private List<CharacterManager> mAvailableTargets = new List<CharacterManager>();
    public CharacterManager mNearestLockOnTarget;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
        }
        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        mCameraZPosition = mCamera.transform.localPosition.z;
    }

    public void HandleAllCameraActions() 
    {
        if (mPlayerManager != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget() 
    {
        Vector3 TargetCameraPosition = Vector3.SmoothDamp(transform.position, mPlayerManager.transform.position,
            ref mCameraVelocity, mCameraSmoothSpeed * Time.deltaTime);
        transform.position = TargetCameraPosition;
    }

    private void HandleRotations() 
    {
        if (mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value)
        {
            Vector3 RotationDirection = mPlayerManager.mPlayerCombatManager.mCurrentTarget.mCharacterCombatManager.mLockOnTransform.position -
                transform.position;

            RotationDirection.Normalize();
            RotationDirection.y = 0;

            Quaternion TargetRotation = Quaternion.LookRotation(RotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, LockOnTargetFollowCameraSpeed);

            RotationDirection = mPlayerManager.mPlayerCombatManager.mCurrentTarget.mCharacterCombatManager.mLockOnTransform.position - mCameraPivot.position;
            RotationDirection.Normalize();

            TargetRotation = Quaternion.LookRotation(RotationDirection);
            mCameraPivot.transform.rotation = Quaternion.Slerp(mCameraPivot.rotation, TargetRotation, LockOnTargetFollowCameraSpeed);

            mLeftAndRightAngle = transform.eulerAngles.y;
            mUpAndDownAngle = transform.eulerAngles.x;

        }
        else
        {
            mLeftAndRightAngle += PlayerInputManager.Instance.mCameraVerticalInput *
                mLeftAndRightRotationSpeed * Time.deltaTime;
            mUpAndDownAngle -= PlayerInputManager.Instance.mCameraHorizontalInput *
                mUpAndDownRotationSpeed * Time.deltaTime;

            mUpAndDownAngle = Mathf.Clamp(mUpAndDownAngle, mMinimumPivot, mMaximumPivot);

            Vector3 CameraRotation = Vector3.zero;
            Quaternion TargetRotation;

            CameraRotation.y = mLeftAndRightAngle;
            TargetRotation = Quaternion.Euler(CameraRotation);
            transform.rotation = TargetRotation;

            CameraRotation = Vector3.zero;
            CameraRotation.x = mUpAndDownAngle;
            TargetRotation = Quaternion.Euler(CameraRotation);
            mCameraPivot.localRotation = TargetRotation;
        }
    }

    private void HandleCollisions()
    {
        mTargetZPosition = mCameraZPosition;

        RaycastHit Hit;

        Vector3 Direction = mCamera.transform.position - mCameraPivot.position;
        Direction.Normalize();

        if (Physics.SphereCast(mCameraPivot.transform.position, mCameraCollsionRadius, Direction, out Hit,
            Mathf.Abs(mTargetZPosition), mCollisionLayer)) 
        {
            float DistanceFromHitObject = Vector3.Distance(mCameraPivot.position, Hit.point);
            mTargetZPosition = -(DistanceFromHitObject - mCameraCollsionRadius);
        }

        if(Mathf.Abs(mTargetZPosition) < mCameraCollsionRadius)
        {
            mTargetZPosition = -mCameraCollsionRadius;
        }

        mCameraObjectPosition.z = Mathf.Lerp(mCamera.transform.localPosition.z, mTargetZPosition, mCameraSmoothTime);
        mCamera.transform.localPosition = mCameraObjectPosition;

    }

    public void HandleLocationgLockOnTargets() 
    {
        float shortDistance = Mathf.Infinity;
        float shortDistanceOfRightTarget = Mathf.Infinity;
        float shortDistanceOfLeftTarget = -Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(mPlayerManager.transform.position, mLockOnRadius,
            WorldUtilityManager.Instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager LockOnTarget = colliders[i].GetComponent<CharacterManager>();

            if (LockOnTarget != null)
            {
                Vector3 LockOnTargetDirection = LockOnTarget.transform.position - mPlayerManager.transform.position;
                float DistanceFromTarget = Vector3.Distance(mPlayerManager.transform.position, LockOnTarget.transform.position);
                float ViewableAngle = Vector3.Angle(LockOnTargetDirection, mCamera.transform.forward);

                if (LockOnTarget.mIsDead.Value)
                    continue;

                if (LockOnTarget.transform.root == mPlayerManager.transform.root)
                    continue;

                if (ViewableAngle > mMinimumViewableAngle && ViewableAngle < mMaximumViewableAngle) 
                {
                    RaycastHit Hit;

                    if (Physics.Linecast(mPlayerManager.mPlayerCombatManager.mLockOnTransform.position,
                        LockOnTarget.mCharacterCombatManager.mLockOnTransform.position, out Hit, WorldUtilityManager.Instance.GetEnviroLayers()))
                    {
                        continue;
                    }
                    else 
                    {
                        mAvailableTargets.Add(LockOnTarget);  
                    }
                }

                
            }
        }

        for (int j = 0; j < mAvailableTargets.Count; j++) 
        {
            if (mAvailableTargets[j] != null)
            {
                float DistanceFromTarget = Vector3.Distance(mPlayerManager.transform.position, mAvailableTargets[j].transform.position);

                if (DistanceFromTarget < shortDistance)
                {
                    shortDistance = DistanceFromTarget;
                    mNearestLockOnTarget = mAvailableTargets[j];
                }
            }
            else 
            {
                mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value = false;
            }

        } 
    }

    public void ClearLockOnTargets() 
    {
        mNearestLockOnTarget = null;
        mAvailableTargets.Clear();
    }

}
