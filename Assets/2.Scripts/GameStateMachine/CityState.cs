using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityState : State
{
    [SerializeField] private AudioClip _cityClip = null;

    [SerializeField] private TMP_Text _calendar = null;
    [SerializeField] private GameObject _askForTravel = null;
    [SerializeField] private Button[] _locationsPanelButtons = null;
    [SerializeField] private Button[] _locationsNumberButtons = null;
    [SerializeField] private Button _travel = null;
    [SerializeField] private List<LocationScriptable> _locations = null;
    [SerializeField] private TMP_Text _destination = null;
    [SerializeField] private GameObject[] _characterFrames = null;
    [SerializeField] private GameObject[] _completed = null;

    [SerializeField] private GameObject _gameOverWindow = null;
    [SerializeField] private TMP_Text _gameOverHeader = null;
    [SerializeField] private TMP_Text _gameOverBody = null;
    [SerializeField] private Button _continueButton = null;

    private int _locationIndex = 0;

    public override void PreLoadState()
    {
        for (int i = 0; i < _locationsPanelButtons.Length; i++)
        {
            int index = i;
            _locationsPanelButtons[i].onClick.AddListener(() => { AskToTravel(index); });
            _locationsNumberButtons[i].onClick.AddListener(() => { AskToTravel(index); });
        }
        
        _travel.onClick.AddListener(() => { TravelTo(_locationIndex); });
        _continueButton.onClick.AddListener(() => { EndGame(); });
    }

    public async override void Enter()
    {
        _stateMachine.PlaySound(_cityClip);
        gameObject.SetActive(true);
        CheckTurn();
        CheckCompletedTasks();
        CheckGameOver();
        await _stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }

    public override void Exit()
    {
        _askForTravel.SetActive(false);
        gameObject.SetActive(false);
        _gameOverWindow.SetActive(false);
    }

    private void CheckTurn()
    {
        GameData.Instance.ChangeTurn();
        _characterFrames[0].SetActive(GameData.Instance.PlayerTurn == 1);
        _characterFrames[1].SetActive(GameData.Instance.PlayerTurn == 0);
    }

    private void CheckCompletedTasks()
    {
        for (int i = 0; i < _completed.Length; i++)
        {
            bool status = GameData.Instance.IsLocationCompleted(i);
            _completed[i].SetActive(status);
            _locationsPanelButtons[i].enabled = !status;
            _locationsNumberButtons[i].enabled = !status;
        }
    }

    private void CheckGameOver()
    {
        if (GameData.Instance.PlayerTurn == 0)
            _calendar.text = GameData.Instance.DayGone().ToString();
        
        if (GameData.Instance.DaysLeft < 0)
        {
            _calendar.text = "0";
            ShowGameOverWindow(TextKeys.HeaderFailure, TextKeys.CaseFailed, false);
        }

        if (GameData.Instance.LocationsCompleted() == 3)
        {
            ShowGameOverWindow(TextKeys.HeaderCongratulations, TextKeys.CaseResolved, true);
        }
    }

    private void AskToTravel(int location)
    {        
        _askForTravel.SetActive(true);
        _locationIndex = location;
        _destination.text = _locations[location].Name;
    }

    private async void TravelTo(int location)
    {
        _stateMachine.LocationState.GetLocation(_locations[location]);
        await _stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.LocationState);
    }

    private void ShowGameOverWindow(string header, string text, bool winner)
    {
        _gameOverWindow.SetActive(true);
        _gameOverHeader.text = header;
        _gameOverBody.text = text;
        _stateMachine.GameOverState.IsWinner(winner);
    }

    private async void EndGame()
    {
        await _stateMachine.Fade(1.0f, 0.5f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.GameOverState);
    }
}
