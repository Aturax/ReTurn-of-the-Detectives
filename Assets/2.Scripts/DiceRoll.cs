using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public enum Dice
{
    Donkey = 0,
    Monocle = 1,
    Monocle2 = Monocle,
    Newspaper = 2,
    BowTie = 3,
    Clover = 4
}

public static class Roll
{
    public static List<Dice> GetRoll(int diceNumber)
    {
        List<Dice> roll = new();
        var dices = Enum.GetValues(typeof(Dice));

        for (int i = 0; i < diceNumber; i++)
        {            
            int index = Random.Range(0, dices.Length);
            Dice dice = (Dice)dices.GetValue(index);
            roll.Add(dice);
        }

        return roll;
    }

    public static bool IsTaskSuccess(Dice[] task, List<Dice> roll)
    {
        foreach (Dice dice in task)
        {
            if (roll.Contains(dice))
                roll.Remove(dice);                
            else return false;
        }
        return true;
    }
}
