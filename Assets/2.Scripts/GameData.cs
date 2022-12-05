public class GameData
{
    private static GameData _instance = null;
    
    public int DaysLeft { get; private set; } = 10;    
    public int PlayerTurn { get; private set; } = 0;
    public int DicesAvailable { get; private set; } = 6;
    public int DonkeyDices = 0;
    public bool[] TaskPassed { get; private set; } = {false, false, false};
    public bool[] LocationsStatus { get; private set; } = { false, false, false };    

    public static GameData Instance
    {
        get
        {
            _instance ??= new GameData();
            return _instance;
        }
    }

    public void ResetData()
    {
        DaysLeft = 10;
        PlayerTurn = 1;
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
        DicesAvailable = 6;
        DonkeyDices = 0;
    }

    public void LoseDice()
    {
        if (DicesAvailable > 0)
            DicesAvailable--;
    }

    public void RecoverDice()
    {
        if (DicesAvailable < 6)
            DicesAvailable++;
    }

    public void RecoverDices()
    {
        DicesAvailable = 6;
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
        for (int i = 0; i < LocationsStatus.Length; i++)
        {
            LocationsStatus[i] = false;
        }
    }

    public void SetLocationPassed(int index)
    {
        LocationsStatus[index] = true;
    }

    public bool IsLocationCompleted(int index)
    {
        return LocationsStatus[index];
    }

    public int LocationsCompleted()
    {
        int passed = 0;
        for (int i = 0; i < LocationsStatus.Length; i++)
        {
            if (LocationsStatus[i])
                passed++;
        }
        return passed;
    }

    public void ChangeTurn()
    {
        if (PlayerTurn == 1)
            PlayerTurn = 0;
        else PlayerTurn = 1;
    }

}
