using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameOverState : State
{
    [SerializeField] private Image gameOverImage = null;
    [SerializeField] private Sprite winSprite = null;
    [SerializeField] private Sprite looseSprite = null;
    [SerializeField] private Button continueButton = null;

    public GameOverState(GameSM stateMachine) : base(stateMachine) { }

    public async override void Enter()
    {
        continueButton.onClick.AddListener(() => ToMainMenu());

        gameOverImage.gameObject.SetActive(true);
        await _stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound        
    }

    private async void ToMainMenu()
    {
        await _stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.mainMenuState);
    }

    public override void Exit()
    {
        gameOverImage.gameObject.SetActive(false);        
    }

    public void IsWinner(bool winner)
    {
        if (winner) gameOverImage.sprite = winSprite;
        else gameOverImage.sprite = looseSprite;
    }
}
