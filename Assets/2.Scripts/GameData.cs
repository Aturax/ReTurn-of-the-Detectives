public class GameData
{
    private static GameData _instance = null;
    
    public int DaysLeft { get; private set; } = 10;    
    public int PlayerTurn { get; private set; } = 1;
    public int DicesAvailable { get; private set; } = 6;
    public int DonkeyDices = 0;
    public bool[] TasksStatus { get; private set; } = {false, false, false};
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
        ResetTasks();
        ResetLocations();
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

    public void ResetTasks()
    {
        for (int i = 0; i < TasksStatus.Length; i++)
        {
            TasksStatus[i] = false;
        }
    }

    public void SetTaskSuccess(int index)
    {
        TasksStatus[index] = true;
    }

    public bool IsTaskCompleted(int index)
    {
        return TasksStatus[index];
    }

    public int TasksCompleted()
    {
        int completed = 0;
        for (int i = 0; i < TasksStatus.Length; i++)
        {
            if (TasksStatus[i])
                completed++;
        }
        return completed;
    }

    public void ResetLocations()
    {
        for (int i = 0; i < LocationsStatus.Length; i++)
        {
            LocationsStatus[i] = false;
        }
    }

    public void SetLocationSuccess(int index)
    {
        LocationsStatus[index] = true;
    }

    public bool IsLocationCompleted(int index)
    {
        return LocationsStatus[index];
    }

    public int LocationsCompleted()
    {
        int completed = 0;
        for (int i = 0; i < LocationsStatus.Length; i++)
        {
            if (LocationsStatus[i])
                completed++;
        }
        return completed;
    }

    public void ChangeTurn()
    {
        if (PlayerTurn == 1)
            PlayerTurn = 0;
        else 
            PlayerTurn = 1;
    }

}
