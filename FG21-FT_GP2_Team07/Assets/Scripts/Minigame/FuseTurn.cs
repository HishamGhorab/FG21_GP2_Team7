using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FuseTurn : MonoBehaviour
{
    private int heat;
    private SoundComponent soundComponent;

    [SerializeField] private List<Sprite> buttons = new List<Sprite>();
    public Image colorIndicator;

    private void Start()
    {
        colorIndicator.color = Color.green;
        soundComponent = gameObject.transform.parent.parent.GetComponent<SoundComponent>();
    }

    public void Turn()
    {
        heat--;
        if (heat < 0)
        {
            heat = 1;
        }
        soundComponent.PlaySound("Flick switch");
        GetComponent<Image>().sprite = buttons[heat];

      GetComponentInChildren<Text>().text = heat.ToString();
    }
    
    
    
    public void SetRotation()
    {
        heat = Int32.Parse(GetComponentInChildren<Text>().text);
        GetComponent<Image>().sprite = buttons[heat];
    }
}