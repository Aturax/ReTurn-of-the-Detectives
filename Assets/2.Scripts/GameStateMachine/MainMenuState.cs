using UnityEngine;
using UnityEngine.UI;

public class MainMenuState : State
{
    [SerializeField] private GameObject mainMenu = null;
    [SerializeField] private Button startButton = null;    

    private void Awake()
    {
        startButton.onClick.AddListener(() => StartGame());
    }

    public async override void Enter()
    {
        mainMenu.SetActive(true);
        await stateMachine.Fade(0.0f, 1.5f); // TODO: Add sound
        GameData.Instance.ResetData();
    }

    private async void StartGame()
    {
        await stateMachine.Fade(1.0f, 0.5f); // TODO: Add sound
        stateMachine.ChangeState(((GameSM)stateMachine).cityState);
    }

    public override void Exit()
    {
        mainMenu.SetActive(false);        
    }
}