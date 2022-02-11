using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenAnimation : MonoBehaviour
{
    public TweenAnimationData tweenData;
    
    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.localPosition;
    }

    IEnumerator Animate(Vector3 pos, string axisToAnim, float delayTime, float animTime)
    {
        switch (axisToAnim)
        {
            case "x":
                yield return new WaitForSeconds(delayTime);
                LeanTween.moveLocalX(gameObject, pos.x, animTime).setEase(tweenData.easeType);
                break;
            case "y":
                yield return new WaitForSeconds(delayTime);
                LeanTween.moveLocalY(gameObject, pos.y, animTime).setEase(tweenData.easeType);
                break;
            case "xy":
                yield return new WaitForSeconds(delayTime);
                LeanTween.moveLocalX(gameObject, pos.x, animTime).setEase(tweenData.easeType);
                LeanTween.moveLocalY(gameObject, pos.y, animTime).setEase(tweenData.easeType);
                break;
        }
    }
    
    IEnumerator AnimateToAndBack(Vector3 pos, string axisToAnim, float delayTimeStart, float delayTimeEnd, float delayTimeBetween, float animTime)
    {
        StartCoroutine(Animate(tweenData.endPos, axisToAnim, tweenData.delayTimeToEnd, tweenData.animTimeToEnd));
        yield return new WaitForSeconds(delayTimeBetween);
        StartCoroutine(Animate(startPos, axisToAnim, tweenData.delayTimeToStart, tweenData.animTimeToStart));
    }
    
    public void AnimateToStart(string axisToAnim)
    {
        StartCoroutine(Animate(startPos, axisToAnim, tweenData.delayTimeToStart, tweenData.animTimeToStart));
    }
    
    public void AnimateToEnd(string axisToAnim)
    {
        StartCoroutine(Animate(tweenData.endPos, axisToAnim, tweenData.delayTimeToEnd, tweenData.animTimeToEnd));
    }

    public void AnimateToEndToStart(string axisToAnim)
    {
        StartCoroutine(AnimateToAndBack(
            tweenData.endPos, axisToAnim, tweenData.delayTimeToStart, tweenData.delayTimeToEnd, 0.3f, tweenData.animTimeToEnd));

    }
    
    [ContextMenu("Add values to manager")]
    public void AddValuesToManager()
    {
        TweenAnimationValueManager tweenAnimationValueManager;
        tweenAnimationValueManager = FindObjectOfType<TweenAnimationValueManager>();

        if (tweenAnimationValueManager == null)
        {
            Debug.LogError("There is no TweenAnimationValueManager script");
            return;
        }
        
        tweenAnimationValueManager.StoreDataInList(tweenData);
    }
}
