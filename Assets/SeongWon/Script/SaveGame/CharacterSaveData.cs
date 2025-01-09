using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Info")]
    public string mCharacterName = "Character";
    public float mPositionX;
    public float mPositionY;
    public float mPositionZ;

    [Header("Character Stats")]
    public int mVitality;
    public int mEndurace;

    [Header("Bosses")]
    public SerializableDictionary<int, bool> mBossesAwakened;
    public SerializableDictionary<int, bool> mBossesDefeated;

    [Header("Resource")]
    public float mCurrentHealth;
    public float mCurrentStamina;

    [Header("Game Info")]
    public float mSecondsPlayed;
    public int mSceneIndex;


    public CharacterSaveData() 
    {
        mBossesAwakened = new SerializableDictionary<int, bool>();
        mBossesDefeated = new SerializableDictionary<int, bool>();
    }
 
}
