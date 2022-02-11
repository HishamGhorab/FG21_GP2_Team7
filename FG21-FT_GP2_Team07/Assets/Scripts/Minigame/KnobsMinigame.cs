using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class KnobsMinigame : Minigame, IMinigame
{
    [SerializeField] private Button[] knobs = new Button[4];
    private int[] heat = new int[4];

    // to check if it's enabled or not
    private Canvas canvas;
    private bool canvasEnabled;
    
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        SetKnobs();
    }

    private void SetKnobs()
    {
        for (int i = 0; i < knobs.Length; i++)
        {
            heat[i] = Random.Range(1, 7);
            knobs[i].GetComponentInChildren<Text>().text = heat[i].ToString();
            knobs[i].GetComponent<TurnKnobs>().SetRotation();
        }
    }

    private void WiningCondition()
    {
        int turnedKnobs = 0;
        for (int i = 0; i < knobs.Length; i++)
        {
            if (heat[i] == 0)
            {
                turnedKnobs++;
            }
        }

        if (turnedKnobs == 4)
        {
            Finished();
            SetKnobs();
        }
    }

    private void HasCanvasBeenEnabled()
    {
        if (canvas.enabled && !canvasEnabled)
        {
            canvasEnabled = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(knobs[0].gameObject);
        }
        else if (!canvas.enabled)
        {
            canvasEnabled = false;
        }
    }
    
    void Update()
    {
        HasCanvasBeenEnabled();
        for (int i = 0; i < knobs.Length; i++)
        {
            heat[i] = Int32.Parse(knobs[i].GetComponentInChildren<Text>().text);
        }
        WiningCondition();
    }

    public void EastControllerButton()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<TurnKnobs>().RotateRight();
    }
}
