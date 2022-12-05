using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State _currentState = null;
    
    public void StartStateMachine()
    {
        _currentState = GetInitialState();
        if (_currentState != null)
            _currentState.Enter();        
    }

    public void ChangeState(State newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    protected abstract State GetInitialState();
}