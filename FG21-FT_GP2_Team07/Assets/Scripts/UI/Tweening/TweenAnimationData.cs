using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TweenAnimation Data", menuName = "ScriptableObjects/Ui/TweenAnimationData", order = 1)]

public class TweenAnimationData : ScriptableObject
{
    public float animTimeToStart;
    public  float animTimeToEnd;
    public float delayTimeToStart;
    public float delayTimeToEnd;
    public LeanTweenType easeType;
    public Vector3 endPos;
}
