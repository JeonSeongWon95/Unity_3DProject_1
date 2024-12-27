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

    public void PlaySoundFX(AudioClip soundFX, float Volume = 1, bool randomizePitch = true, float pitchrandom = 0.1f) 
    {
        mAudioSource.PlayOneShot(soundFX, Volume);
        mAudioSource.pitch = 1;

        if (randomizePitch) 
        {
            mAudioSource.pitch += Random.Range(-pitchrandom, pitchrandom);
        }
    }

    public void PlayRollSoundFX() 
    {
        mAudioSource.PlayOneShot(WorldSoundFXManager.Instance.mRollSFX);
    }
}
