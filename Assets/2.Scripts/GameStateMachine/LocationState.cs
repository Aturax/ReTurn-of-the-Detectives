using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

[Serializable]
public class TasksImages
{
    public Image[] taskImages;
}

[Serializable]
public class LocationState : State
{
    public const string SuccessHeader = "Enhorabuena";
    public const string SuccessText = "Has encontrado todas las pistas del lugar.";
    
    public const string FailureHeader = "Fracasaste";
    public const string FailureText = "No has encontrado informacion relevante.";


    private LocationScriptable location = null;
    private int selectedTaskIndex = 0;
    private bool[] completedTasks = null;
    private int completedTaskNumber = 0;
    private bool taskSelected = false;
    private bool[] dicesReroll = null;
    private bool rerolling = false;
    private bool rerolled = false;    

    [SerializeField] private AudioSource audiosource = null;
    [SerializeField] private GameObject locationPanel = null;
    [SerializeField] private Image paperLocationImage = null;
    [SerializeField] private TMP_Text paperLocationLabel = null;
    [SerializeField] private TasksImages[] tasksImages = null;
    [SerializeField] private List<Sprite> diceImages = null;
    [SerializeField] private Button[] dices = null;
    [SerializeField] private Image investigatorPortrait = null;
    [SerializeField] private Button investigateButton = null;
    [SerializeField] private GameObject[] completedTasksSeal = null;
    [SerializeField] private Button[] taskIndicatorButtons = null;
    [SerializeField] private Button rerollButton = null;

    [Header("Location Window")]
    [SerializeField] private GameObject locationEndedWindow = null;
    [SerializeField] private TMP_Text locationEndedHeader = null;
    [SerializeField] private TMP_Text locationEndedText = null;
    [SerializeField] private Button continueButton = null;

    public override void PreaLoadState()
    {
        investigateButton.onClick.AddListener(() => { RollDices(); });
        continueButton.onClick.AddListener(() => { LocationEnded(); });
        rerollButton.onClick.AddListener(() => { ActiveReroll(); });

        for (int i = 0; i < taskIndicatorButtons.Length; i++)
        {
            int number = i;
            taskIndicatorButtons[i].onClick.AddListener(() => { SelectTask(number); });
        }

        for (int i = 0; i < dices.Length; i++)
        {
            int index = i;
            dices[index].onClick.AddListener(() => { SelectDiceToReroll(index); });
        }
    }

    public async override void Enter()
    {
        audiosource.clip = location.clip;
        audiosource.Play();

        locationPanel.gameObject.SetActive(true);
        
        dicesReroll = new bool[dices.Length];
        investigatorPortrait.sprite = location.investigatorPortrait[GameData.Instance.playerTurn];
        
        ResetButtons();
        ResetDices();
        LoadTasks();
        ResetIndicators();
        SelectTask();

        await stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }

    public override void Exit()
    {
        locationPanel.gameObject.SetActive(false);
        locationEndedWindow.SetActive(false);
    }

    private void ResetButtons()
    {
        investigateButton.enabled = true;
        rerollButton.enabled = true;
        rerollButton.gameObject.SetActive(false);

        rerolling = false;
        rerolled = GameData.Instance.playerTurn == 0;
    }

    private void ResetDices()
    {
        int index = 0;

        foreach (Dice diceValue in Enum.GetValues(typeof(Dice)))
        {
            dices[index].image.sprite = diceImages[(int)diceValue];
            dices[index].gameObject.SetActive(true);            
            index++;
        }
        GameData.Instance.RecoverDices();
    }

    private void ResetIndicators()
    {
        for (int i = 0; i < completedTasksSeal.Length; i++)
        {
            completedTasksSeal[i].SetActive(false);
        }

        for (int i = 0; i < taskIndicatorButtons.Length; i++)
        {
            taskIndicatorButtons[i].gameObject.SetActive(GameData.Instance.playerTurn == 0);
            taskIndicatorButtons[i].enabled = GameData.Instance.playerTurn == 0;
        }
    }

    private void LoadTasks()
    {
        selectedTaskIndex = 0;
        taskSelected = false;

        completedTasks = new bool[taskIndicatorButtons.Length];
        completedTaskNumber = 0;

        for (int i = 0; i < taskIndicatorButtons.Length; i++)
        {
            completedTasks[i] = false;
        }

        for (int i = 0; i < location.diceTasks.Length; i++)
        {
            ShowDiceTask(tasksImages[i].taskImages, location.diceTasks[i].task);
        }
    }

    private void SelectTask()
    {
        if (taskSelected) return;

        if (GameData.Instance.playerTurn == 1)
        {
            for (int i = 0; i < taskIndicatorButtons.Length; i++)
            {
                taskIndicatorButtons[i].gameObject.SetActive(i == selectedTaskIndex);
            }

            taskSelected = true;
        }
        else
        {
            for (int i = 0; i < taskIndicatorButtons.Length; i++)
            {
                taskIndicatorButtons[i].gameObject.SetActive(!completedTasks[i]);
            }
            AutoSelectLastTask();
        }
    }

    public void SelectTask(int index)
    {
        for (int i = 0; i < taskIndicatorButtons.Length; i++)
        {
            taskIndicatorButtons[i].gameObject.SetActive(i == index);
        }

        selectedTaskIndex = index;
        taskSelected = true;
    }

    private void AutoSelectLastTask()
    {
        if (completedTaskNumber == 2)
        {
            for (int i = 0; i < completedTasks.Length; i++)
            {
                if (!completedTasks[i])
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
                images[i].sprite = diceImages[(int)task[i]];
            }
            else images[i].gameObject.SetActive(false);
        }
    }

    private void RollDices()
    {
        if (rerolling)
        {
            RerollDices();
            return;
        }
        Investigate();
    }

    private async void Investigate()
    {
        if (!taskSelected) return;

        if (IsLocationFailed() && GameData.Instance.playerTurn == 1)
        {
            await ShowEndLocationWindow(FailureHeader, FailureText);
            return;
        }

        await HideDiceButtons();

        List<Dice> roll = DiceRoll.GetDiceRoll(GameData.Instance.dicesAvailable);

        for (int i = 0; i < dices.Length; i++)
        {
            if (i < GameData.Instance.dicesAvailable)
            {
                dices[i].image.sprite = diceImages[(int)roll[i]];
                await ShowDiceButton(i);                
            }
            else
            {
                DiceRoll.FadeDice(dices[i].image, 0.0f, 0.0f);
            }
        }

        CheckDiceTask(location.diceTasks[selectedTaskIndex].task, roll);
    }

    private async void RerollDices()
    {
        int dicesSelected = 0;

        for (int i = 0; i < dicesReroll.Length; i++)
        {
            if (dicesReroll[i]) dicesSelected++;
        }

        if (dicesSelected == 0) return;
        rerolled = true;

        investigateButton.enabled = false;
        rerollButton.enabled = false;
        await FadeRerollButtons(0.0f);

        GameData.Instance.RecoverDice();

        List<Dice> roll = DiceRoll.GetDiceRoll(GameData.Instance.dicesAvailable);

        for (int i =0 ; i < GameData.Instance.dicesAvailable; i++)
        {
            if (!dicesReroll[i])
            {
                roll[i] = (Dice)diceImages.IndexOf(dices[i].image.sprite);
            }
            dices[i].image.sprite = diceImages[(int)roll[i]];            
        }

        await FadeRerollButtons(1.0f);        

        CheckDiceTask(location.diceTasks[selectedTaskIndex].task, roll);
    }

    private async void CheckDiceTask(Dice[] task, List<Dice> roll)
    {
        bool status = DiceRoll.CheckDiceTask(task, roll);
        completedTasksSeal[selectedTaskIndex].SetActive(status);

        if (status)
        {
            completedTasksSeal[selectedTaskIndex].SetActive(DiceRoll.CheckDiceTask(task, roll));
            completedTasksSeal[selectedTaskIndex].SetActive(true);
            GameData.Instance.SetTaskPassed(selectedTaskIndex);
            GameData.Instance.RecoverDices();
            completedTasks[selectedTaskIndex] = true;
            completedTaskNumber++;            
            taskSelected = false;
            if (GameData.Instance.playerTurn == 1)
            {
                selectedTaskIndex++;
                ShowRerollButton(false);
            }
        }
        else
        {
            GameData.Instance.LoseDice();
            if (GameData.Instance.playerTurn == 1) ShowRerollButton(true);
        }

        investigateButton.enabled = true;
        rerollButton.enabled = true;

        if (completedTaskNumber == 3)
        {
            GameData.Instance.SetLocationPassed(location.number);
            await ShowEndLocationWindow(SuccessHeader, SuccessText);
        }
        else
        {
            if (IsLocationFailed() && rerolled)
            {
                await ShowEndLocationWindow(FailureHeader, FailureText);
                return;
            }
            SelectTask();
        }
    }

    private void ShowRerollButton(bool status)
    {
        rerollButton.gameObject.SetActive(status && !rerolled);
        rerolling = false;

        if (status)
        {
            for (int i = 0; i < dices.Length; i++)
            {
                dices[i].enabled = status;
                dicesReroll[i] = false;
            }
        }
    }

    private void ActiveReroll()
    {
        rerolling = true;
    }

    private void SelectDiceToReroll(int index)
    {
        if (rerolling)
        {
            dicesReroll[index] = true;
        }
    }

    public void GetLocation(LocationScriptable location)
    {
        this.location = location;
        paperLocationImage.sprite = location.sprite;
        paperLocationLabel.text = location.name;
    }

    private bool IsLocationFailed()
    {
        return GameData.Instance.dicesAvailable < location.diceTasks[selectedTaskIndex].task.Length;
    }

    private async Task ShowEndLocationWindow(string header, string text)
    {
        investigateButton.enabled = false;
        rerollButton.enabled = false;

        await Task.Delay(1000);

        locationEndedWindow.SetActive(true);
        locationEndedHeader.text = header;
        locationEndedText.text = text;
    }

    private async void LocationEnded()
    {
        await stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        stateMachine.ChangeState(stateMachine.cityState);
    }

    private async Task HideDiceButtons()
    {
        investigateButton.enabled = false;
        rerollButton.enabled = false;

        for (int i = 0; i < dices.Length; i++)
        {
            DiceRoll.FadeDice(dices[i].image, 0.0f, 0.3f);
            await Task.Delay(150);
        }

        await Task.Delay(500);
    }

    private async Task ShowDiceButton(int index)
    {
        dices[index].gameObject.SetActive(true);
        DiceRoll.FadeDice(dices[index].image, 1.0f, 0.3f);
        await Task.Delay(500);
    }

    private async Task FadeRerollButtons(float alpha)
    {
        int delay = (alpha == 0.0f) ? 150 : 500;

        for (int i = 0; i < dices.Length; i++)
        {
            if (dicesReroll[i])
            {
                DiceRoll.FadeDice(dices[i].image, alpha, 0.3f);
                await Task.Delay(delay);
            }
        }

        await Task.Delay(500);
    }
}
