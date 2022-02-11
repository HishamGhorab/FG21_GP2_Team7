using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    //public Ability ability;
    public Sprite itemHandSprite;
    private Hand hand;

    public bool AllowPickup { get => allowPickup; set => allowPickup = value; }
    private bool allowPickup;
    
    void Start()
    {
        hand = Hand.Singleton;
        allowPickup = true;
    }
    public void Activate()
    {
        /*if (Hand.handHoldingState == Hand.HandHoldingState.Ability)
            return;*/
        if (hand.abilityItem.item == this || Hand.handHoldingState == Hand.HandHoldingState.Ability)
            return;
        
        PickUpItem();
    }

    void PickUpItem()
    {
        if (!allowPickup)
            return;

        ShowHideObject(false);
        hand.SetHandSprite(itemHandSprite, gameObject, Hand.HandHoldingState.Ability);
    }

    public void ShowHideObject(bool show)
    {
        if (show)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<Collider>().enabled = true;
        }
        else
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
