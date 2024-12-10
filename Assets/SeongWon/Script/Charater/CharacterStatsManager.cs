using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager mCharacterManager;

    [Header("Stamina Regeneration")]
    private float mStaminaRegenerationTimer = 0.0f;
    private float mStaminaRegenerationDelay = 2.0f;
    private float mStaminaRegenerationTickTimer = 0.0f;
    private float mStaminaRegenerationAmount = 2.0f;

    protected virtual void Awake() 
    {
        mCharacterManager = GetComponent<CharacterManager>();
    }

    public int CalculateStaminaBaseOnEndurance(int endurance)
    {
        float stamina = 0.0f;
        stamina = endurance * 10.0f;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        if (!mCharacterManager.IsOwner)
            return;

        if (mCharacterManager.mCharacterNetworkManager.mNetworkIsSprint.Value)
            return;

        if (mCharacterManager.IsPerformingAction)
            return;


        mStaminaRegenerationTimer += Time.deltaTime;

        if (mStaminaRegenerationTimer >= mStaminaRegenerationDelay)
        {
            if (mCharacterManager.mCharacterNetworkManager.mNetworkCurrentStamina.Value < 
                mCharacterManager.mCharacterNetworkManager.mNetworkMaxStamina.Value)
            {
                mStaminaRegenerationTickTimer += Time.deltaTime;

                if (mStaminaRegenerationTickTimer >= 0.1)
                {
                    mStaminaRegenerationTickTimer = 0;
                    mCharacterManager.mCharacterNetworkManager.mNetworkCurrentStamina.Value += mStaminaRegenerationAmount;
                }
            }
        }

    }

    public virtual void ResetRegenerateStamina(float PreviousStaminaAmount, float CurrentStaminaAmount) 
    {
        if (CurrentStaminaAmount < PreviousStaminaAmount)
        {

            mStaminaRegenerationTimer = 0;
        }
    }
}
