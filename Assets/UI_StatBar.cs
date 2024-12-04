using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    private Slider mSlider;

    protected virtual void Awake()
    {
        mSlider = GetComponent<Slider>();
    }

    public void SetStats(int NewStat) 
    {
        mSlider.value = NewStat;
    }

    public void SetMaxStats(int NewMaxStat) 
    {
        mSlider.maxValue = NewMaxStat;
        mSlider.value = NewMaxStat;
    }
}
