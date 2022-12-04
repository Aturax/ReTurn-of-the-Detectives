public abstract class State
{
    protected readonly GameSM _stateMachine;

    public State(GameSM stateMachine)
    {
        _stateMachine = stateMachine;
        LoadState();
    }

    public virtual void LoadState() { }
    public virtual void Enter() { }
    public virtual void Exit() { }
}