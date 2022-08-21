using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSM : StateMachine
{
    public MainMenuState mainMenuState;
    public CityState cityState;
    public LocationState locationState;
    public GameOverState gameOverState;

    protected override State GetInitialState()
    {
        return mainMenuState;
    }
}