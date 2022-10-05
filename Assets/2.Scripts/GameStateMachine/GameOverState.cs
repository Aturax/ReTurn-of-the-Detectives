using UnityEngine;
using UnityEngine.UI;

public class GameOverState : State
{
    [SerializeField] private Image gameOverImage = null;
    [SerializeField] private Sprite winSprite = null;
    [SerializeField] private Sprite looseSprite = null;
    [SerializeField] private Button continueButton = null;    

    private void Awake()
    {
        continueButton.onClick.AddListener(() => ToMainMenu());
    }

    public async override void Enter()
    {
        gameOverImage.gameObject.SetActive(true);
        await stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound        
    }

    private async void ToMainMenu()
    {
        await stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        stateMachine.ChangeState(((GameSM)stateMachine).mainMenuState);
    }

    public override void Exit()
    {
        gameOverImage.gameObject.SetActive(false);        
    }

    public void SetSprite(bool winner)
    {
        if (winner) gameOverImage.sprite = winSprite;
        else gameOverImage.sprite = looseSprite;
    }
}
