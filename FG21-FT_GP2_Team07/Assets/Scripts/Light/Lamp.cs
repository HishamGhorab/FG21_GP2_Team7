using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Lamp : MonoBehaviour
{
    public bool Active { get => active; set => active = value; }

    [SerializeField] private bool active;
    private Light myLightSource;
    private bool storedState;

    private  void Awake()
    {
        myLightSource = GetComponent<Light>();
    }
    
    public void ActivateDisableLamp(bool activeState)
    {
        if (activeState)
        {
            myLightSource.enabled = true;
        }
        else
        {
            myLightSource.enabled = false;
        }
        
        
    }
}
