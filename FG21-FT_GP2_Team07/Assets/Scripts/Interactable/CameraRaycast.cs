using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] private Canvas interactableIcon;
    [SerializeField] private float interactDistance = 3;

    private bool oldControllerInput;
    private bool buttonPressed;

    private NotificationTextUi notificationTextUi;

    private void Start()
    {
        notificationTextUi = NotificationTextUi.Singleton;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * interactDistance));
    }

    public void ControllerInput(InputAction.CallbackContext context)
    {
        
        bool input = context.ReadValueAsButton();
        if (oldControllerInput != input)
        {
            oldControllerInput = input;
            if (input)
            {
                buttonPressed = input;
            }
        }
    }

    private void CheckForInteraction()
    {
        if (Time.timeScale == 0)
            return;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            if (hit.transform.gameObject.tag == "interactable")
            {
                if(Hand.Singleton.handInPlayerImage.sprite == Hand.Singleton.handSprite)
                    Hand.Singleton.SetInteractHand();
                interactableIcon.enabled = true;

                if (hit.transform.gameObject.GetComponent<InteractionText>() != null)
                {
                    ShowNotificationText(hit.transform.gameObject.GetComponent<InteractionText>());
                }
                
                if (buttonPressed)
                {
                    hit.transform.gameObject.GetComponent<IInteractable>().Activate();
                    HideNotificationText();
                }
            }
        }
        else
        {
            if (Hand.Singleton.handInPlayerImage.sprite == Hand.Singleton.interactionHand)
                Hand.Singleton.SetHandSprite();
            interactableIcon.enabled = false;
            HideNotificationText();
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            buttonPressed = true;
        }
        
        CheckForInteraction();
        
        buttonPressed = false;
    }

    private void ShowNotificationText(InteractionText interactionText)
    {
        interactionText.SetTextAndShow();
    }    
    
    private void HideNotificationText()
    {
        notificationTextUi.HideUi();
    }
}
