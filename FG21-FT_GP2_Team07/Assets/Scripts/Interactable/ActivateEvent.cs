using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateEvent : MonoBehaviour, IInteractable
{
    public UnityEvent eventToBeTriggered;
    public void Activate()
    {
        eventToBeTriggered.Invoke();
    }
}
