using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    PlayerControl mPlayercontrol;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 mMovement;
    public float mVerticalInput;
    public float mHorizontalInput;

    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 mCameraMovement;
    public float mCameraVerticalInput;
    public float mCameraHorizontalInput;




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
        HandleMovementInput();
        HandleCameraMovementInput();
    }

    private void HandleMovementInput() 
    {
        mVerticalInput = mMovement.y;
        mHorizontalInput = mMovement.x;
    }

    private void HandleCameraMovementInput()
    {
        mCameraMovement.Normalize();

        mCameraVerticalInput = mCameraMovement.x;
        mCameraHorizontalInput = mCameraMovement.y;
    }
}
