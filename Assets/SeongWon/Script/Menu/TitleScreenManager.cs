using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager Instance;
    
    [Header("Menus")]
    [SerializeField] GameObject mTitleScreenMainMenu;
    [SerializeField] GameObject mTitleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button mMenuNewGameButton;
    [SerializeField] Button mLoadMenuReturnButton;
    [SerializeField] Button mMainMenuLoadGameButton;
    [SerializeField] Button mNoCharacterSlotOkayButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject mNoCharacterSlotsPopUp;
    [SerializeField] GameObject mDeleteSlotsPopUp;

    [Header("Character Slots")]
    public CharacterSlot mCurrentSelectedSlot = CharacterSlot.END;

    private bool mIsDeleteMode = false;

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
    }
    public void StartNetworkAsHost() 
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame() 
    {
        WorldSaveGameManager.Instance.AttempToCreateNewGame();
    }

    public void OpenLoadGameMenu() 
    {
        mTitleScreenMainMenu.SetActive(false);
        mTitleScreenLoadMenu.SetActive(true);
    }

    public void CloseLoadGameMenu() 
    {
        mTitleScreenLoadMenu.SetActive(false);
        mTitleScreenMainMenu.SetActive(true);
    }

    public void DisplayNoFreeCharacterSlotPopUp()
    {
        mNoCharacterSlotsPopUp.SetActive(true);
    }

    public void CloseNoCharacterSlotPopUp() 
    {
        mNoCharacterSlotsPopUp.SetActive(false);
    }

    public void SelectCharacterSlot(CharacterSlot NewCharacterSlot) 
    {
        mCurrentSelectedSlot = NewCharacterSlot;
    }

    public void SelectNoSlot() 
    {
        mCurrentSelectedSlot = CharacterSlot.END;
    }

    public bool IsDeleteMode() 
    {
        return mIsDeleteMode;
    }

    public void SetIsDeleteMode(bool NewDeleteMode) 
    {
        mIsDeleteMode = NewDeleteMode;
    }

    public void DisplayDeleteSaveSlotPopUp() 
    {
        mDeleteSlotsPopUp.SetActive(true);
    }

    public void CloseDeleteSaveSlotPopUp() 
    {
        mDeleteSlotsPopUp.SetActive(false);
        mIsDeleteMode = false;
    }

}
