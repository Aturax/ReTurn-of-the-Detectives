using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MainMenuState : State
{
    [SerializeField] private GameObject mainMenu = null;
    [SerializeField] private Button startButton = null;

    public async override void Enter()
    {
        startButton.onClick.AddListener(() => StartGame());

        mainMenu.SetActive(true);
        await stateMachine.Fade(0.0f, 1.5f); // TODO: Add sound
        GameData.Instance.ResetData();
    }

    private async void StartGame()
    {
        await stateMachine.Fade(1.0f, 0.5f); // TODO: Add sound
        stateMachine.ChangeState(stateMachine.cityState);
    }

    public override void Exit()
    {
        mainMenu.SetActive(false);        
    }
}