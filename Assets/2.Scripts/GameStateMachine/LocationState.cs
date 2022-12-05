using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class TasksImages
{
    public Image[] TaskImages;
}

public class LocationState : State
{
    public const string SuccessHeader = "Enhorabuena";
    public const string SuccessText = "Has encontrado todas las pistas del lugar.";
    
    public const string FailureHeader = "Fracasaste";
    public const string FailureText = "No has encontrado informacion relevante.";

    private LocationScriptable _location = null;
    private int _selectedTaskIndex = 0;
    private bool[] _completedTasks = null;
    private int _completedTaskNumber = 0;
    private bool _taskSelected = false;
    private bool[] _dicesReroll = null;
    private bool _rerolling = false;
    private bool _rerolled = false;    

    [SerializeField] private Image _paperLocationImage = null;
    [SerializeField] private TMP_Text _paperLocationLabel = null;
    [SerializeField] private TasksImages[] _tasksImages = null;
    [SerializeField] private List<Sprite> _diceImages = null;
    [SerializeField] private Button[] _dices = null;
    [SerializeField] private Image _investigatorPortrait = null;
    [SerializeField] private Button _investigateButton = null;
    [SerializeField] private GameObject[] _completedTasksSeal = null;
    [SerializeField] private Button[] _taskIndicatorButtons = null;
    [SerializeField] private Button _rerollButton = null;

    [Header("Location Window")]
    [SerializeField] private GameObject _locationEndedWindow = null;
    [SerializeField] private TMP_Text _locationEndedHeader = null;
    [SerializeField] private TMP_Text _locationEndedText = null;
    [SerializeField] private Button _continueButton = null;
    
    public override void PreLoadState()
    {
        _investigateButton.onClick.AddListener(() => { RollDices(); });
        _continueButton.onClick.AddListener(() => { LocationEnded(); });
        _rerollButton.onClick.AddListener(() => { ActiveReroll(); });

        for (int i = 0; i < _taskIndicatorButtons.Length; i++)
        {
            int number = i;
            _taskIndicatorButtons[i].onClick.AddListener(() => { SelectTask(number); });
        }

        for (int i = 0; i < _dices.Length; i++)
        {
            int index = i;
            _dices[index].onClick.AddListener(() => { SelectDiceToReroll(index); });
        }
    }

    public async override void Enter()
    {
        _stateMachine.PlaySound(_location.Clip);
        gameObject.SetActive(true);
        
        _dicesReroll = new bool[_dices.Length];
        _investigatorPortrait.sprite = _location.InvestigatorPortrait[GameData.Instance.PlayerTurn];
        
        ResetButtons();
        ResetDices();
        LoadTasks();
        ResetIndicators();
        SelectTask();

        await _stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }

    public override void Exit()
    {
        gameObject.SetActive(false);
        _locationEndedWindow.SetActive(false);
    }

    private void ResetButtons()
    {
        _investigateButton.enabled = true;
        _rerollButton.enabled = true;
        _rerollButton.gameObject.SetActive(false);

        _rerolling = false;
        _rerolled = GameData.Instance.PlayerTurn == 0;
    }

    private void ResetDices()
    {
        int index = 0;

        foreach (Dice diceValue in Enum.GetValues(typeof(Dice)))
        {
            _dices[index].image.sprite = _diceImages[(int)diceValue];
            _dices[index].gameObject.SetActive(true);            
            index++;
        }
        GameData.Instance.RecoverDices();
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
                images[i].sprite = _diceImages[(int)task[i]];
            }
            else
            {
                images[i].gameObject.SetActive(false);
            }
        }
    }

    private void RollDices()
    {
        if (_rerolling)
        {
            RerollDices();
            return;
        }
        Investigate();
    }

    private async void Investigate()
    {
        if (!_taskSelected)
            return;

        if (IsLocationFailed() && GameData.Instance.PlayerTurn == 1)
        {
            await ShowEndLocationWindow(FailureHeader, FailureText);
            return;
        }

        await HideDiceButtons();

        List<Dice> roll = DiceRoll.GetDiceRoll(GameData.Instance.DicesAvailable);

        for (int i = 0; i < _dices.Length; i++)
        {
            if (i < GameData.Instance.DicesAvailable)
            {
                _dices[i].image.sprite = _diceImages[(int)roll[i]];
                await ShowDiceButton(i);                
            }
            else
            {
                DiceRoll.FadeDice(_dices[i].image, 0.0f, 0.0f);
            }
        }

        CheckDiceTask(_location.DiceTasks[_selectedTaskIndex].Task, roll);
    }

    private async void RerollDices()
    {
        int dicesSelected = 0;

        for (int i = 0; i < _dicesReroll.Length; i++)
        {
            if (_dicesReroll[i])
                dicesSelected++;
        }

        if (dicesSelected == 0) 
            return;

        _rerolled = true;
        _investigateButton.enabled = false;
        _rerollButton.enabled = false;
        await FadeRerollButtons(0.0f);

        GameData.Instance.RecoverDice();

        List<Dice> roll = DiceRoll.GetDiceRoll(GameData.Instance.DicesAvailable);

        for (int i =0 ; i < GameData.Instance.DicesAvailable; i++)
        {
            if (!_dicesReroll[i])
                roll[i] = (Dice)_diceImages.IndexOf(_dices[i].image.sprite);
            
            _dices[i].image.sprite = _diceImages[(int)roll[i]];            
        }

        await FadeRerollButtons(1.0f);        

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
            await ShowEndLocationWindow(SuccessHeader, SuccessText);
        }
        else
        {
            if (IsLocationFailed() && _rerolled)
            {
                await ShowEndLocationWindow(FailureHeader, FailureText);
                return;
            }
            SelectTask();
        }
    }

    private void ShowRerollButton(bool status)
    {
        _rerollButton.gameObject.SetActive(status && !_rerolled);
        _rerolling = false;

        if (status)
        {
            for (int i = 0; i < _dices.Length; i++)
            {
                _dices[i].enabled = status;
                _dicesReroll[i] = false;
            }
        }
    }

    private void ActiveReroll()
    {
        _rerolling = true;
    }

    private void SelectDiceToReroll(int index)
    {
        if (_rerolling)
            _dicesReroll[index] = true;        
    }

    public void GetLocation(LocationScriptable location)
    {
        _location = location;
        _paperLocationImage.sprite = location.Sprite;
        _paperLocationLabel.text = location.Name;
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

        _locationEndedWindow.SetActive(true);
        _locationEndedHeader.text = header;
        _locationEndedText.text = text;
    }

    private async void LocationEnded()
    {
        await _stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.CityState);
    }

    private async Task HideDiceButtons()
    {
        _investigateButton.enabled = false;
        _rerollButton.enabled = false;

        for (int i = 0; i < _dices.Length; i++)
        {
            DiceRoll.FadeDice(_dices[i].image, 0.0f, 0.3f);
            await Task.Delay(150);
        }

        await Task.Delay(500);
    }

    private async Task ShowDiceButton(int index)
    {
        _dices[index].gameObject.SetActive(true);
        DiceRoll.FadeDice(_dices[index].image, 1.0f, 0.3f);
        await Task.Delay(500);
    }

    private async Task FadeRerollButtons(float alpha)
    {
        int delay = (alpha == 0.0f) ? 150 : 500;

        for (int i = 0; i < _dices.Length; i++)
        {
            if (_dicesReroll[i])
            {
                DiceRoll.FadeDice(_dices[i].image, alpha, 0.3f);
                await Task.Delay(delay);
            }
        }

        await Task.Delay(500);
    }
}
