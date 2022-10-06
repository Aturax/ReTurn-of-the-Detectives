using UnityEngine;

public class State: MonoBehaviour
{
    protected GameSM stateMachine;

    public void LoadStateMachine(GameSM stateMachine)
    {
        this.stateMachine = stateMachine;
        PreaLoadState();
    }

    public virtual void PreaLoadState() { }
    public virtual void Enter() { }
    public virtual void Exit() { }
}