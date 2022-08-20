using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : State
{
    private GameObject mainMenu = null;

    public MainMenuState(GameLoader gameManager, StateMachine stateMachine, GameObject mainMenu) : base(gameManager, stateMachine)
    {
        this.mainMenu = mainMenu;
    }

    public override void Enter()
    {
        mainMenu.SetActive(true);
    }

    public override void HandleInput() 
    {
        if (Input.GetMouseButtonUp(0))
        {
            stateMachine.ChangeState(((GameSM)stateMachine).cityState);
        }
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        mainMenu.SetActive(false);
    }
}