using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("You Died Pop Up")]
    [SerializeField] GameObject mYouDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI mYouDiedPopUpBackGroundText;
    [SerializeField] TextMeshProUGUI mYouDiedPopUpText;
    [SerializeField] CanvasGroup mYouDiedPopUpCanvasGroup;

    public void SendYouDiedPopUp() 
    {
        mYouDiedPopUpGameObject.SetActive(true);
        mYouDiedPopUpBackGroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(mYouDiedPopUpBackGroundText, 8, 19f));
        StartCoroutine(FadeInPopUpOverTime(mYouDiedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(mYouDiedPopUpCanvasGroup, 2, 5));
    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float Duration,float stretchAmount) 
    {
        if (Duration > 0) 
        {
            text.characterSpacing = 0;
            float Timer = 0;

            yield return null;

            while (Duration < Timer) 
            {
                Timer += Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, Duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup Canvas, float Duration) 
    {
        if (Duration > 0) 
        {
            Canvas.alpha = 0;
            float Timer = 0;

            yield return null;

            while (Timer < Duration) 
            {
                Timer += Time.deltaTime;
                Canvas.alpha = Mathf.Lerp(Canvas.alpha, 1, Duration * Time.deltaTime);
                yield return null;
            }
        }

        Canvas.alpha = 1;
    }

    private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup Canvas, float Duration, float Delay) 
    {
        if (Duration > 0)
        {
            while (Delay > 0)
            {
                Delay -= Time.deltaTime;
                yield return null;
            }

            Canvas.alpha = 1;
            float Timer = 0;

            yield return null;

            while (Timer < Duration)
            {
                Timer += Time.deltaTime;
                Canvas.alpha = Mathf.Lerp(Canvas.alpha, 1, Duration * Time.deltaTime);
                yield return null;
            }
        }

        Canvas.alpha = 0;

        yield return null;

    }

}
