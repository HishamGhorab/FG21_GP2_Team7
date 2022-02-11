using UnityEngine;

public class MoveToInteractableState : ChildState
{
    public MoveToInteractableState(ChildBehaviour child) : base(child) {}

    public override void Enter()
    {
        if (child.inactiveInteractionPoints.Count > 0)
        {
            // child.spriteHandler.UpdateSpriteSet(child.movingSprites);
            child.currentInteractable = child.inactiveInteractionPoints[Random.Range(0, child.inactiveInteractionPoints.Count)];
            child.navMeshAgent.destination = child.currentInteractable.transform.position;
        }
        else
        {
            //Debug.Log("Not enough inactive interaction points!");
            child.ChangeState(new RoamingState(child));
        }
    }
    public override void Update()
    {
        if (child.navMeshAgent.remainingDistance < Mathf.Epsilon)
        {
            Exit();
        }
    }

    public override void Exit()
    {
        child.ChangeState(new InteractingState(child));
    }
}