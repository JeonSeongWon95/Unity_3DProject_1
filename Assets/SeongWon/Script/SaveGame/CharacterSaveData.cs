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

    [Header("Resource")]
    public float mCurrentHealth;
    public float mCurrentStamina;


    [Header("Game Info")]
    public float mSecondsPlayed;
    public int mSceneIndex;
 
}
