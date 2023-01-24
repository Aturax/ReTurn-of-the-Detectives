using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Newspaper : MonoBehaviour
{
    [SerializeField] private TMP_Text _locationText = null;
    [SerializeField] private Image _locationImage = null;
    [SerializeField] private TMP_Text _locationDescription = null;
    [SerializeField] private Image _investigatorPortrait = null;

    public void LoadData(LocationScriptable location, Sprite investigator)
    {
        _locationText.text = location.name;
        _locationImage.sprite = location.Sprite;
        _locationDescription.text = location.Description;
        _investigatorPortrait.sprite = investigator;
    }
}
