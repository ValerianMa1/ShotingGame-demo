using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatesBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    [SerializeField] bool delayFill = true;
    [SerializeField] float fillDelay = 0.5f;
    [SerializeField] float fillSpeed = 1f;

    float currentFillAmount;
    float previousFillAmount;
    protected float targetFillAmount;
    float t;

    Canvas canvas;
    WaitForSeconds waitForDelayFill;
    Coroutine bufferedFillingCoroutine;

    private void Awake() {
        if(TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }


    private void OnDisable()
    {
        StopAllCoroutines();    
    }



    public virtual void Initialize(float currentValue,float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }


    public void UpdateStates(float currentValue,float maxValue)
    {
        targetFillAmount = currentValue / maxValue;
        if(bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }
        if(currentFillAmount > targetFillAmount)
        {
            //收到伤害时血量减少，front先减少，background延迟减少
            fillImageFront.fillAmount = targetFillAmount;
            bufferedFillingCoroutine =  StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }

        else if(currentFillAmount < targetFillAmount)
        {
            //吃到血包时血量增加，background先达到目标值，然后front延迟增加
            fillImageBack.fillAmount = targetFillAmount;
            bufferedFillingCoroutine =  StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }










    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if(delayFill)
        {
            yield return waitForDelayFill;
        }


        previousFillAmount = currentFillAmount;
        t = 0;
        while(t < 1f)
        {
            t +=Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(previousFillAmount,targetFillAmount,t);
            image.fillAmount = currentFillAmount;
            yield return null;
        }
    }
}
