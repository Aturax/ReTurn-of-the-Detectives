public class State
{
    public GameLoader gameManager;
    protected StateMachine stateMachine;

    public State(GameLoader gameManager, StateMachine stateMachine)
    {
        this.gameManager = gameManager;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void HandleInput() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}