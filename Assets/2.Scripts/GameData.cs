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
    public bool[] TaskPassed { get; private set; } = {false, false, false};
    public bool[] locationsPassed { get; private set; } = { false, true, false };

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
        ResetTaskPassed();
        ResetLocationsPassed();
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

    public void LoseDice()
    {
        if (dicesAvailable > 0) dicesAvailable--;
    }

    public void RecoverDices()
    {
        dicesAvailable = 6;
    }

    public void ResetTaskPassed()
    {
        for (int i = 0; i < TaskPassed.Length; i++)
        {
            TaskPassed[i] = false;
        }
    }

    public void SetTaskPassed(int index)
    {
        TaskPassed[index] = true;
    }

    public void ResetLocationsPassed()
    {
        for (int i = 0; i < locationsPassed.Length; i++)
        {
            locationsPassed[i] = false;
        }
    }

    public void SetLocationPassed(int index)
    {
        locationsPassed[index] = true;
    }

    public bool IsLocationCompleted(int index)
    {
        return locationsPassed[index];
    }

    public int LocationsPassed()
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
