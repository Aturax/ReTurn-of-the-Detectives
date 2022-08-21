using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private static GameData instance = null;
    
    public int DaysLeft { get; private set; } = 10;    
    public int playerTurn { get; private set; } = 1;
    public int dicesAvailable { get; private set; } = 6;
    public int donkeyDices = 0;
    public bool[] testPassed { get; private set; } = {false, false, false};
    public bool[] locationsPassed { get; private set; } = { false, false, false };

    private GameData() { }

    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameData();
            }
            return instance;
        }
    }

    public void ResetData()
    {
        DaysLeft = 10;
        playerTurn = 1;
        ResetDices();
        ResetTestPassed();
        ResetLocationsPased();
    }

    public int DayGone()
    {
        return --DaysLeft;
    }

    public void ResetDices()
    {
        dicesAvailable = 6;
        donkeyDices = 0;
    }

    public void RecoverDices()
    {
        dicesAvailable += 6 - donkeyDices;         
    }

    public void ResetTestPassed()
    {
        for (int i = 0; i < testPassed.Length; i++)
        {
            testPassed[i] = false;
        }
    }

    public int GetTestPassed()
    {
        int passed = 0;
        for (int i = 0; i < testPassed.Length; i++)
        {
            if (testPassed[i]) passed++;
        }
        return passed;
    }

    public void ResetLocationsPased()
    {
        for (int i = 0; i < locationsPassed.Length; i++)
        {
            locationsPassed[i] = false;
        }
    }

    public int GetLocationsPassed()
    {
        int passed = 0;
        for (int i = 0; i < locationsPassed.Length; i++)
        {
            if (locationsPassed[i]) passed++;
        }
        return passed;
    }

    public void ChangeTurn()
    {
        if (playerTurn == 1) playerTurn = 0;
        else playerTurn = 1;
    }

}
