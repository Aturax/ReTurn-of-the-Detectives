public abstract class StateMachine
{
    private State currentState;

    public void StartStateMachine()
    {
        currentState = GetInitialState();
        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public void HandleInput()
    {
        if (currentState != null)
        {
            currentState.HandleInput();
        }
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
    }

    public void ChangeState(State newState)
    {
        currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    protected abstract State GetInitialState();

    public string GetCurrentState()
    {
        return currentState.ToString();
    }
}