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
    [SerializeField] bool IsWalk = false;
    public float mMovementAmount;
    public float mVerticalInput;
    public float mHorizontalInput;

    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 mCameraMovement;
    public float mCameraVerticalInput;
    public float mCameraHorizontalInput;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool IsDodge = false;


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
    }

    private void OnSceneChange(Scene OldScene, Scene NewScene) 
    {
        if (NewScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex()) 
        {
            Instance.enabled = true;
        }
        else 
        {
            Instance.enabled = false;
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
        else 
        {
            mPlayerManager.mPlayerLocomotionManager.ChangeSpeed(6);
        }

        if (mPlayerManager == null)
            return;

        mPlayerManager.mPlayerAnimatorManager.UpdateAnimatorValues(0, mMovementAmount);

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
}
