public abstract class StateMachine
{
    private State currentState = null;
    
    public void StartStateMachine()
    {
        currentState = GetInitialState();
        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public void ChangeState(State newState)
    {
        currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    protected abstract State GetInitialState();
}