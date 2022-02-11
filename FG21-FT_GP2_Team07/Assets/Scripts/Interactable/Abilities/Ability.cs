using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CountdownTimer))]
public class Ability : MonoBehaviour
{
    public AbilityIcon abilityIcon;
    
    [Header("General ability values")]
    [SerializeField] public bool toggle;
    [SerializeField] protected CountdownTimer afterUseCooldown;
    [SerializeField] protected int maxAbilityRespawns;
    
    [HideInInspector] public bool active;
    [HideInInspector] public Hand hand;
    [HideInInspector] public CountdownTimer myTimer;
    [SerializeField] protected int abilityRespawnValue;

    
    public virtual void Start()
    {
        active = false;
        hand = Hand.Singleton;
        myTimer = gameObject.GetComponent<CountdownTimer>();
    }

    public virtual void Update()
    {
        if (!active)
            return;
        
        UpdateLogic();
    }

    protected bool IsAbilityInHand()
    {
        if (hand.abilityItem.ability != this)
            return false;
        return true;
    }
    
    public virtual void Activate()
    {
        if (hand.abilityItem.ability != this)
            return;
        
        abilityRespawnValue++;
        if (maxAbilityRespawns != 0 && abilityRespawnValue >= maxAbilityRespawns)
        {
            afterUseCooldown.CanCallEndEvent = false;
        }
        
        EnableAbility();
        myTimer.Active = true;
        active = true;
    }

    public virtual void UpdateLogic()
    {
        if (myTimer.TimeValue <= 0.01f)
            EndCondition();

        //logic for the effect itself
    }

    public virtual void OnPlay()
    {
        
    }
    public virtual void OnPause()
    {
        
    }

    public virtual void EndCondition()
    {
        active = false;
        hand.RemoveAbilityFromHand();
        DisableAbility();
    }

    public virtual void DisableAbility()
    {
        this.enabled = false;
    }    
    
    public virtual void EnableAbility()
    {
        this.enabled = true;
    }
}


