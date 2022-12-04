using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MainMenuState : State
{
    [SerializeField] private GameObject mainMenu = null;
    [SerializeField] private Button startButton = null;

    public MainMenuState(GameSM stateMachine) : base(stateMachine) { }

    public async override void Enter()
    {
        startButton.onClick.AddListener(() => StartGame());

        mainMenu.SetActive(true);
        await _stateMachine.Fade(0.0f, 1.5f); // TODO: Add sound
        GameData.Instance.ResetData();
    }

    private async void StartGame()
    {
        await _stateMachine.Fade(1.0f, 0.5f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.cityState);
    }

    public override void Exit()
    {
        mainMenu.SetActive(false);        
    }
}