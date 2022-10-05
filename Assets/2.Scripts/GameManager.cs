using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("State Machine")]
    [SerializeField] private MainMenuState mainMenuState = null;
    [SerializeField] private CityState cityState = null;
    [SerializeField] private LocationState locationState = null;
    [SerializeField] private GameOverState gameOverState = null;
    private GameSM stateMachine = null;

    [Space]
    [SerializeField] Fader fader = null;

    void Start()
    {
        stateMachine = new GameSM();
        
        stateMachine.SetFader(fader);

        mainMenuState.LoadStateMachine(stateMachine);
        cityState.LoadStateMachine(stateMachine);
        locationState.LoadStateMachine(stateMachine);
        gameOverState.LoadStateMachine(stateMachine);

        stateMachine.mainMenuState = mainMenuState;
        stateMachine.cityState = cityState;
        stateMachine.locationState = locationState;
        stateMachine.gameOverState = gameOverState;

        stateMachine.StartStateMachine();
    }
}
