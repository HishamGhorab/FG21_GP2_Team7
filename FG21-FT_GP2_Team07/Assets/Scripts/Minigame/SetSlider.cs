using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSlider : MonoBehaviour
{
    private Slider slider;
    [HideInInspector] public bool unlocked;
    private SoundComponent soundComponent;

    private void Start()
    {
        slider = GetComponent<Slider>();
        soundComponent = gameObject.transform.parent.GetComponent<SoundComponent>();
    }

    public void ResetSlider()
    {
        unlocked = false;
        slider.value = 1;
    }

    private void Update()
    {
        if (slider.value < 0.1f && !unlocked)
        {
            unlocked = true;
            soundComponent.PlaySound("Slide");
        }
    }
}
