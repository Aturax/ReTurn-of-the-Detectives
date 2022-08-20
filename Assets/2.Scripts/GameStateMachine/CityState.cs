using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityState : State
{
    private CityPanel cityPanel = null;

    public CityState(GameLoader gameManager, StateMachine stateMachine, CityPanel cityPanel) : base(gameManager, stateMachine)
    {
        this.cityPanel = cityPanel;

        for (int i = 0; i < cityPanel.locations.Length; i++)
        {
            int index = 1 + i;
            cityPanel.locations[i].onClick.AddListener(() => { AskToTravel(index); });
        }        
    }

    public override void Enter()
    {
        cityPanel.gameObject.SetActive(true);
    }
    public override void HandleInput() { }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        cityPanel.gameObject.SetActive(false);
    }

    private void AskToTravel(int location)
    {
        cityPanel.askForTravel.SetActive(true);
    }
}
