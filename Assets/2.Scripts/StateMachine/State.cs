using UnityEngine;

public class State: MonoBehaviour
{
    protected StateMachine stateMachine;

    public void LoadStateMachine(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        PreaLoadState();
    }

    public virtual void PreaLoadState() { }
    public virtual void Enter() { }
    public virtual void Exit() { }
}