using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
   AudioSource mAudioSource;

    protected virtual void Awake() 
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX() 
    {
        mAudioSource.PlayOneShot(WorldSoundFXManager.Instance.mRollSFX);
    }
}
