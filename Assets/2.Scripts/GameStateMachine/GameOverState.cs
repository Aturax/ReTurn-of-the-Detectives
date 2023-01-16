using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GameOverState : State
{
    [SerializeField] private GameObject _winScreen = null;
    [SerializeField] private GameObject _looseScreen = null;
    [SerializeField] private Button _continueButton = null;

    private bool _winner = false;

    public async override void Enter()
    {
        _continueButton.onClick.AddListener(() => ToMainMenu());
        
        _winScreen.SetActive(_winner);
        _looseScreen.SetActive(!_winner);

        await _stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }

    private async void ToMainMenu()
    {
        await _stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.MainMenuState);
    }

    public override void Exit()
    {
        _winScreen.SetActive(false);
        _looseScreen.SetActive(false);
    }

    public void IsWinner(bool winner)
    {
        _winner = winner;        
    }
}
