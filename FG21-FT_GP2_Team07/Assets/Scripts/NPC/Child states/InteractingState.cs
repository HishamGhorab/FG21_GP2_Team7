using UnityEngine;

public class InteractingState : ChildState
{
    private float interactTimer;

    public InteractingState(ChildBehaviour child) : base(child)
    {
        interactTimer = child.interactTime;
    }
    
    public override void Enter()
    {
        //Debug.Log($"Starting interaction, distance to target: {Vector3.Distance(child.transform.position, child.navMeshAgent.destination)}, time left {interactTimer}");
        // child.spriteHandler.UpdateSpriteSet(child.idleSprites);
    }

    public override void Update()
    {
        if (child.navMeshAgent.remainingDistance < Mathf.Epsilon)
        {
            interactTimer -= Time.deltaTime;
        }
        
        if (interactTimer <= 0f)
        {
            //Debug.Log($"Activating interactable, distance to target: {Vector3.Distance(child.transform.position, child.navMeshAgent.destination)}, time left {interactTimer}");
            child.currentInteractable.BabyActivate();
            Exit();
        }
    }
}