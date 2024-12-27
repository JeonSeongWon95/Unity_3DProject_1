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

}
