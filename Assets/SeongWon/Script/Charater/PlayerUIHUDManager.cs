using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHUDManager : MonoBehaviour
{
    [SerializeField] UI_StatBar mStaminaBar;
    public void SetStaminaValue(float OldValue, float NewValue) 
    {
        mStaminaBar.SetStats(Mathf.RoundToInt(NewValue));
    }

    public void SetMaxStaminaValue(int MaxStamina) 
    {
        mStaminaBar.SetMaxStats(MaxStamina);
    }
}
