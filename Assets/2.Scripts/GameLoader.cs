using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TasksImages
{
    public Image[] taskImages;
}

public class GameLoader : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu = null;

    [Header("City Window")]
    public GameObject cityPanel = null;
    public TMP_Text calendar = null;
    public GameObject askForTravel = null;
    public Button[] locationsPanelButtons = null;
    public Button[] locationsNumberButtons = null;
    public Button travel = null;
    public GameObject[] frames = null;
    public GameObject[] completed = null;
    
    [Header("Locations")]
    public GameObject locationPanel = null;
    public Image locationImage = null;
    public TMP_Text locationLabel = null;
    public List<LocationScriptable> locations = null;
    public TMP_Text destination = null;
    public List<Image> diceRoll = null;
    public Image investigatorImage = null;
    public Button investigateButton = null;

    [Header("Location Tasks")]
    public TasksImages[] tasksImages = null;
    public GameObject[] completedTasks = null;
    public GameObject[] tasksIndicators = null;


    [Header("Dice Images")]
    public List<Sprite> diceImages = null;

    public GameSM stateMachine;
    private bool started = false;
    
    private void InitializeStateMachine()
    {
        stateMachine = new GameSM();

        stateMachine.mainMenuState = new MainMenuState(stateMachine, mainMenu);
        
        stateMachine.cityState = new CityState(stateMachine, cityPanel, calendar, askForTravel, locationsPanelButtons,
            locationsNumberButtons, locations, destination, travel, frames, completed);

        stateMachine.locationState = new LocationState(stateMachine, locationPanel, locationImage, locationLabel,
            tasksImages, diceImages, diceRoll, investigatorImage, investigateButton,
            completedTasks, tasksIndicators);
                
        stateMachine.StartStateMachine();
        started = true;
    }


    void Start()
    {
        InitializeStateMachine();        
    }

    void Update()
    {
        if (started)
        {
            stateMachine.HandleInput();
            stateMachine.Update();
            stateMachine.FixedUpdate();
        }
    }
}
