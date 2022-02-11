using UnityEngine;

public sealed class RoamingState : ChildState
{
    public RoamingState(ChildBehaviour child) : base(child) {}
    
    public override void Enter()
    {
        // child.spriteHandler.UpdateSpriteSet(child.movingSprites);
        child.navMeshAgent.destination = child.transform.position + child.RandomPointOnUnitCircle() * child.maxRoamDistance;
    }

    public override void Update()
    {
        if (child.navMeshAgent.remainingDistance < Mathf.Epsilon)
        {
            Exit();
        }
    }
}