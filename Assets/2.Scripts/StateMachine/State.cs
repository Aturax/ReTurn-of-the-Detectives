using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected GameManager _stateMachine;

    public void SetStateMachine(GameManager stateMachine)
    {
        _stateMachine = stateMachine;     
    }
    
    public virtual void Enter() { }
    public virtual void Exit() { }
}