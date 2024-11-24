using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    PlayerControl playercontrol;

    [SerializeField] Vector2 Movement;

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
        if (playercontrol == null)
        {
            playercontrol = new PlayerControl();
            playercontrol.PlayerMovement.Movement.performed += i => Movement = i.ReadValue<Vector2>();
        }
    }
}
