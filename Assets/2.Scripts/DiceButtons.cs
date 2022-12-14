using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DiceButtons : MonoBehaviour
{
    [SerializeField] private DiceScriptable _dice = null;
    [SerializeField] private Button[] _diceButtons = null;
    private bool[] _dicesSelected = null;

    private void Awake()
    {
        _dicesSelected = new bool[_diceButtons.Length];

        for (int i = 0; i < _diceButtons.Length; i++)
        {
            int index = i;
            _diceButtons[index].onClick.AddListener(() => { SelectDiceToRoll(index); });
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _diceButtons.Length; i++)
        {
            _diceButtons[i].onClick.RemoveAllListeners();
        }
    }

    public void ResetDices()
    {
        int index = 0;

        foreach (Dice diceValue in Enum.GetValues(typeof(Dice)))
        {
            _diceButtons[index].image.sprite = _dice.Faces[(int)diceValue];
            _diceButtons[index].gameObject.SetActive(true);
            _dicesSelected[index] = true;
            index++;
        }
        GameData.Instance.RecoverDices();
        DisableButtons();
    }

    public void DisableButtons()
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

    public void EnableButtons()
    {
        for(int i = 0; i < _diceButtons.Length; i++)
        {
            if (_diceButtons[i].image.color.a != 0.0f)
                _diceButtons[i].enabled = true;
            _dicesSelected[i] = false;
        }
    }

    public void FillWithRoll(List<Dice> roll)
    {
        for (int i = 0; i < _diceButtons.Length; i++)
        {
            if (i > GameData.Instance.DicesAvailable - 1)
            {
                FadeDice(i, 0.0f, 0.0f);
                SelectDiceToRoll(i, false);
                continue;
            }

            if (IsDiceSelected(i))
                SetDiceImage(i, (int)roll[i]);
            else
                roll[i] = GetSpriteOnButton(i);
        }
    }

    public async Task FadeButtons(float alpha)
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

    private void SelectDiceToRoll(int diceIndex)
    {
        _dicesSelected[diceIndex] = !_dicesSelected[diceIndex];
    }

    private void SelectDiceToRoll(int diceIndex, bool status)
    {
        _dicesSelected[diceIndex] = status;
    }

    private bool IsDiceSelected(int diceIndex)
    {
        return _dicesSelected[diceIndex];
    }

    private void SetDiceImage(int diceIndex, int faceNumber)
    {
        _diceButtons[diceIndex].image.sprite = _dice.Faces[faceNumber];
    }

    private Dice GetSpriteOnButton(int diceIndex)
    {
        return (Dice)_dice.Faces.IndexOf(_diceButtons[diceIndex].image.sprite);
    }

    private void FadeDice(int diceIndex, float alpha, float time)
    {
        _diceButtons[diceIndex].image.CrossFadeAlpha(alpha, time, false);
    }

}
