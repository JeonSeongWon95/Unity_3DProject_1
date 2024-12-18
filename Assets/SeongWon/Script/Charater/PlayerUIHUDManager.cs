using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHUDManager : MonoBehaviour
{
    [SerializeField] UI_StatBar mHealthBar;
    [SerializeField] UI_StatBar mStaminaBar;
    public void SetStaminaValue(float OldValue, float NewValue) 
    {
        mStaminaBar.SetStats(Mathf.RoundToInt(NewValue));
    }

    public void SetMaxStaminaValue(int MaxStamina) 
    {
        mStaminaBar.SetMaxStats(MaxStamina);
    }

    public void SetHealthValue(float OldValue, float NewValue)
    {
        mHealthBar.SetStats(Mathf.RoundToInt(NewValue));
    }

    public void SetMaxHealthValue(int MaxHealth)
    {
        mHealthBar.SetMaxStats(MaxHealth);
    }

    public void RefeshHUD() 
    {
        mHealthBar.gameObject.SetActive(false);
        mHealthBar.gameObject.SetActive(true);
        mStaminaBar.gameObject.SetActive(false);
        mStaminaBar.gameObject.SetActive(true);
    }
}
