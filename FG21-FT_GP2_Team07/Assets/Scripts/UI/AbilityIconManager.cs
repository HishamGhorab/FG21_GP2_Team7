using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AbilityIcon {CoffeeIcon, CandelIcon} 

public class AbilityIconManager : MonoBehaviour
{
    private static AbilityIconManager _singleton;
    public static AbilityIconManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(AbilityIconManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private TweenAnimation tweenAnimation;
    
    private void Awake()
    {
        Singleton = this;
        tweenAnimation = GetComponent<TweenAnimation>();
    }

    public void ShowUi(AbilityIcon abilityIcon)
    {
        tweenAnimation.AnimateToEnd("x");
        gameObject.transform.Find(abilityIcon.ToString()).gameObject.SetActive(true);
    }

    public void HideUi(AbilityIcon abilityIcon)
    {
        tweenAnimation.AnimateToStart("x");
        StartCoroutine(DelaySetActive(tweenAnimation.tweenData.animTimeToStart, abilityIcon));
    }

    IEnumerator DelaySetActive(float time, AbilityIcon abilityIcon)
    {
        yield return new WaitForSeconds(time);
        gameObject.transform.Find(abilityIcon.ToString()).gameObject.SetActive(false);
    }
    
}
