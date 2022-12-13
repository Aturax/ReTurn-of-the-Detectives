using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Newspaper : MonoBehaviour
{
    [SerializeField] private TMP_Text _locationText = null;
    [SerializeField] private Image _locationImage = null;
    [SerializeField] private Image _investigatorPortrait = null;

    public void LoadData(string location, Sprite locationSprite, Sprite investigatorSprite)
    {
        _locationText.text = location;
        _locationImage.sprite = locationSprite;
        _investigatorPortrait.sprite = investigatorSprite;
    }
}
