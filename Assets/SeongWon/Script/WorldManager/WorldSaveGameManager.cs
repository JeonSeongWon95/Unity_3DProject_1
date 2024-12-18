using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;
    public PlayerManager mPlayerManager;

    [Header("Save/Load")]
    [SerializeField] bool IsSave;
    [SerializeField] bool IsLoad;

    [Header("World Scene Index")]
    [SerializeField] int mWorldSceneIndex = 1;

    [Header("Save File Data Wirter")]
    SaveFileDataWriter mSaveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot mCurrentCharacterSlot;
    public CharacterSaveData mCurrentCharacterData;
    public string mFileName;

    [Header("Character Slots")]
    public CharacterSaveData[] mCharacterSlots = new CharacterSaveData[10];


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
        LoadAllCharacterProfiles();
    }

    private void Update()
    {
        if (IsSave) 
        {
            IsSave = false;
            SaveGame();
        }

        if (IsLoad) 
        {
            IsLoad = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBaseOnCurrentCharacterSlot(CharacterSlot NewSlot) 
    {
        string fileName = "";

        switch(NewSlot)
        {
            case CharacterSlot.CharacterSlot_00:
                fileName = "CharacterSlot_00";
                break;
            case CharacterSlot.CharacterSlot_01:
                fileName = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "CharacterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "CharacterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "CharacterSlot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "CharacterSlot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "CharacterSlot_09";
                break;
            default:
                break;

        }

        return fileName;

    }

    public void AttempToCreateNewGame() 
    {
        mSaveFileDataWriter = new SaveFileDataWriter();
        mSaveFileDataWriter.mSaveDataDirectoryPath = Application.persistentDataPath;

        for (int i = 0; i < (int)CharacterSlot.END - 1; ++i)
        {
            mSaveFileDataWriter.mSaveFileName = DecideCharacterFileNameBaseOnCurrentCharacterSlot((CharacterSlot)i);

            if (!mSaveFileDataWriter.CheckToSeeIfFileExists())
            {
                mCurrentCharacterSlot = (CharacterSlot)i;
                mCurrentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
        }

        TitleScreenManager.Instance.DisplayNoFreeCharacterSlotPopUp();
    }

    private void NewGame() 
    {
        mPlayerManager.mPlayerNetworkManager.mNetworkVitality.Value = 10;
        mPlayerManager.mPlayerNetworkManager.mNetworkEndurence.Value = 10;
        SaveGame();
        StartCoroutine(LoadWorldScene());
    }

    public void LoadGame() 
    {
        mFileName = DecideCharacterFileNameBaseOnCurrentCharacterSlot(mCurrentCharacterSlot);
        mSaveFileDataWriter = new SaveFileDataWriter();
        mSaveFileDataWriter.mSaveDataDirectoryPath = Application.persistentDataPath;
        mSaveFileDataWriter.mSaveFileName = mFileName;

        mCurrentCharacterData = mSaveFileDataWriter.LoadSaveFile();
        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame() 
    {
        mFileName = DecideCharacterFileNameBaseOnCurrentCharacterSlot(mCurrentCharacterSlot);
        mSaveFileDataWriter = new SaveFileDataWriter();
        mSaveFileDataWriter.mSaveDataDirectoryPath = Application.persistentDataPath;
        mSaveFileDataWriter.mSaveFileName = mFileName;

        mPlayerManager.SaveGameDataToCurrentCharacterData(ref mCurrentCharacterData);

        mSaveFileDataWriter.CreateSaveFile(mCurrentCharacterData);
    }

    private void LoadAllCharacterProfiles() 
    {
        mSaveFileDataWriter = new SaveFileDataWriter();
        mSaveFileDataWriter.mSaveDataDirectoryPath = Application.persistentDataPath;

        for (int i = 0; i < 10; ++i) 
        {
            mSaveFileDataWriter.mSaveFileName = DecideCharacterFileNameBaseOnCurrentCharacterSlot((CharacterSlot)i);
            mCharacterSlots[i] = mSaveFileDataWriter.LoadSaveFile();
        }
    }

    public IEnumerator LoadWorldScene() 
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(mWorldSceneIndex);
        mPlayerManager.LoadGameDataToCurrentCharacterData(ref mCurrentCharacterData);

        yield return null;
    }

    public int GetWorldSceneIndex() 
    {
        return mWorldSceneIndex;
    }

    public void DeleteSaveSlot() 
    {
        mFileName = DecideCharacterFileNameBaseOnCurrentCharacterSlot(mCurrentCharacterSlot);
        mSaveFileDataWriter = new SaveFileDataWriter();
        mSaveFileDataWriter.mSaveDataDirectoryPath = Application.persistentDataPath;
        mSaveFileDataWriter.mSaveFileName = mFileName;
        mSaveFileDataWriter.DeleteSaveFile();

        TitleScreenManager.Instance.CloseLoadGameMenu();
        TitleScreenManager.Instance.OpenLoadGameMenu();
    }
}
