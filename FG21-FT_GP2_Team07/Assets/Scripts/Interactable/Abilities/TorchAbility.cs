using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchAbility : Ability
{
    [Header("Specific Values")]
    [SerializeField] private float chaseChance;
    
    [Header("Light Settings")]
    [Range(1,10)]
    [SerializeField] private float lightIntensity;
    [Range(1,50)]
    [SerializeField] private float lightRange;
    
    [Header("Associated hand sprites")]
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private ChildBehaviour childBehaviour;
    private SoundComponent soundComponent;
    private Light lightSource;
    
    public override void Start()
    {
        base.Start();
        childBehaviour = GameObject.FindGameObjectWithTag("Child").GetComponent<ChildBehaviour>();
        soundComponent = GetComponent<SoundComponent>();
        
        lightSource = GameObject.FindGameObjectWithTag("Player").transform.GetComponentInChildren<Light>();
        lightSource.intensity = lightIntensity;
        lightSource.range = lightRange;
    }
    public override void Activate()
    {
        base.Activate();
        OnPlay();
    }

    public override void OnPlay()
    {
        base.OnPlay();
        
        hand.SetHandSprite(activeSprite, Hand.HandHoldingState.Ability);
        
        //todo: When the candle is on, the chances of the child going after the player is severely lowered (a multiplier, 0.2x).
        childBehaviour.SetChaseProbabilityMultiplier(chaseChance);
        soundComponent.PlayAudioSound("Burning Effect");

        lightSource.enabled = true;
    }

    public override void OnPause()
    {
        base.OnPause();
        
        hand.SetHandSprite(inactiveSprite, Hand.HandHoldingState.Ability);

        childBehaviour.SetChaseProbabilityMultiplier(1);
        soundComponent.StopAudioSource();
        
        lightSource.enabled = false;
    }

    public override void EndCondition()
    {
        base.EndCondition();
        childBehaviour.SetChaseProbabilityMultiplier(1);
        soundComponent.StopAudioSource();
        lightSource.enabled = false;
    }
}
