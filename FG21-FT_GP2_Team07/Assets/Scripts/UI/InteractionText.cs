using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionText : MonoBehaviour
{
    [TextArea(3, 3)] public string text;

    private NotificationTextUi notificationTextUi;

    private void Start()
    {
        notificationTextUi = NotificationTextUi.Singleton;
    }

    public void SetTextAndShow()
    {
        if (GetComponent<ActivateMinigame>() == null)
        {
            notificationTextUi.ShowUi(text);
        }
        else
        {
            if(GetComponent<ActivateMinigame>().Active)
                notificationTextUi.ShowUi(text);
        }
    }
}
