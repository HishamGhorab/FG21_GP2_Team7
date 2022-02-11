using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ActivateMinigame : MonoBehaviour, IInteractable
{
    public float PunishmentTimerMultiplyer { get => punishmentTimerMultiplyer; set => punishmentTimerMultiplyer = value; }
    public string nameOfMinigameHolder;

    [SerializeField] private Canvas minigame;
    [SerializeField] private GameObject effect;
    [SerializeField] private float punishmentTime = 10f;
    [SerializeField] private string gameOverExplanation;

    private float punishmentTimer = 0;
    private bool oldControllerInput;
    private float punishmentTimerMultiplyer = 1;

    private InteractionNotifText interactionNotifText;

    public bool Active = false;

    private void Start()
    {
        interactionNotifText = InteractionNotifText.Singleton;
    }
    public void Finished()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        punishmentTimerMultiplyer = 1;
        minigame.enabled = false;
        Active = false;
    }

    public void BabyActivate()
    {
        Active = true;
        punishmentTimer = punishmentTime;
        UpdateDangerText();

        if(nameOfMinigameHolder == "Fuse Box")
        {
            LightManager.Instance.TurnOffAllLamps(false);
        }
    }
    
    public void Activate()
    {
        if (!Active)
        {
            return;
        }
        
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        minigame.enabled = true;
    }

    public void EastControllerButton(InputAction.CallbackContext context)
    {
        print("reached");
        bool input = context.ReadValueAsButton();
        if (oldControllerInput != input)
        {
            oldControllerInput = input;
            if (input)
            {
                minigame.gameObject.GetComponent<IMinigame>().EastControllerButton();
            }
        }
    }

    private void Update()
    {
        if (Active)
        {
            effect.SetActive(true);
            punishmentTimer -= Time.deltaTime * punishmentTimerMultiplyer;
            
            if (punishmentTimer <= 0)
            {
                // ToDo some sort of punishment for not finish the task in time
                if(gameOverExplanation != "")
                {
                    Active = false;
                    GameOver.instance.SetExplanation(gameOverExplanation);
                }
                    
            }
        }
        else
        {
            UpdateDangerText();
            effect.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            minigame.enabled = false;
        }
    }

    private void UpdateDangerText()
    {
        interactionNotifText.UpdateUi();
    }
}
