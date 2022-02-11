using UnityEngine;

public class ChasingState : ChildState
{
    private GameObject player;

    public ChasingState(ChildBehaviour child) : base(child) {}
    
    public override void Enter()
    {
        // child.spriteHandler.UpdateSpriteSet(child.movingSprites);
        player = GameObject.FindGameObjectWithTag("Player");
        child.navMeshAgent.destination = player.transform.position + child.RandomPointOnUnitCircle() * 2;
    }

    public override void Update()
    {
        if (child.navMeshAgent.remainingDistance < Mathf.Epsilon)
        {
            Exit();
        }
    }
}