using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private static LightManager instance = null;
    private static readonly object padlock = new object();
    
    public static LightManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<LightManager>();
                }
                return instance;
            }
        }
    }

    [SerializeField] List<RoomLightManagerStruct> roomsWithLight;

    public void TurnOffAllLamps(bool state)
    {
        foreach (RoomLightManagerStruct lightRoom  in roomsWithLight)
        {
            lightRoom.roomLightManager.SwitchOnOffSubscribedLamps(state);
        }
    }

}

[Serializable]
public class RoomLightManager
{
    public List<Lamp> subscribedLamps = new List<Lamp>();
    
    private int lampAmount;

    public int CheckActiveLamps()
    {
        int lampAmount = 0;
        foreach (Lamp lamp in subscribedLamps)
        {
            if (lamp.Active)
            {
                lampAmount++;
            }
        }
        return lampAmount;
    }

    public void SwitchOnOffSubscribedLamps(bool state)
    {
        foreach (Lamp lamp in subscribedLamps)
        {
            lamp.ActivateDisableLamp(state);
        }
    }
    
    public void SubscribeLamp(Lamp givenLamp)
    {
        subscribedLamps.Add(givenLamp);
    }
}

[Serializable]
public struct RoomLightManagerStruct
{
    public string name;
    public RoomLightManager roomLightManager;
}