using UnityEngine;

public sealed class IdleState : ChildState
{
    private float idleTimer;

    public IdleState(ChildBehaviour child) : base(child)
    {
        idleTimer = child.idleTime;
    }
    public IdleState(ChildBehaviour child, float time) : base(child)
    {
        idleTimer = time;
    }

    public override void Enter()
    {
        // child.spriteHandler.UpdateSpriteSet(child.idleSprites);
        //child.navMeshAgent.destination = child.transform.position;
    }

    public override void Update()
    {
        if (child.navMeshAgent.remainingDistance < Mathf.Epsilon)
        {
            idleTimer -= Time.deltaTime;
        }

        if (idleTimer <= 0f)
        {
            Exit();
        }
    }
}