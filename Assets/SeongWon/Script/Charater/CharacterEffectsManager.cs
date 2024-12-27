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

    public void PlayBloodSplatterVFX(Vector3 ContactPoint) 
    {
        if (WorldCharacterEffectsManager.Instance.BloodSplatterVFX != null) 
        {
            GameObject BloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance.BloodSplatterVFX,
                ContactPoint, Quaternion.identity);
        }
    }
}
