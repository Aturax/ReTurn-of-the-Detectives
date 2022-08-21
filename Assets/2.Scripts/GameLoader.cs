using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    
    [Header("Locations")]
    public GameObject locationPanel = null;
    public Image locationImage = null;
    public TMP_Text locationLabel = null;
    public List<LocationScriptable> locations = null;
    public TMP_Text destination = null;
    public List<Image> diceRoll = null;
    public Image investigatorImage = null;
    public Button investigateButton = null;

    [Header("Location Test")]
    public List<Image> firstTestImages = null;
    public List<Image> secondTestImages = null;
    public List<Image> thirdTestImages = null;

    [Header("Dice Images")]
    public List<Sprite> diceImages = null;

    public GameSM stateMachine;
    private bool started = false;
    
    private void InitializeStateMachine()
    {
        stateMachine = new GameSM();

        stateMachine.mainMenuState = new MainMenuState(stateMachine, mainMenu);
        
        stateMachine.cityState = new CityState(stateMachine, cityPanel, calendar, askForTravel, locationsPanelButtons,
            locationsNumberButtons, locations, destination, travel, frames);

        stateMachine.locationState = new LocationState(stateMachine, locationPanel, locationImage, locationLabel,
            firstTestImages, secondTestImages, thirdTestImages, diceImages, diceRoll, investigatorImage, investigateButton);
                
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
