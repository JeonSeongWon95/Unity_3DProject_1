using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    public PlayerManager mPlayerManager;
    PlayerControl mPlayercontrol;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 mMovement;
    public float mMovementAmount;
    public float mVerticalInput;
    public float mHorizontalInput;

    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 mCameraMovement;
    public float mCameraVerticalInput;
    public float mCameraHorizontalInput;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool IsDodge = false;
    [SerializeField] bool IsWalk = false;
    [SerializeField] bool IsSprint = false;
    [SerializeField] bool IsJump = false;

    [Header("LOCK ON INPUT")]
    [SerializeField] bool LockOnInput = false;
    [SerializeField] Vector2 mSeekLeftRingtLockOnTarget;
    Coroutine mLockOnCoroutine;

    [Header("BUMPER INPUT")]
    [SerializeField] bool NomalAttack = false;
    [SerializeField] bool StrongAttack = false;

    [Header("TRIGGER INPUT")]
    [SerializeField] bool Hold_ChargeAttack = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += OnSceneChange;
        Instance.enabled = false;

        if (mPlayercontrol != null)
        {
            mPlayercontrol.Disable();
        }
    }

    private void OnSceneChange(Scene OldScene, Scene NewScene) 
    {
        if (NewScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex()) 
        {
            Instance.enabled = true;

            if (mPlayercontrol != null)
            {
                mPlayercontrol.Enable();
            }
        }
        else 
        {
            Instance.enabled = false;

            if (mPlayercontrol != null)
            {
                mPlayercontrol.Disable();
            }

        }

    }

    private void OnEnable()
    {

        if (mPlayercontrol == null)
        {
            mPlayercontrol = new PlayerControl();

            mPlayercontrol.PlayerMovement.Movement.performed += i =>
            {
                mMovement = i.ReadValue<Vector2>();
            };

            mPlayercontrol.CameraMovement.Movement.performed += i =>
            {
                mCameraMovement = i.ReadValue<Vector2>();
            };

            mPlayercontrol.PlayerActions.Walk.started += i =>
            {
                IsWalk = true;
            };

            mPlayercontrol.PlayerActions.Walk.canceled += i =>
            {
                IsWalk = false;
            };

            mPlayercontrol.PlayerActions.Dodge.performed += i =>
            {
                IsDodge = true;
            };

            mPlayercontrol.PlayerActions.Jump.performed += i =>
            {
                IsJump = true;
            };

            mPlayercontrol.PlayerActions.Sprint.performed += i =>
            {
                IsSprint = true;
            };
            mPlayercontrol.PlayerActions.Sprint.canceled += i =>
            {
                IsSprint = false;
            };

            mPlayercontrol.PlayerActions.NomalAttack.performed += i =>
            {
                NomalAttack = true;
            };
            mPlayercontrol.PlayerActions.LockOn.performed += i =>
            {
                LockOnInput = true;
            };
            mPlayercontrol.PlayerActions.SeekLeftAndRightLockOnTarget.performed += i =>
            {
                mSeekLeftRingtLockOnTarget = i.ReadValue<Vector2>();
            };

            mPlayercontrol.PlayerActions.StrongAttack.performed += i =>
            {
                StrongAttack = true;
            };

            mPlayercontrol.PlayerActions.ChargeAttack.performed += i =>
            {
                Hold_ChargeAttack = true;
            };

            mPlayercontrol.Enable();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (mPlayercontrol != null)
        {
            if (focus)
            {
                mPlayercontrol.Enable();
            }
            else
            {
                mPlayercontrol.Disable();
            }
        }
    }

    private void Update()
    {

        HandleAllInputs();
    }

    private void HandleAllInputs() 
    {
        HandleMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleJumpInput();
        HandleSprintInput();
        HandNomalAttackInput();
        HandleLockOnInput();
        HandleLockOnSwitchInput();
        HandleStrongAttackInput();
        HandleChargeAttackInput();
    }

    private void HandleLockOnSwitchInput() 
    {
        if (mSeekLeftRingtLockOnTarget.y == 0)
            return;

        else if (mSeekLeftRingtLockOnTarget.y > 0)
        {
            mSeekLeftRingtLockOnTarget.y = 0;

            if (mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value) 
            {
                PlayerCamera.Instance.HandleLocationgLockOnTargets();

                if (PlayerCamera.Instance.mRightLockOnTarget != null) 
                {
                    mPlayerManager.mPlayerCombatManager.SetTarget(PlayerCamera.Instance.mRightLockOnTarget);
                }
            }

        }
        else if (mSeekLeftRingtLockOnTarget.y < 0) 
        {
            mSeekLeftRingtLockOnTarget.y = 0;

            if (mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value)
            {
                PlayerCamera.Instance.HandleLocationgLockOnTargets();

                if (PlayerCamera.Instance.mLeftLockOnTarget != null)
                {
                    mPlayerManager.mPlayerCombatManager.SetTarget(PlayerCamera.Instance.mLeftLockOnTarget);
                }
            }

        }
 
    }

    private void HandleMovementInput() 
    {
        mVerticalInput = mMovement.y;
        mHorizontalInput = mMovement.x;

        mMovementAmount = Mathf.Clamp01(Mathf.Abs(mVerticalInput) + Mathf.Abs(mHorizontalInput));
            
        if (IsWalk)
        {
            mMovementAmount = 0.5f;
            mPlayerManager.mPlayerLocomotionManager.ChangeSpeed(3);
        }
        else if (IsSprint) 
        {
            mMovementAmount = 2.0f;
            mPlayerManager.mPlayerLocomotionManager.ChangeSpeed(9);
        }
        else
        {
            mPlayerManager.mPlayerLocomotionManager.ChangeSpeed(6);
        }

        if (mPlayerManager == null)
            return;

        if (mMovementAmount <= 0)
        {
            mMovementAmount = 0;
        }

        if (!mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value || mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value)
        {
            mPlayerManager.mPlayerAnimatorManager.UpdateAnimatorValues(0, mMovementAmount, mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value);
        }
        else
        {

            mPlayerManager.mPlayerAnimatorManager.UpdateAnimatorValues(mHorizontalInput, mVerticalInput,
                mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value);
        }

    }

    private void HandleCameraMovementInput()
    {
        mCameraMovement.Normalize();

        mCameraVerticalInput = mCameraMovement.x;
        mCameraHorizontalInput = mCameraMovement.y;
    }

    private void HandleDodgeInput() 
    {
        if (IsDodge) 
        {
            IsDodge = false;
            mPlayerManager.mPlayerLocomotionManager.AttemptToPerfotmDodge();
        }
    }

    private void HandleJumpInput()
    {
        if (IsJump)
        {
            IsJump = false;
            mPlayerManager.mPlayerLocomotionManager.AttemptToPerfotmJump();
        }
    }

    private void HandleSprintInput() 
    {
        if (IsSprint)
        {
            mPlayerManager.mPlayerLocomotionManager.HandleSprinting();
        }
        else 
        {
            mPlayerManager.mPlayerNetworkManager.mNetworkIsSprint.Value = false;
        }
    }

    private void HandNomalAttackInput() 
    {
        if (NomalAttack) 
        {
            NomalAttack = false;

            mPlayerManager.mPlayerNetworkManager.SetCharacterActionHand(true);

            mPlayerManager.mPlayerCombatManager.PerformWeaponBasedAction(
                mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon.OH_NomalAction,
                mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon);
        }
    }

    private void HandleLockOnInput() 
    {
        if (mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value) 
        {
            if (mPlayerManager.mPlayerCombatManager.mCurrentTarget == null)
                return;

            if (mPlayerManager.mPlayerCombatManager.mCurrentTarget.mIsDead.Value) 
            {
                mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value = false;
            }

            if (mLockOnCoroutine != null)
                StopCoroutine(mLockOnCoroutine);

            mLockOnCoroutine = StartCoroutine(PlayerCamera.Instance.WaitThenFindNewTarget());

        }

        if (LockOnInput && mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value) 
        {
            LockOnInput = false;
            mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value = false;
            PlayerCamera.Instance.ClearLockOnTargets();
            return;
        }

        if (LockOnInput && !mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value) 
        {
            LockOnInput = false;
            PlayerCamera.Instance.HandleLocationgLockOnTargets();

            if (PlayerCamera.Instance.mNearestLockOnTarget != null) 
            {
                mPlayerManager.mPlayerCombatManager.SetTarget(PlayerCamera.Instance.mNearestLockOnTarget);
                mPlayerManager.mPlayerNetworkManager.mNetworkIsLockOn.Value = true;
            }
        }
    }

    private void HandleStrongAttackInput()
    {
        if (StrongAttack)
        {
            StrongAttack = false;

            mPlayerManager.mPlayerNetworkManager.SetCharacterActionHand(true);

            mPlayerManager.mPlayerCombatManager.PerformWeaponBasedAction(
                mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon.OH_StrongAction,
                mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon);
        }
    }

    private void HandleChargeAttackInput()
    {
        if (Hold_ChargeAttack)
        {
            Hold_ChargeAttack = false;
            mPlayerManager.mPlayerNetworkManager.SetCharacterActionHand(true);

            mPlayerManager.mPlayerCombatManager.PerformWeaponBasedAction(
                mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon.OH_ChargeAction,
                mPlayerManager.mPlayerInventoryManager.mCurrentRightWeapon);

        }
    }
}
