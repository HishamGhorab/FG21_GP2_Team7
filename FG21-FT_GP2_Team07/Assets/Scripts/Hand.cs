using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    private static Hand _singleton;
    public static Hand Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(Hand)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }
    
    public AbilityIconManager abilityIconManager;
    public enum HandHoldingState{Empty, Ability}
    public static HandHoldingState handHoldingState;

    public Sprite handSprite;
    public Sprite inHandSprite;
    public Sprite interactionHand;
    //public GameObject itemObject;

    public AbilityItem abilityItem;

    public Image handInPlayerImage;

    void Start()
    {
        inHandSprite = handSprite;
        handInPlayerImage.sprite = inHandSprite;
        abilityIconManager = AbilityIconManager.Singleton;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && abilityItem.ability != null && !abilityItem.ability.active)
        {
            //handHoldingState = HandHoldingState.Ability;
            abilityItem.ability.Activate();
            abilityIconManager.ShowUi(abilityItem.ability.abilityIcon);
        }
        else if (Input.GetKeyDown(KeyCode.F) && abilityItem.ability != null && abilityItem.ability.toggle && abilityItem.ability.active)
        {
            if (abilityItem.ability.myTimer.TimerPaused)
            {
                abilityItem.ability.OnPlay();
                abilityItem.ability.myTimer.TimerPaused = false;
                abilityIconManager.ShowUi(abilityItem.ability.abilityIcon);
            }
            else
            {
                abilityItem.ability.OnPause();
                abilityItem.ability.myTimer.TimerPaused = true;
                abilityIconManager.HideUi(abilityItem.ability.abilityIcon);
            }
        }
    }

    public void SetHandSprite(Sprite itemHandSprite, GameObject itemObj, HandHoldingState state)
    {
        inHandSprite = itemHandSprite;
        handInPlayerImage.sprite = inHandSprite;
        //itemObject = itemObj;
        abilityItem.ability = itemObj.GetComponent<Ability>();
        abilityItem.item = itemObj.GetComponent<Item>();
        
        handHoldingState = state;
    }
    
    public void SetHandSprite(Sprite itemHandSprite, HandHoldingState state)
    {
        inHandSprite = itemHandSprite;
        handInPlayerImage.sprite = itemHandSprite;
        handHoldingState = state;
    }
    public void SetHandSprite()
    {
        inHandSprite = handSprite;
        handInPlayerImage.sprite = handSprite;
        handHoldingState = HandHoldingState.Empty;
    }

    public void SetInteractHand()
    {
        inHandSprite = interactionHand;
        handInPlayerImage.sprite = inHandSprite;
        handHoldingState = HandHoldingState.Empty;
    }
    
    public void RemoveAbilityFromHand()
    {
        abilityIconManager.HideUi(abilityItem.ability.abilityIcon);
        
        abilityItem.item = null;
        abilityItem.ability = null;
        SetHandSprite();
    }


    public void ActivateAbility()
    {
        if (handHoldingState != HandHoldingState.Ability)
            return;
        
        abilityItem.ability.Activate();
        //itemObject.GetComponent<Ability>().Activate();
    }
}

[System.Serializable] public struct AbilityItem
{
    public Item item;
    public Ability ability;
}
