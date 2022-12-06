using System;
using UnityEngine;

[Serializable]
public class DiceTask
{
    public Dice[] Task = null;
}

[CreateAssetMenu(fileName = "newLocation", menuName = "Location")]
public class LocationScriptable : ScriptableObject
{
    public Sprite[] InvestigatorPortrait = null;
    public string Name = string.Empty;
    public int Number = 0;
    public DiceTask[] DiceTasks = null;
    public Sprite Sprite = null;
    public AudioClip Clip = null;
}
