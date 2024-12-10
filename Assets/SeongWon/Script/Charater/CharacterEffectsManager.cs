using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager mCharacterManager;

    private void Awake()
    {
        mCharacterManager = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect NewEffect) 
    {
        NewEffect.ProcessEffect(mCharacterManager);
    }
}
