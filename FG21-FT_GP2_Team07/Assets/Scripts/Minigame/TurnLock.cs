using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnLock : MonoBehaviour
{
    private int rotations;
    [HideInInspector] public bool unlocked;
    private SoundComponent soundComponent;

    private void Start()
    {
        soundComponent = gameObject.transform.parent.GetComponent<SoundComponent>();
    }

    public void Turn()
    {
        if (rotations <= 0)
        {
            unlocked = true;
            return;
        }
        
        rotations--;
        soundComponent.PlaySound("Turn");
        transform.Rotate(new Vector3(0, 0, (360f / 7f)));
        
        GetComponentInChildren<Text>().text = rotations.ToString();
        if (rotations <= 0)
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }
    }
    
    public void SetRotation()
    {
        unlocked = false;
        int currentHeat = Int32.Parse(GetComponentInChildren<Text>().text);
        if (rotations != currentHeat)
        {
            int temp = rotations - currentHeat > 0 ? 1 : -1;
            
            for (int i = 0; i < Math.Abs(rotations - currentHeat); i++)
            {
                transform.Rotate(new Vector3(0, 0, (360f / 7f) * temp));
            }
        }

        rotations = currentHeat;
    }
}
