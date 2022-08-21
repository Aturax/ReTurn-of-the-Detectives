using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            diceRoll.Add((Dice)Random.Range(0, System.Enum.GetValues(typeof(Dice)).Length - 1));            
        }

        return diceRoll;
    }

    public static bool CheckDiceTest(List<Dice> test, List<Dice> diceRoll)
    {
        foreach (Dice dice in test)
        {
            if (diceRoll.Contains(dice))
            {
                diceRoll.Remove(dice);                
            }
            else return false;
        }
        return true;
    }
}
