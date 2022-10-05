using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSM : StateMachine
{
    public MainMenuState mainMenuState = null;
    public CityState cityState = null;
    public LocationState locationState = null;
    public GameOverState gameOverState = null;

    protected override State GetInitialState()
    {
        return mainMenuState;
    }
}