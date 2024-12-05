using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Character_Save_Slot : MonoBehaviour
{
    SaveFileDataWriter mSaveFileDataWriter;

    [Header("Game Slot")]
    public CharacterSlot mCharacterSlot;

    [Header("Character Info")]
    public TextMeshProUGUI mCharacterName;
    public TextMeshProUGUI mCharacterTimePlayed;

    private void OnEnable()
    {
        LoadSaveSlot();
    }

    private void LoadSaveSlot()
    {
        mSaveFileDataWriter = new SaveFileDataWriter();
        mSaveFileDataWriter.mSaveDataDirectoryPath = Application.persistentDataPath;

        mSaveFileDataWriter.mSaveFileName =
            WorldSaveGameManager.Instance.DecideCharacterFileNameBaseOnCurrentCharacterSlot(mCharacterSlot);

        if (mSaveFileDataWriter.CheckToSeeIfFileExists())
        {
            mCharacterName.text = WorldSaveGameManager.Instance.mCharacterSlots[(int)mCharacterSlot].mCharacterName;
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
}
