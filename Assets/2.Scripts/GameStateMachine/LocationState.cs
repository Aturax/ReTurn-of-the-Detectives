using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocationState : State
{
    private GameObject locationPanel = null;
    private LocationScriptable location = null;
    private Image locationImage = null;
    private TMP_Text locationLabel = null;
    private TasksImages[] tasksImages = null;
    private List<Sprite> diceImages = null;
    private List<Image> diceRoll = null;
    private Image investigatorImage = null;
    private Button investigateButton = null;
    private GameObject[] completedTasks = null;
    private GameObject[] tasksIndicators = null;
    private int selectedTask = 0;

    private GameObject locationEndedWindow = null;
    private TMP_Text locationEndedHeader = null;
    private TMP_Text locationEndedText = null;
    private Button continueButton = null;

    public LocationState(StateMachine stateMachine, GameObject locationPanel, Image locationImage, TMP_Text locationLabel,
        TasksImages[] tasksImages, List<Sprite> dice, List<Image> diceRoll, Image investigatorImage, Button investigateButton,
        GameObject[] completedTasks, GameObject[] tasksIndicators, GameObject locationEndedWindow, TMP_Text locationEndedHeader,
        TMP_Text locationEndedText, Button continueButton) : base( stateMachine)
    {
        this.locationPanel = locationPanel;
        this.locationImage = locationImage;
        this.locationLabel = locationLabel;

        this.tasksImages = tasksImages;

        diceImages = dice;

        this.diceRoll = diceRoll;
        this.investigatorImage = investigatorImage;
        this.investigateButton = investigateButton;
        this.completedTasks = completedTasks;
        this.tasksIndicators = tasksIndicators;
        this.locationEndedWindow = locationEndedWindow;
        this.locationEndedHeader = locationEndedHeader;
        this.locationEndedText = locationEndedText;
        this.continueButton = continueButton;

        investigateButton.onClick.AddListener(() => { RollDices(); });
        continueButton.onClick.AddListener(() => { LocationEnded(); });
    }

    public override void Enter()
    {
        locationPanel.gameObject.SetActive(true);
        investigatorImage.sprite = location.investigatorPortrait[GameData.Instance.playerTurn];
        selectedTask = 0;
        ResetDices();
        FillTasks();
        ResetIndicators();
        SelectTask();
    }
    public override void HandleInput() { }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        locationPanel.gameObject.SetActive(false);
        locationEndedWindow.SetActive(false);
    }

    private void ResetDices()
    {
        int index = 0;

        foreach(Dice diceValue in System.Enum.GetValues(typeof(Dice)))
        {
            diceRoll[index].sprite = diceImages[(int)diceValue];
            diceRoll[index].gameObject.SetActive(true);
            index++;
        }
        GameData.Instance.RecoverDices();
    }

    private void ResetIndicators()
    {
        for (int i = 0; i < completedTasks.Length; i++)
        {
            completedTasks[i].SetActive(false);        
        }

        for (int i = 0; i < tasksIndicators.Length; i++)
        {
            tasksIndicators[i].SetActive(false);
        }
    }

    private void FillTasks()
    {
        for (int i = 0; i < location.diceTasks.Length; i++)
        {
            ShowDiceTask(tasksImages[i].taskImages, location.diceTasks[i].task);
        }        
    }

    private void SelectTask()
    {
        for (int i = 0; i < tasksIndicators.Length; i++)
        {
            tasksIndicators[i].SetActive(i == selectedTask);
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
        if (GameData.Instance.dicesAvailable < location.diceTasks[selectedTask].task.Length) LocationFailed();
            
        List<Dice> roll = DiceRoll.GetDiceRoll(GameData.Instance.dicesAvailable);

        for (int i = 0; i < diceRoll.Count; i++)
        {
            if (i < GameData.Instance.dicesAvailable)
            {
                diceRoll[i].gameObject.SetActive(true);
                diceRoll[i].sprite = diceImages[(int)roll[i]];
            }
            else
            {
                diceRoll[i].gameObject.SetActive(false);
            }
        }

        CheckDiceTask(location.diceTasks[selectedTask].task, roll);
    }

    private void CheckDiceTask(Dice[] task, List<Dice> roll)
    {
        bool status = DiceRoll.CheckDiceTask(task, roll);
        completedTasks[selectedTask].SetActive(status);
        
        if (status)
        {
            completedTasks[selectedTask].SetActive(DiceRoll.CheckDiceTask(task, roll));
            completedTasks[selectedTask].SetActive(true);
            GameData.Instance.SetTaskPassed(selectedTask);
            GameData.Instance.RecoverDices();
            selectedTask++;
        }
        else
        {
            GameData.Instance.LoseDice();
        }

        if (selectedTask == 3)
        {
            GameData.Instance.SetLocationPassed(location.number);
            LocationSucceded();
        }
        else
        {
            SelectTask();
        }
    }

    public void GetLocation(LocationScriptable location)
    {
        this.location = location;
        locationImage.sprite = location.sprite;
        locationLabel.text = location.name;
    }

    private void LocationFailed()
    {
        locationEndedWindow.SetActive(true);
        locationEndedHeader.text = "Fracaso";
        locationEndedText.text = "No has conseguido averiguar nada";
    }

    private void LocationSucceded()
    {
        locationEndedWindow.SetActive(true);
        locationEndedHeader.text = "Enhorabuena";
        locationEndedText.text = "Estas un poco más cerca del asesino";
    }

    private void LocationEnded()
    {
        stateMachine.ChangeState(((GameSM)stateMachine).cityState);
    }
}
