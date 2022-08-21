using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocationState : State
{
    private GameObject locationPanel = null;
    private LocationScriptable location = null;
    private Image locationImage = null;
    private TMP_Text locationLabel = null;
    private List<Image> firstTestImages = null;
    private List<Image> secondTestImages = null;
    private List<Image> thirdTestImages = null;
    private List<Sprite> diceImages = null;

    public LocationState(GameLoader gameManager, StateMachine stateMachine, GameObject locationPanel, Image locationImage, TMP_Text locationLabel,
        List<Image> first, List<Image> second, List<Image> third, List<Sprite> dice) : base(gameManager, stateMachine)
    {
        this.locationPanel = locationPanel;
        this.locationImage = locationImage;
        this.locationLabel = locationLabel;
        
        firstTestImages = first;
        secondTestImages = second;
        thirdTestImages = third;

        diceImages = dice;
    }

    public override void Enter()
    {
        locationPanel.gameObject.SetActive(true);
        FillTests();
    }
    public override void HandleInput() { }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        locationPanel.gameObject.SetActive(false);
    }

    private void FillTests()
    {
        ShowDiceTest(firstTestImages, location.firstDiceTest);
        ShowDiceTest(secondTestImages, location.secondDiceTest);
        ShowDiceTest(thirdTestImages, location.thirdDiceTest);
    }

    private void ShowDiceTest(List<Image> images, List<Dice> test)
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (test.Count > i)
            {
                images[i].gameObject.SetActive(true);
                images[i].sprite = diceImages[(int)test[i]];
            }
            else images[i].gameObject.SetActive(false);            
        }
    }

    public void GetLocation(LocationScriptable location)
    {
        this.location = location;
        locationLabel.text = location.name;
    }
}
