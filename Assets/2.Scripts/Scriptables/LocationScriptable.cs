using UnityEngine;

[System.Serializable]
public class DiceTask
{
    public Dice[] task;
}

[CreateAssetMenu(fileName = "newLocation", menuName = "Location")]
public class LocationScriptable : ScriptableObject
{
    public Sprite[] investigatorPortrait;
    public new string name;
    public int number;
    public DiceTask[] diceTasks;
    public Sprite sprite;
    public AudioClip clip;
}
