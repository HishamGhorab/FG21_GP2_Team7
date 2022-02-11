using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeAbility : Ability
{
    [SerializeField] private List<GameObject> affectedInteractablesList;
    [SerializeField] private float increasedSpeedValue;
    
    private PlayerMovement playerMovement;
    private SoundComponent soundComponent;

    public override void Start()
    {
        base.Start();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        soundComponent = GetComponent<SoundComponent>();
    }

    public override void OnPlay()
    {
        playerMovement.SpeedModifier = increasedSpeedValue;
        DecreasePunishmentTimerForCurrentInteractable();
        soundComponent.PlaySound("Drink Effect");
    }
    
    public override void Activate()
    {
        base.Activate();
        OnPlay();
    }

    public override void EndCondition()
    {
        base.EndCondition();
        playerMovement.SpeedModifier = 0;
    }

    void DecreasePunishmentTimerForCurrentInteractable()
    {
        for (int i = 0; i < affectedInteractablesList.Count; i++)
        {
            if (affectedInteractablesList[i].GetComponent<ActivateMinigame>().Active)
            {
                affectedInteractablesList[i].GetComponent<ActivateMinigame>().PunishmentTimerMultiplyer = 0.5f;
            }
        }
    }
}
