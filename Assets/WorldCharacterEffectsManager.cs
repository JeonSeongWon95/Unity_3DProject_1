using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager Instance;

    [Header("Damage")]
    public TakeHealthDamageEffect mTakeHealthDamageEffect;

    [Header("VFX")]
    public GameObject BloodSplatterVFX;

    [SerializeField] List<InstantCharacterEffect> mInstantEffects;

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

        GenerateEffectIDs();
    }

    private void GenerateEffectIDs() 
    {
        for (int i = 0; i < mInstantEffects.Count; ++i) 
        {
            mInstantEffects[i].mInstantEffectID = i;
        }
    }


}
