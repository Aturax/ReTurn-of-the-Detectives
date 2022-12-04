using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Dice
{
    Donkey = 0,
    Monocle = 1,
    Monocle2 = Monocle,
    Newspaper = 2,
    BowTie = 3,
    Clover = 4
}

public static class DiceRoll
{
    public static List<Dice> GetDiceRoll(int diceNumber)
    {
        List<Dice> diceRoll = new List<Dice>();        

        for (int i = 0; i < diceNumber; i++)
        {            
            var dices = System.Enum.GetValues(typeof(Dice));
            int index = Random.Range(0, dices.Length);
            Dice dice = (Dice)dices.GetValue(index);
            diceRoll.Add(dice);
        }

        return diceRoll;
    }

    public static bool CheckDiceTask(Dice[] task, List<Dice> diceRoll)
    {
        foreach (Dice dice in task)
        {
            if (diceRoll.Contains(dice))
            {
                diceRoll.Remove(dice);                
            }
            else return false;
        }
        return true;
    }

    public static void FadeDice(Image diceImage, float alpha, float time)
    {
        diceImage.CrossFadeAlpha(alpha, time, false);
    }
}
