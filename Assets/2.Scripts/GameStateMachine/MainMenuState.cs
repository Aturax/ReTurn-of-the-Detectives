using UnityEngine;
using UnityEngine.UI;

public class MainMenuState : State
{
    [SerializeField] private Button _startButton = null;

    private void Awake()
    {
        _startButton.onClick.AddListener(() => StartGame());
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveAllListeners();
    }

    public async override void Enter()
    {
        gameObject.SetActive(true);
        await _stateMachine.Fade(0.0f, 1.5f); // TODO: Add sound        
    }

    private async void StartGame()
    {
        await _stateMachine.Fade(1.0f, 0.5f); // TODO: Add sound
        GameData.Instance.ResetData();
        _stateMachine.ChangeState(_stateMachine.CityState);
    }

    public override void Exit()
    {
        gameObject.SetActive(false);
    }
}