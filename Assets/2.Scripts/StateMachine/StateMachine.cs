using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public abstract class StateMachine
{
    private State currentState = null;
    private bool started = false;
    private Fader fader = null;

    public void StartStateMachine()
    {
        currentState = GetInitialState();
        if (currentState != null)
        {
            currentState.Enter();
        }
        started = true;
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

    public bool IsStarted()
    {
        return started;
    }

    public void SetFader(Fader fader)
    {
        this.fader = fader;
    }

    public async Task Fade(float alpha, float time) 
    {
        await fader.Fade(alpha, time);
    }

    public async Task Fade(float alpha, float time, AudioClip clip)
    {
        await fader.Fade(alpha, time, clip);
    }
}