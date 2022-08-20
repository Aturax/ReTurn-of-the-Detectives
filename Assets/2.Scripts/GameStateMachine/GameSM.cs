using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSM : StateMachine
{
    public MainMenuState mainMenuState;
    public CityState cityState;    

    protected override State GetInitialState()
    {
        return mainMenuState;
    }
}