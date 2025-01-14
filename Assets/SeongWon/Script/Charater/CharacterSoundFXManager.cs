using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
   AudioSource mAudioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] mDamageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] mAttackGrunts;

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

    public virtual void PlayDamageGrunt() 
    {
        if(mDamageGrunts.Length > 0) 
        PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(mDamageGrunts));
    }

    public virtual void PlayAttackGrunt() 
    {
        if(mAttackGrunts.Length > 0)
        PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(mAttackGrunts));
    }
}
