using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionNotifText : MonoBehaviour
{
    private static InteractionNotifText _singleton;
    public static InteractionNotifText Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(InteractionNotifText)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    
    [SerializeField] private List<ActivateMinigame> minigameInteractables;
    //[SerializeField] private TextMeshProUGUI dangerHintText;
    
    private void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        //dangerHintText.text = "";
        foreach(GameObject interactable in GameObject.FindGameObjectsWithTag("interactable")) {
            if (interactable.GetComponent<ActivateMinigame>() != null)
            {
                minigameInteractables.Add(interactable.GetComponent<ActivateMinigame>());
            }
        }
    }

    public void UpdateUi()
    {
        int activeMinigames = 0;
        string text = "";
        foreach (ActivateMinigame minigame in minigameInteractables)
        {
            if (minigame.Active)
            {
                activeMinigames++;
                text = minigame.nameOfMinigameHolder;
            }
        }

        /*if (activeMinigames == 0)
        {
            dangerHintText.text = "YOU ARE SAFE.";
        }
        else
        {
            dangerHintText.text = $"DANGER AT {text.ToUpper()}!";
        }*/
        
        GetComponent<TextMeshProUGUI>().text = activeMinigames.ToString();
    }
}
