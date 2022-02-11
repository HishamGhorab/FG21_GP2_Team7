public class ChildState
{
    protected ChildBehaviour child;

    protected ChildState(ChildBehaviour child)
    {
        this.child = child;
    }
    public virtual void Enter(){}
    public virtual void Update(){}

    public virtual void Exit()
    {
        child.ChangeState();
    }
}
