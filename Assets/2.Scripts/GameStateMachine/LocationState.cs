using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TasksImages
{
    public Image[] taskImages;
}

public class LocationState : State
{
    [SerializeField] private AudioSource audiosource = null;
    [SerializeField] private GameObject locationPanel = null;
    [SerializeField] private Image paperLocationImage = null;
    [SerializeField] private TMP_Text paperLocationLabel = null;
    [SerializeField] private TasksImages[] tasksImages = null;
    [SerializeField] private List<Sprite> diceImages = null;
    [SerializeField] private List<Image> diceRoll = null;
    [SerializeField] private Image investigatorPortrait = null;
    [SerializeField] private Button investigateButton = null;
    [SerializeField] private GameObject[] completedTasks = null;
    [SerializeField] private GameObject[] tasksIndicators = null;

    private LocationScriptable location = null;
    private int selectedTask = 0;

    [SerializeField] private GameObject locationEndedWindow = null;
    [SerializeField] private TMP_Text locationEndedHeader = null;
    [SerializeField] private TMP_Text locationEndedText = null;
    [SerializeField] private Button continueButton = null;

    public override void PreaLoadState()
    {        
        investigateButton.onClick.AddListener(() => { RollDices(); });
        continueButton.onClick.AddListener(() => { LocationEnded(); });
    }

    public async override void Enter()
    {
        audiosource.clip = location.clip;
        audiosource.Play();
        locationPanel.gameObject.SetActive(true);
        investigatorPortrait.sprite = location.investigatorPortrait[GameData.Instance.playerTurn];
        selectedTask = 0;
        ResetDices();
        FillTasks();
        ResetIndicators();
        SelectTask();
        await stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }
    
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
        paperLocationImage.sprite = location.sprite;
        paperLocationLabel.text = location.name;
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

    private async void LocationEnded()
    {
        await stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        stateMachine.ChangeState(((GameSM)stateMachine).cityState);
    }
}
