using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TweenAnimationValueManager : MonoBehaviour
{
    [SerializeField] private List<TweenAnimationData> tweenAnimationDataList = new List<TweenAnimationData>();
    public void StoreDataInList(TweenAnimationData thisData)
    {
        if (tweenAnimationDataList.Contains(thisData))
        {
            Debug.Log("You have already added this data to the list before.");
            return;
        }
        
        tweenAnimationDataList.Add(thisData);
    }
}
