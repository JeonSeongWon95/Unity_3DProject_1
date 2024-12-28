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

}
