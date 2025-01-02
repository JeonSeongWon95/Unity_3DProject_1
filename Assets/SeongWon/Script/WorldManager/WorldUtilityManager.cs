using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager Instance;

    [Header("Layers")]
    [SerializeField] LayerMask mCharacterLayers;
    [SerializeField] LayerMask mEnviroLayers;

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

        DontDestroyOnLoad(gameObject);
    }

    public LayerMask GetCharacterLayers() 
    {
        return mCharacterLayers;
    }

    public LayerMask GetEnviroLayers()
    {
        return mEnviroLayers;
    }

    public bool mCanDamageThisTarget(CharacterGroup AttackingCharacter, CharacterGroup TargetCharacter) 
    {
        if (AttackingCharacter == CharacterGroup.Team01)
        {
            switch (TargetCharacter)
            {
                case CharacterGroup.Team01: return false;
                case CharacterGroup.Team02: return true;
                default:
                    break;
            }
        }
        else if (AttackingCharacter == CharacterGroup.Team02) 
        {
            switch (TargetCharacter)
            {
                case CharacterGroup.Team01: return true;
                case CharacterGroup.Team02: return false;
                default:
                    break;
            }
        }

        return false;
    }

}
