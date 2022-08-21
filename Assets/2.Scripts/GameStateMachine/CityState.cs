using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityState : State
{
    private GameObject cityPanel = null;
    private TMP_Text calendar = null;
    private GameObject askForTravel = null;
    private Button[] locationsPanelButtons = null;
    private Button[] locationsNumberButtons = null;
    private Button travel = null;
    private List<LocationScriptable> locations = null;
    private TMP_Text destination = null;
    private int locationIndex = 0;
    private GameObject[] frames = null;

    public CityState(StateMachine stateMachine, GameObject cityPanel, TMP_Text calendar, GameObject askForTravel, Button[] locationsPanelButtons,
        Button[] locationsNumberButtons, List<LocationScriptable> locations, TMP_Text destination, Button travel, GameObject[] frames) : base( stateMachine)
    {
        this.cityPanel = cityPanel;
        this.calendar = calendar;
        this.askForTravel = askForTravel;
        this.locationsPanelButtons = locationsPanelButtons;
        this.locationsNumberButtons = locationsNumberButtons;
        this.locations = locations;
        this.destination = destination;
        this.travel = travel;
        this.frames = frames;
        

        for (int i = 0; i < locationsPanelButtons.Length; i++)
        {
            int index = i;
            locationsPanelButtons[i].onClick.AddListener(() => { AskToTravel(index); });
        }

        for (int i = 0; i < locationsNumberButtons.Length; i++)
        {
            int index = i;
            locationsNumberButtons[i].onClick.AddListener(() => { AskToTravel(index); });
        }
        
        travel.onClick.AddListener(() => { TravelTo(locationIndex); });
    }

    public override void Enter()
    {
        cityPanel.gameObject.SetActive(true);
        CheckTurn();
        CheckGameOver();
    }
    public override void HandleInput() { }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        cityPanel.gameObject.SetActive(false);
    }

    private void CheckTurn()
    {
        GameData.Instance.ChangeTurn();
        frames[0].SetActive(GameData.Instance.playerTurn == 0);
        frames[1].SetActive(GameData.Instance.playerTurn == 1);
    }

    private void CheckGameOver()
    {
        calendar.text = GameData.Instance.DayGone().ToString();
        if (GameData.Instance.DaysLeft < 0)
        {
            calendar.text = "0";
            //TODO: GameOver
        }
    }

    private void AskToTravel(int location)
    {        
        askForTravel.SetActive(true);
        locationIndex = location;
        destination.text = locations[location].name;
    }

    private void TravelTo(int location)
    {
        ((GameSM)stateMachine).locationState.GetLocation(locations[location]);
        stateMachine.ChangeState(((GameSM)stateMachine).locationState);
    }
}
