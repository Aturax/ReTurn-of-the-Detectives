using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class GameSM : StateMachine
{
    public MainMenuState mainMenuState = null;
    public CityState cityState = null;
    public LocationState locationState = null;
    public GameOverState gameOverState = null;
    private Fader fader = null;

    protected override State GetInitialState()
    {
        return mainMenuState;
    }

    public void LoadStateMachine(Fader fader)
    {
        mainMenuState = new MainMenuState(this);
        cityState = new CityState(this);
        locationState = new LocationState(this);
        gameOverState = new GameOverState(this);
        this.fader = fader;
        fader.LoadFader();       
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