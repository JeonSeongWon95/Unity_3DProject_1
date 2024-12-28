using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [Header("NETWORK JOIN")]
    [SerializeField] bool StartGameAsClient;
    public PlayerUIHUDManager mPlayerUIHUDManager;
    public PlayerUIPopUpManager mPlayerUIPopUpManager;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            HidenUI();
        }
        else 
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (StartGameAsClient) 
        {
            StartGameAsClient = false;
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }
    }

    public void HidenUI()
    {
        RectTransform rectTransform = mPlayerUIHUDManager.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
    }

    public void ShowUI()
    {
        RectTransform rectTransform = mPlayerUIHUDManager.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
    }


}
