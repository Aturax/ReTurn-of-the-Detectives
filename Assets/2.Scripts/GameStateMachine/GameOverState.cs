using UnityEngine;
using UnityEngine.UI;

public class GameOverState : State
{
    [SerializeField] private Image _gameOverImage = null;
    [SerializeField] private Sprite _winSprite = null;
    [SerializeField] private Sprite _looseSprite = null;
    [SerializeField] private Button _continueButton = null;    

    public async override void Enter()
    {
        _continueButton.onClick.AddListener(() => ToMainMenu());

        _gameOverImage.gameObject.SetActive(true);
        await _stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }

    private async void ToMainMenu()
    {
        await _stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        _stateMachine.ChangeState(_stateMachine.MainMenuState);
    }

    public override void Exit()
    {
        _gameOverImage.gameObject.SetActive(false);        
    }

    public void IsWinner(bool winner)
    {
        if (winner)
            _gameOverImage.sprite = _winSprite;
        else
            _gameOverImage.sprite = _looseSprite;
    }
}
