using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Info")]
    public string mCharacterName;
    public float mPositionX;
    public float mPositionY;
    public float mPositionZ;

    [Header("Game Info")]
    public float mSecondsPlayed;
 
}
