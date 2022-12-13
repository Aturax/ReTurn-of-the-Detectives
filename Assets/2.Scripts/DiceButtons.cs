using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DiceButtons : MonoBehaviour
{
    [SerializeField] private List<Sprite> _diceFaces = null;
    [SerializeField] private Button[] _diceButtons = null;
    private bool[] _dicesSelected = null;

    private void Awake()
    {
        _dicesSelected = new bool[_diceButtons.Length];
        
        ResetDices();        
        DisableDiceButtons();        

        for (int i = 0; i < _diceButtons.Length; i++)
        {
            int index = i;
            _dicesSelected[i] = true;
            _diceButtons[index].onClick.AddListener(() => { SelectDiceToRoll(index); });
        }
    }    

    public void ResetDices()
    {
        int index = 0;

        foreach (Dice diceValue in Enum.GetValues(typeof(Dice)))
        {
            _diceButtons[index].image.sprite = _diceFaces[(int)diceValue];
            _diceButtons[index].gameObject.SetActive(true);
            index++;
        }
        GameData.Instance.RecoverDices();
    }

    public void DisableDiceButtons()
    {
        foreach(Button button in _diceButtons)
        {
            button.enabled = false;
        }

        for (int i = 0; i < _diceButtons.Length; i++)
        {
            if (_diceButtons[i].image.color.a != 0.0f)
                _diceButtons[i].enabled = false;
            _dicesSelected[i] = true;
        }
    }

    public void EnableDiceButtons()
    {
        for(int i = 0; i < _diceButtons.Length; i++)
        {
            if (_diceButtons[i].image.color.a != 0.0f)
                _diceButtons[i].enabled = true;
            _dicesSelected[i] = false;
        }
    }

    private void SelectDiceToRoll(int diceIndex)
    {
        _dicesSelected[diceIndex] = !_dicesSelected[diceIndex];
    }

    public void SelectDiceToRoll(int diceIndex, bool status)
    {
        _dicesSelected[diceIndex] = status;
    }

    public int GetNumberOfDicesToRoll()
    {
        int dices = 0;

        for (int i = 0; i < _dicesSelected.Length; i++)
        {
            if (_dicesSelected[i])
                dices++;
        }

        return dices;
    }

    public bool IsDiceSelected(int diceIndex)
    {
        return _dicesSelected[diceIndex];
    }

    public void SetDiceImage(int diceIndex, int faceNumber)
    {
        _diceButtons[diceIndex].image.sprite = _diceFaces[faceNumber];
    }

    public Sprite GetDiceFace(int faceNumber)
    {
        return _diceFaces[faceNumber];
    }

    public Dice GetDiceOfButton(int diceIndex)
    {
        return (Dice)_diceFaces.IndexOf(_diceButtons[diceIndex].image.sprite);
    }

    public int GetDiceButtonsLength()
    {
        return _diceButtons.Length;
    }

    public async Task ShowDiceButton(int index)
    {
        _diceButtons[index].gameObject.SetActive(true);
        FadeDice(index, 1.0f, 0.3f);
        await Task.Delay(500);
    }

    public async Task FadeDiceButtons(float alpha)
    {
        int delay = (alpha == 0.0f) ? 150 : 500;

        for (int i = 0; i < _diceButtons.Length; i++)
        {
            if (_dicesSelected[i])
            {
                FadeDice(i, alpha, 0.3f);
                await Task.Delay(delay);
            }
        }

        await Task.Delay(500);
    }

    public void FadeDice(int diceIndex, float alpha, float time)
    {
        _diceButtons[diceIndex].image.CrossFadeAlpha(alpha, time, false);
    }

}
