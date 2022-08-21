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
    public AudioSource audiosource = null;

    [Header("Main Menu")]
    public GameObject mainMenu = null;

    [Header("City Window")]
    public AudioClip cityClip = null;
    public GameObject cityPanel = null;
    public TMP_Text calendar = null;
    public GameObject askForTravel = null;
    public Button[] locationsPanelButtons = null;
    public Button[] locationsNumberButtons = null;
    public Button travel = null;
    public GameObject[] frames = null;
    public GameObject[] completed = null;
    public GameObject gameOverWindow = null;
    public TMP_Text gameOverLabel = null;
    public TMP_Text gameOverText = null;
    public Button continueButton = null;

    [Header("Locations")]
    public GameObject locationPanel = null;
    public Image locationImage = null;
    public TMP_Text locationLabel = null;
    public List<LocationScriptable> locations = null;
    public TMP_Text destination = null;
    public List<Image> diceRoll = null;
    public Image investigatorImage = null;
    public Button investigateButton = null;
    public GameObject locationEndedWindow = null;
    public TMP_Text locationEndedHeader = null;
    public TMP_Text locationEndedText = null;
    public Button returnToCityButton = null;

    [Header("Location Tasks")]
    public TasksImages[] tasksImages = null;
    public GameObject[] completedTasks = null;
    public GameObject[] tasksIndicators = null;

    [Header("Dice Images")]
    public List<Sprite> diceImages = null;

    [Header("Game Over")]
    public Image gameOverImage = null;
    public Sprite winSprite = null;
    public Sprite looseSprite = null;

    public GameSM stateMachine;
    private bool started = false;
    
    private void InitializeStateMachine()
    {
        stateMachine = new GameSM();

        stateMachine.mainMenuState = new MainMenuState(stateMachine, mainMenu);
        
        stateMachine.cityState = new CityState(stateMachine, audiosource, cityClip, cityPanel, calendar, askForTravel, locationsPanelButtons,
            locationsNumberButtons, locations, destination, travel, frames, completed, gameOverWindow, gameOverLabel,
            gameOverText, continueButton);

        stateMachine.locationState = new LocationState(stateMachine, audiosource, locationPanel, locationImage, locationLabel,
            tasksImages, diceImages, diceRoll, investigatorImage, investigateButton, completedTasks, tasksIndicators,
            locationEndedWindow, locationEndedHeader, locationEndedText, returnToCityButton);

        stateMachine.gameOverState = new GameOverState(stateMachine, gameOverImage, winSprite, looseSprite);
                
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
