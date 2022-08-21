using UnityEngine;
using UnityEngine.UI;

public class GameOverState : State
{
    private Image gameOverImage = null;
    private Sprite winSprite = null;
    private Sprite looseSprite = null;
    private int push = 0;

    public GameOverState(StateMachine stateMachine, Image gameOverImage, Sprite winSprite, Sprite looseSprite) : base(stateMachine)
    {
        this.gameOverImage = gameOverImage;
        this.winSprite = winSprite;
        this.looseSprite = looseSprite;
    }

    public override void Enter()
    {
        gameOverImage.gameObject.SetActive(true);        
    }

    public override void HandleInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            push++;
        }

        if (push == 2) stateMachine.ChangeState(((GameSM)stateMachine).mainMenuState);
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
