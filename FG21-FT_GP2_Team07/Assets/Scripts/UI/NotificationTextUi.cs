using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationTextUi : MonoBehaviour
{
    private static NotificationTextUi _singleton;
    public static NotificationTextUi Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NotificationTextUi)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
        gameObject.SetActive(false);
    }

    public void ShowUi(string text)
    {
        GetComponent<TextMeshProUGUI>().text = text;
        
        if (gameObject.activeSelf)
            return;
        
        gameObject.SetActive(true);
    }    
    
    public void HideUi()
    {
        if (!gameObject.activeSelf)
            return;
        
        gameObject.SetActive(false);
    }
}
