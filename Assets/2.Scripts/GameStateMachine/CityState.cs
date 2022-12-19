using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityState : State
{
    [SerializeField] private AudioClip _cityClip = null;

    [SerializeField] private TMP_Text _calendar = null;
    [SerializeField] private DialogPanelWindow _travelWindow = null;
    [SerializeField] private Button[] _locationsPanelButtons = null;
    [SerializeField] private Button[] _locationsNumberButtons = null;    
    [SerializeField] private List<LocationScriptable> _locations = null;
    [SerializeField] private GameObject[] _characterFrames = null;
    [SerializeField] private GameObject[] _completed = null;

    [SerializeField] private DialogPanelWindow _gameOverWindow = null;

    private int _locationIndex = 0;

    private void Awake()
    {
        _gameOverWindow.AcceptButtonClicked += EndGame;
        _travelWindow.AcceptButtonClicked += TravelTo;

        for (int i = 0; i < _locationsPanelButtons.Length; i++)
        {
            int index = i;
            _locationsPanelButtons[i].onClick.AddListener(() => { AskToTravel(index); });
            _locationsNumberButtons[i].onClick.AddListener(() => { AskToTravel(index); });
        }
    }

    private void OnDestroy()
    {
        _gameOverWindow.AcceptButtonClicked -= EndGame;
        _travelWindow.AcceptButtonClicked -= TravelTo;

        for (int i = 0; i < _locationsPanelButtons.Length; i++)
        {
            _locationsPanelButtons[i].onClick.RemoveAllListeners();
            _locationsNumberButtons[i].onClick.RemoveAllListeners();
        }
    }

    public async override void Enter()
    {
        _stateMachine.PlaySound(_cityClip);
        gameObject.SetActive(true);
        ShowInvestigatorPortrait();
        UpdateLocationsAvaliables();
        UpdateCalendar();
        CheckGameOver();
        await _stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }

    public override void Exit()
    {
        _travelWindow.ShowWindow(false);
        gameObject.SetActive(false);
        _gameOverWindow.ShowWindow(false);
    }

    private void ShowInvestigatorPortrait()
    {
        GameData.Instance.ChangeTurn();
        _characterFrames[0].SetActive(GameData.Instance.PlayerTurn == 1);
        _characterFrames[1].SetActive(GameData.Instance.PlayerTurn == 0);
    }

    private void UpdateLocationsAvaliables()
    {
        for (int i = 0; i < _completed.Length; i++)
        {
            bool status = GameData.Instance.IsLocationCompleted(i);
            _completed[i].SetActive(status);
            _locationsPanelButtons[i].enabled = !status;
            _locationsNumberButtons[i].enabled = !status;
        }
    }

    private void UpdateCalendar()
    {
        if (GameData.Instance.PlayerTurn == 0)
            _calendar.text = GameData.Instance.DayGone().ToString();
    }

    private void CheckGameOver()
    {        
        if (GameData.Instance.DaysLeft < 0)
        {
            _calendar.text = "0";
            ShowGameOverWindow(TextKeys.HeaderFailure, TextKeys.CaseFailed, false);
        }

        if (GameData.Instance.NumberOfLocationsCompleted() == 3)
        {
            ShowGameOverWindow(TextKeys.HeaderCongratulations, TextKeys.CaseResolved, true);
        }
    }

    private void AskToTravel(int location)
    {        
        _travelWindow.ShowWindow(true);
        _travelWindow.FillTextData(_locations[location].Name, TextKeys.TravelQuestion);
        _locationIndex = location;        
    }

    private async void TravelTo()
    {
        _stateMachine.LocationState.SetLocation(_locations[_locationIndex]);
        await _stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.LocationState);
    }

    private void ShowGameOverWindow(string header, string text, bool winner)
    {
        _gameOverWindow.ShowWindow(true);
        _gameOverWindow.FillTextData(header, text);
        _stateMachine.GameOverState.IsWinner(winner);
    }

    private async void EndGame()
    {
        await _stateMachine.Fade(1.0f, 0.5f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.GameOverState);
    }
}
