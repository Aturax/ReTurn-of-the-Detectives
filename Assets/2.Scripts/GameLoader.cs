using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] CityPanel cityPanel = null;
    
    public GameSM stateMachine;
    private bool started = false;


    private void InitializeStateMachine()
    {
        stateMachine = new GameSM();

        stateMachine.mainMenuState = new MainMenuState(this, stateMachine, mainMenu);
        stateMachine.cityState = new CityState(this, stateMachine, cityPanel);
                
        stateMachine.StartStateMachine();
        started = true;
    }


    void Start()
    {
        InitializeStateMachine();
    }

    void Update()
    {
        if (started)
        {
            stateMachine.HandleInput();
            stateMachine.Update();
            stateMachine.FixedUpdate();
        }
    }
}
