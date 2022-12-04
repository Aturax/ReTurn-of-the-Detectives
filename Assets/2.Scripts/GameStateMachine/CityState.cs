using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class CityState : State
{
    private const string WinnerHeader = "Enhorabuena";
    private const string WinnerBody = "El asesino ha sido atrapado";

    private const string LooserHeader = "Enhorabuena";
    private const string LooserBody = "El asesino ha sido atrapado";

    [SerializeField] private AudioSource audiosource = null;
    [SerializeField] private AudioClip cityClip = null;

    [SerializeField] private GameObject cityPanel = null;
    [SerializeField] private TMP_Text calendar = null;
    [SerializeField] private GameObject askForTravel = null;
    [SerializeField] private Button[] locationsPanelButtons = null;
    [SerializeField] private Button[] locationsNumberButtons = null;
    [SerializeField] private Button travel = null;
    [SerializeField] private List<LocationScriptable> locations = null;
    [SerializeField] private TMP_Text destination = null;
    [SerializeField] private GameObject[] characterFrames = null;
    [SerializeField] private GameObject[] completed = null;

    [SerializeField] private GameObject gameOverWindow = null;
    [SerializeField] private TMP_Text gameOverHeader = null;
    [SerializeField] private TMP_Text gameOverBody = null;
    [SerializeField] private Button continueButton = null;

    private int locationIndex = 0;

    public CityState(GameSM stateMachine) : base(stateMachine) { }

    public override void LoadState()
    {
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
        continueButton.onClick.AddListener(() => { EndGame(); });
    }

    public async override void Enter()
    {
        audiosource.clip = cityClip;
        audiosource.Play();
        cityPanel.gameObject.SetActive(true);
        CheckTurn();
        CheckCompletedTasks();
        CheckGameOver();
        await _stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }

    public override void Exit()
    {
        askForTravel.SetActive(false);
        cityPanel.gameObject.SetActive(false);
        gameOverWindow.SetActive(false);
    }

    private void CheckTurn()
    {
        GameData.Instance.ChangeTurn();
        characterFrames[0].SetActive(GameData.Instance.playerTurn == 1);
        characterFrames[1].SetActive(GameData.Instance.playerTurn == 0);
    }

    private void CheckCompletedTasks()
    {
        for (int i = 0; i < completed.Length; i++)
        {
            bool status = GameData.Instance.IsLocationCompleted(i);
            completed[i].SetActive(status);
            locationsPanelButtons[i].enabled = !status;
            locationsNumberButtons[i].enabled = !status;
        }
    }

    private void CheckGameOver()
    {
        if (GameData.Instance.playerTurn == 0)
        {
            calendar.text = GameData.Instance.DayGone().ToString();
        }
        
        if (GameData.Instance.DaysLeft < 0)
        {
            calendar.text = "0";
            ShowGameOverWindow(LooserHeader, LooserBody, false);
        }

        if (GameData.Instance.LocationsPassed() == 3)
        {
            ShowGameOverWindow(WinnerHeader, WinnerBody, true);
        }
    }

    private void AskToTravel(int location)
    {        
        askForTravel.SetActive(true);
        locationIndex = location;
        destination.text = locations[location].name;
    }

    private async void TravelTo(int location)
    {
        _stateMachine.locationState.GetLocation(locations[location]);
        await _stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.locationState);
    }

    private void ShowGameOverWindow(string header, string text, bool winner)
    {
        gameOverWindow.SetActive(true);
        gameOverHeader.text = header;
        gameOverBody.text = text;
        _stateMachine.gameOverState.IsWinner(winner);
    }

    private async void EndGame()
    {
        await _stateMachine.Fade(1.0f, 0.5f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.gameOverState);
    }
}
