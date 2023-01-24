using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TasksImages
{
    public Image[] TaskImages;
}

public class LocationState : State
{
    private LocationScriptable _location = null;
    private bool _rerolling = false;
    private bool _rerolled = false;

    [SerializeField] private Newspaper _newspaper = null;
    [SerializeField] private LocationTasks _locationTasks = null;
    [SerializeField] private DiceButtons _diceButtons = null;
    [SerializeField] private Button _investigateButton = null;
    [SerializeField] private Button _rerollButton = null;

    [Header("Location Window")]
    [SerializeField] private DialogPanelWindow _locationEndedWindow = null;

    private void Awake()
    {
        _locationEndedWindow.AcceptButtonClicked += LocationEnded;
        _investigateButton.onClick.AddListener(() => { Investigate(); });
        _rerollButton.onClick.AddListener(() => { ActiveReroll(); });
    }

    private void OnDestroy()
    {
        _locationEndedWindow.AcceptButtonClicked -= LocationEnded;
        _investigateButton.onClick.RemoveAllListeners();
        _rerollButton.onClick.RemoveAllListeners();
    }

    public async override void Enter()
    {
        _stateMachine.PlaySound(_location.Clip);
        gameObject.SetActive(true);

        _newspaper.LoadData(_location, _location.InvestigatorPortrait[GameData.Instance.PlayerTurn]);
        _locationTasks.LoadTasks(_location);
        _locationTasks.ResetIndicators();
        ResetButtons();
        SelectTask();

        await _stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }

    public override void Exit()
    {
        gameObject.SetActive(false);
        _locationEndedWindow.ShowWindow(false);
    }

    private void ResetButtons()
    {
        EnableInvestigateRerollButtons(true);
        _rerollButton.gameObject.SetActive(false);
        _diceButtons.ResetDices();
        _rerolling = false;
        _rerolled = GameData.Instance.PlayerTurn == 0;        
    }

    private void EnableInvestigateRerollButtons(bool status)
    {
        _investigateButton.enabled = status;
        _rerollButton.enabled = status;
    }

    private void SelectTask()
    {
        if (_locationTasks.TaskSelected)
            return;

        if (GameData.Instance.PlayerTurn == 1)
        {
            _locationTasks.SelectTask();            
        }
        else
        {
            _locationTasks.ShowNotCompletedTasksIndicators();
            _locationTasks.SelectLastTask();
        }
    }

    private async void Investigate()
    {
        if (_diceButtons.GetNumberOfDicesToRoll() == 0 || !_locationTasks.TaskSelected)
            return;

        if (IsLocationFailed() && GameData.Instance.PlayerTurn == 1 && !_rerolling)
        {
            await ShowEndLocationWindow(TextKeys.HeaderFailure, TextKeys.LocationFailed);
            return;
        }

        EnableInvestigateRerollButtons(false);
        await _diceButtons.FadeButtons(0.0f);

        if (_rerolling)
        {
            _rerolled = true;
            GameData.Instance.RecoverDice();
        }

        List<Dice> roll = Roll.GetRoll(GameData.Instance.DicesAvailable);
        _diceButtons.FillWithRoll(roll);

        await _diceButtons.FadeButtons(1.0f);
        CompareTaskWithRoll(_location.DiceTasks[_locationTasks.SelectedTaskIndex].Task, roll);
    }

    private void CompareTaskWithRoll(Dice[] task, List<Dice> roll)
    {
        bool status = Roll.IsTaskSuccess(task, roll);

        if (status)
            TaskSuccess();
        else
            TaskFailed();

        EnableInvestigateRerollButtons(true);

        CheckLocalizationCompleted();
    }

    private void TaskSuccess()
    {
        _locationTasks.ActiveCompleteTaskSeal();
        GameData.Instance.SetTaskSucceed(_locationTasks.SelectedTaskIndex);
        GameData.Instance.RecoverDices();
        GameData.Instance.TasksStatus[_locationTasks.SelectedTaskIndex] = true;
        _locationTasks.DeselectTask();
        _diceButtons.DisableButtons();

        if (GameData.Instance.PlayerTurn == 1)
        {
            _locationTasks.IncreaseTaskIndex();
            ShowRerollButton(false);
        }
    }

    private void TaskFailed()
    {
        GameData.Instance.LoseDice();
        _diceButtons.DisableButtons();
        if (GameData.Instance.PlayerTurn == 1)
            ShowRerollButton(true);
    }

    private async void CheckLocalizationCompleted()
    {
        if (GameData.Instance.NumberOfTasksCompleted() == 3)
        {
            GameData.Instance.SetLocationSucceed(_location.Number);
            await ShowEndLocationWindow(TextKeys.HeaderCongratulations, TextKeys.LocationSuccess);
        }
        else
        {
            if (IsLocationFailed() && _rerolled)
            {
                await ShowEndLocationWindow(TextKeys.HeaderFailure, TextKeys.LocationFailed);
                return;
            }
            SelectTask();
        }
    }

    private void ShowRerollButton(bool status)
    {
        _rerollButton.gameObject.SetActive(status && !_rerolled);
        _rerolling = false;
    }

    private void ActiveReroll()
    {
        _rerolling = !_rerolling;

        if (_rerolling)
            _diceButtons.EnableButtons();
        else
            _diceButtons.DisableButtons();
    }

    public void SetLocation(LocationScriptable location)
    {
        _location = location;
    }    

    private bool IsLocationFailed()
    {
        return GameData.Instance.DicesAvailable < _location.DiceTasks[_locationTasks.SelectedTaskIndex].Task.Length;
    }

    private async Task ShowEndLocationWindow(string header, string text)
    {
        EnableInvestigateRerollButtons(false);

        await Task.Delay(1000);

        _locationEndedWindow.ShowWindow(true);
        _locationEndedWindow.FillTextData(header, text);
    }

    private async void LocationEnded()
    {
        await _stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.CityState);
    }
}
