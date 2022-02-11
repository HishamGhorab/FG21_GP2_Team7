using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TurnKnobs : MonoBehaviour, IPointerClickHandler
{
    private int heat;
    private SoundComponent soundComponent;

    private void Start()
    {
        soundComponent = gameObject.transform.parent.parent.GetComponent<SoundComponent>();
    }

    public void Turn()
    {
        heat--;
        if (heat < 0)
        {
            heat = 6;
        }
        transform.Rotate(new Vector3(0, 0, (360f / 7f)));
        soundComponent.PlaySound("Turn knob");

        GetComponentInChildren<Text>().text = heat.ToString();
    }
    
    public void OnPointerClick (PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameObject);
            RotateRight();
        }
    }

    public void RotateRight()
    {
        heat++;
        if (heat > 6)
        {
            heat = 0;
        }
        transform.Rotate(new Vector3(0, 0, -(360f / 7f)));
        soundComponent.PlaySound("Turn knob");

        GetComponentInChildren<Text>().text = heat.ToString();
    }
    
    public void SetRotation()
    {
        int currentHeat = Int32.Parse(GetComponentInChildren<Text>().text);
        if (heat != currentHeat)
        {
            int temp = heat - currentHeat > 0 ? 1 : -1;
            
            for (int i = 0; i < Math.Abs(heat - currentHeat); i++)
            {
                //heat += temp;
                transform.Rotate(new Vector3(0, 0, (360f / 7f) * temp));
            }
        }

        heat = currentHeat;
    }
}
