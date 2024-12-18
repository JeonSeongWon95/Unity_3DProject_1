using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    private Slider mSlider;
    private RectTransform mRectTransform;

    [Header("Bar Options")]
    [SerializeField] protected bool mScaleBarLengthWithStats = true;
    [SerializeField] protected float mWidthScaleMultiplier = 1;

    protected virtual void Awake()
    {
        mSlider = GetComponent<Slider>();
        mRectTransform = GetComponent<RectTransform>();
    }

    public void SetStats(int NewStat) 
    {
        mSlider.value = NewStat;
    }

    public void SetMaxStats(int NewMaxStat) 
    {
        mSlider.maxValue = NewMaxStat;
        mSlider.value = NewMaxStat;

        if (mScaleBarLengthWithStats) 
        {
            mRectTransform.sizeDelta = new Vector2(NewMaxStat * mWidthScaleMultiplier, mRectTransform.sizeDelta.y);
            PlayerUIManager.Instance.mPlayerUIHUDManager.RefeshHUD();
        }
    }
}
