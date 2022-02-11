using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DoorMinigame : Minigame
{
    [SerializeField] private GameObject sliderLock;
    [SerializeField] private Button[] turnableButtons = new Button[2];
    private int[] rotation = new int[2];

    // to check if it's enabled or not
    private Canvas canvas;
    private bool canvasEnabled;
    
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        SetRotation();
    }

    private void SetRotation()
    {
        sliderLock.GetComponent<SetSlider>().ResetSlider();
        for (int i = 0; i < turnableButtons.Length; i++)
        {
            rotation[i] = Random.Range(1, 4);
            turnableButtons[i].GetComponentInChildren<Text>().text = rotation[i].ToString();
            turnableButtons[i].GetComponent<TurnLock>().SetRotation();
        }
    }

    private void WiningCondition()
    {
        int unlocked = 0;
        for (int i = 0; i < turnableButtons.Length; i++)
        {
            if (turnableButtons[i].GetComponent<TurnLock>().unlocked)
            {
                unlocked++;
            }
        }

        if (sliderLock.GetComponent<SetSlider>().unlocked)
        {
            unlocked++;
        }

        if (unlocked == 3)
        {
            Finished();
            SetRotation();
        }
    }

    private void HasCanvasBeenEnabled()
    {
        if (canvas.enabled && !canvasEnabled)
        {
            canvasEnabled = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(turnableButtons[0].gameObject);
        }
        else if (!canvas.enabled)
        {
            canvasEnabled = false;
        }
    }
    
    void Update()
    {
        HasCanvasBeenEnabled();
        for (int i = 0; i < turnableButtons.Length; i++)
        {
            rotation[i] = Int32.Parse(turnableButtons[i].GetComponentInChildren<Text>().text);
        }
        WiningCondition();
    }
}
