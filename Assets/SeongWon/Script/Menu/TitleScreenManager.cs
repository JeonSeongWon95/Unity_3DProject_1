using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject mTitleScreenMainMenu;
    [SerializeField] GameObject mTitleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button mLoadMenuReturnButton;
    [SerializeField] Button mMainMenuLoadGameButton;
    public void StartNetworkAsHost() 
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame() 
    {
        WorldSaveGameManager.Instance.CreateNewGame();
        StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
    }

    public void OpenLoadGameMenu() 
    {
        mTitleScreenMainMenu.SetActive(false);
        mTitleScreenLoadMenu.SetActive(true);

        mLoadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu() 
    {
        mTitleScreenLoadMenu.SetActive(false);
        mTitleScreenMainMenu.SetActive(true);
        mMainMenuLoadGameButton.Select();
    }
}
