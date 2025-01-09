using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager Instance;

    [Header("Damage Sounds")]
    public AudioClip[] PhysicsDamageSFX;

    [Header("Action Sounds")]
    public AudioClip mRollSFX;

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

    private void Start()
    {
       
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array) 
    {
        int Index = Random.Range(0, array.Length - 1);
        return array[Index];
    }

}
