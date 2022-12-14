using UnityEngine;
using UnityEngine.UI;

public class LocationTasks : MonoBehaviour
{
    [SerializeField] private DiceScriptable _dice = null;
    [SerializeField] private TasksImages[] _tasksImages = null;
    [SerializeField] private GameObject[] _completedTasksSeal = null;
    [SerializeField] private Button[] _taskIndicatorButtons = null;

    public int SelectedTaskIndex { get; set; } = 0;
    public bool TaskSelected { get; set; } = false;

    private void Awake()
    {
        for (int i = 0; i < _taskIndicatorButtons.Length; i++)
        {
            int number = i;
            _taskIndicatorButtons[i].onClick.AddListener(() => { SelectTask(number); });
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _taskIndicatorButtons.Length; i++)
        {
            _taskIndicatorButtons[i].onClick.RemoveAllListeners();
        }
    }

    public void LoadTasks(LocationScriptable location)
    {
        SelectedTaskIndex = 0;
        TaskSelected = false;

        GameData.Instance.ResetTasks();

        for (int i = 0; i < location.DiceTasks.Length; i++)
        {
            FillTaskWithObjectives(_tasksImages[i].TaskImages, location.DiceTasks[i].Task);
        }
    }

    public void ResetIndicators()
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

    public void ActiveCompleteTaskSeal()
    {
        _completedTasksSeal[SelectedTaskIndex].SetActive(true);
    }

    public void ShowNotCompletedTasksIndicators()
    {
        for (int i = 0; i < _taskIndicatorButtons.Length; i++)
        {
            _taskIndicatorButtons[i].gameObject.SetActive(!GameData.Instance.IsTaskCompleted(i));
        }
    }

    public void SelectTask(int index = -1)
    {
        if (index == -1)
            index = SelectedTaskIndex;

        for (int i = 0; i < _taskIndicatorButtons.Length; i++)
        {
            _taskIndicatorButtons[i].gameObject.SetActive(i == index);
        }
        SelectedTaskIndex = index;
        TaskSelected = true;
    }

    public void SelectLastTask()
    {
        if (GameData.Instance.TasksCompleted() == 2)
        {
            for (int i = 0; i < GameData.Instance.TasksStatus.Length; i++)
            {
                if (!GameData.Instance.IsTaskCompleted(i))
                {
                    SelectTask(i);
                    return;
                }
            }
        }
    }
    
    private void FillTaskWithObjectives(Image[] images, Dice[] task)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (task.Length > i)
            {
                images[i].gameObject.SetActive(true);
                images[i].sprite = _dice.GetDiceFace((int)task[i]);
            }
            else
            {
                images[i].gameObject.SetActive(false);
            }
        }
    }
}
