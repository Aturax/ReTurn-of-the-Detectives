using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityState : State
{
    [SerializeField] private AudioSource audiosource = null;
    [SerializeField] private AudioClip cityClip = null;

    [SerializeField] private GameObject cityPanel = null;
    [SerializeField] private TMP_Text calendar = null;
    [SerializeField] private GameObject askForTravel = null;
    [SerializeField] private Button[] locationsPanelButtons = null;
    [SerializeField] private Button[] locationsNumberButtons = null;
    [SerializeField] private Button travel = null;
    [SerializeField] private List<LocationScriptable> locations = null;
    [SerializeField] private TMP_Text destination = null;
    [SerializeField] private GameObject[] characterFrames = null;
    [SerializeField] private GameObject[] completed = null;

    [SerializeField] private GameObject gameOverWindow = null;
    [SerializeField] private TMP_Text gameOverLabel = null;
    [SerializeField] private TMP_Text gameOverText = null;
    [SerializeField] private Button continueButton = null;

    private int locationIndex = 0;

    public override void PreaLoadState()
    {
        for (int i = 0; i < locationsPanelButtons.Length; i++)
        {
            int index = i;
            locationsPanelButtons[i].onClick.AddListener(() => { AskToTravel(index); });
        }

        for (int i = 0; i < locationsNumberButtons.Length; i++)
        {
            int index = i;
            locationsNumberButtons[i].onClick.AddListener(() => { AskToTravel(index); });
        }
        
        travel.onClick.AddListener(() => { TravelTo(locationIndex); });
        continueButton.onClick.AddListener(() => { EndGame(); });
    }

    public async override void Enter()
    {
        audiosource.clip = cityClip;
        audiosource.Play();
        cityPanel.gameObject.SetActive(true);
        CheckTurn();
        CheckCompletedTasks();
        CheckGameOver();
        await stateMachine.Fade(0.0f, 1.0f); // TODO: Add sound
    }

    public override void Exit()
    {
        askForTravel.SetActive(false);
        cityPanel.gameObject.SetActive(false);
        gameOverWindow.SetActive(false);
    }

    private void CheckTurn()
    {
        GameData.Instance.ChangeTurn();
        characterFrames[0].SetActive(GameData.Instance.playerTurn == 1);
        characterFrames[1].SetActive(GameData.Instance.playerTurn == 0);
    }

    private void CheckCompletedTasks()
    {
        for (int i = 0; i < completed.Length; i++)
        {
            bool status = GameData.Instance.IsLocationCompleted(i);
            completed[i].SetActive(status);
            locationsPanelButtons[i].enabled = !status;
            locationsNumberButtons[i].enabled = !status;
        }
    }

    private void CheckGameOver()
    {
        if (GameData.Instance.playerTurn == 0)
        {
            calendar.text = GameData.Instance.DayGone().ToString();
        }
        
        if (GameData.Instance.DaysLeft < 0)
        {
            calendar.text = "0";
            GameOverWindow();
        }

        if (GameData.Instance.LocationsPassed() == 3)
        {
            WinnerWindow();
        }
    }

    private void AskToTravel(int location)
    {        
        askForTravel.SetActive(true);
        locationIndex = location;
        destination.text = locations[location].name;
    }

    private async void TravelTo(int location)
    {
        stateMachine.locationState.GetLocation(locations[location]);
        await stateMachine.Fade(1.0f, 1.0f); // TODO: Add sound
        stateMachine.ChangeState(stateMachine.locationState);
    }

    private void WinnerWindow()
    {
        gameOverWindow.SetActive(true);
        gameOverLabel.text = "Enhorabuena";
        gameOverText.text = "El asesino ha sido atrapado";
        stateMachine.gameOverState.SetSprite(true);
    }

    private void GameOverWindow()
    {
        gameOverWindow.SetActive(true);
        gameOverLabel.text = "Fracasaste";
        gameOverText.text = "El asesino ha conseguido escapar";
        stateMachine.gameOverState.SetSprite(false);
    }

    private void EndGame()
    {
        stateMachine.ChangeState(stateMachine.gameOverState);
    }
}
