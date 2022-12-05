using UnityEngine;
using UnityEngine.UI;

public class MainMenuState : State
{
    [SerializeField] private Button _startButton = null;

    public async override void Enter()
    {
        _startButton.onClick.AddListener(() => StartGame());

        gameObject.SetActive(true);
        await _stateMachine.Fade(0.0f, 1.5f); // TODO: Add sound
        GameData.Instance.ResetData();
    }

    private async void StartGame()
    {
        await _stateMachine.Fade(1.0f, 0.5f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.CityState);
    }

    public override void Exit()
    {
        gameObject.SetActive(false);
    }
}