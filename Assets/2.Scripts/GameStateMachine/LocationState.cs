using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[Serializable]
public class TasksImages
{
    public Image[] TaskImages;
}

public class LocationState : State
{
    private LocationScriptable _location = null;
    private int _selectedTaskIndex = 0;
    private bool[] _completedTasks = null;
    private int _completedTaskNumber = 0;
    private bool _taskSelected = false;    
    private bool _rerolling = false;
    private bool _rerolled = false;

    [SerializeField] private Newspaper _newspaper = null;
    [SerializeField] private DiceButtons _diceButtons = null;
    [SerializeField] private TasksImages[] _tasksImages = null;
    [SerializeField] private Button _investigateButton = null;
    [SerializeField] private GameObject[] _completedTasksSeal = null;
    [SerializeField] private Button[] _taskIndicatorButtons = null;
    [SerializeField] private Button _rerollButton = null;

    [Header("Location Window")]
    [SerializeField] private DialogPanelWindow _locationEndedWindow = null;

    private void Awake()
    {
        _locationEndedWindow.AcceptButtonClicked += LocationEnded;
    }

    private void OnDestroy()
    {
        _locationEndedWindow.AcceptButtonClicked -= LocationEnded;
    }

    public override void PreLoadState()
    {
        _investigateButton.onClick.AddListener(() => { Investigate(); });        
        _rerollButton.onClick.AddListener(() => { ActiveReroll(); });

        for (int i = 0; i < _taskIndicatorButtons.Length; i++)
        {
            int number = i;
            _taskIndicatorButtons[i].onClick.AddListener(() => { SelectTask(number); });
        }
    }

    public async override void Enter()
    {
        _stateMachine.PlaySound(_location.Clip);
        gameObject.SetActive(true);
        GameData.Instance.RecoverDices();
        LoadNewspaperData();
        ResetButtons();        
        LoadTasks();
        ResetIndicators();
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
        _investigateButton.enabled = true;
        _rerollButton.enabled = true;
        _rerollButton.gameObject.SetActive(false);

        _rerolling = false;
        _rerolled = GameData.Instance.PlayerTurn == 0;
    }

    private void ResetIndicators()
    {
        for (int i = 0; i < _completedTasksSeal.Length; i++)
        {
            _completedTasksSeal[i].SetActive(false);
        }

        for (int i = 0; i < _taskIndicatorButtons.Length; i++)
        {
            _taskIndicatorButtons[i].gameObject.SetActive(GameData.Instance.PlayerTurn == 0);
            _taskIndicatorButtons[i].enabled = GameData.Instance.PlayerTurn == 0;
        }
    }

    private void LoadTasks()
    {
        _selectedTaskIndex = 0;
        _taskSelected = false;

        _completedTasks = new bool[_taskIndicatorButtons.Length];
        _completedTaskNumber = 0;

        for (int i = 0; i < _taskIndicatorButtons.Length; i++)
        {
            _completedTasks[i] = false;
        }

        for (int i = 0; i < _location.DiceTasks.Length; i++)
        {
            ShowDiceTask(_tasksImages[i].TaskImages, _location.DiceTasks[i].Task);
        }
    }

    private void SelectTask()
    {
        if (_taskSelected) return;

        if (GameData.Instance.PlayerTurn == 1)
        {
            for (int i = 0; i < _taskIndicatorButtons.Length; i++)
            {
                _taskIndicatorButtons[i].gameObject.SetActive(i == _selectedTaskIndex);
            }

            _taskSelected = true;
        }
        else
        {
            for (int i = 0; i < _taskIndicatorButtons.Length; i++)
            {
                _taskIndicatorButtons[i].gameObject.SetActive(!_completedTasks[i]);
            }
            AutoSelectLastTask();
        }
    }

    public void SelectTask(int index)
    {
        for (int i = 0; i < _taskIndicatorButtons.Length; i++)
        {
            _taskIndicatorButtons[i].gameObject.SetActive(i == index);
        }

        _selectedTaskIndex = index;
        _taskSelected = true;
    }

    private void AutoSelectLastTask()
    {
        if (_completedTaskNumber == 2)
        {
            for (int i = 0; i < _completedTasks.Length; i++)
            {
                if (!_completedTasks[i])
                {
                    SelectTask(i);
                    return;
                }                
            }            
        }
    }

    private void ShowDiceTask(Image[] images, Dice[] task)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (task.Length > i)
            {
                images[i].gameObject.SetActive(true);
                images[i].sprite = _diceButtons.GetDiceFace((int)task[i]);
            }
            else
            {
                images[i].gameObject.SetActive(false);
            }
        }
    }

    private async void Investigate()
    {
        if (_diceButtons.GetNumberOfDicesToRoll() == 0 && !_taskSelected)
            return;

        if (IsLocationFailed() && GameData.Instance.PlayerTurn == 1 && !_rerolling)
        {
            await ShowEndLocationWindow(TextKeys.HeaderFailure, TextKeys.LocationFailed);
            return;
        }

        _investigateButton.enabled = false;
        _rerollButton.enabled = false;
        await _diceButtons.FadeDiceButtons(0.0f);

        if (_rerolling)
        {
            _rerolled = true;
            GameData.Instance.RecoverDice();
        }

        List<Dice> roll = DiceRoll.GetDiceRoll(GameData.Instance.DicesAvailable);

        for (int i = 0; i < _diceButtons.GetDiceButtonsLength(); i++)
        {
            if (i > GameData.Instance.DicesAvailable - 1)
            {
                _diceButtons.FadeDice(i, 0.0f, 0.0f);
                _diceButtons.SelectDiceToRoll(i, false);
                continue;
            }

            if (_diceButtons.IsDiceSelected(i))
                _diceButtons.SetDiceImage(i, (int)roll[i]);
            else
                roll[i] = _diceButtons.GetDiceOfButton(i);            
        }

        await _diceButtons.FadeDiceButtons(1.0f);
        CheckDiceTask(_location.DiceTasks[_selectedTaskIndex].Task, roll);
    }

    private async void CheckDiceTask(Dice[] task, List<Dice> roll)
    {
        bool status = DiceRoll.CheckDiceTask(task, roll);
        _completedTasksSeal[_selectedTaskIndex].SetActive(status);

        if (status)
        {
            _completedTasksSeal[_selectedTaskIndex].SetActive(DiceRoll.CheckDiceTask(task, roll));
            _completedTasksSeal[_selectedTaskIndex].SetActive(true);
            GameData.Instance.SetTaskPassed(_selectedTaskIndex);
            GameData.Instance.RecoverDices();
            _completedTasks[_selectedTaskIndex] = true;
            _completedTaskNumber++;            
            _taskSelected = false;
            _diceButtons.DisableDiceButtons();
            if (GameData.Instance.PlayerTurn == 1)
            {
                _selectedTaskIndex++;
                ShowRerollButton(false);
            }
        }
        else
        {
            GameData.Instance.LoseDice();
            if (GameData.Instance.PlayerTurn == 1) 
                ShowRerollButton(true);
        }

        _investigateButton.enabled = true;
        _rerollButton.enabled = true;

        if (_completedTaskNumber == 3)
        {
            GameData.Instance.SetLocationPassed(_location.Number);
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
            _diceButtons.EnableDiceButtons();
        else
            _diceButtons.DisableDiceButtons();
    }

    public void SetLocation(LocationScriptable location)
    {
        _location = location;
    }

    public void LoadNewspaperData()
    {
        _newspaper.LoadData(_location.name, _location.Sprite, _location.InvestigatorPortrait[GameData.Instance.PlayerTurn]);        
    }

    private bool IsLocationFailed()
    {
        return GameData.Instance.DicesAvailable < _location.DiceTasks[_selectedTaskIndex].Task.Length;
    }

    private async Task ShowEndLocationWindow(string header, string text)
    {
        _investigateButton.enabled = false;
        _rerollButton.enabled = false;

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
