using System;
using UnityEngine;

[Serializable]
public class DiceTask
{
    public Dice[] Task { get; private set; } = null;
}

[CreateAssetMenu(fileName = "newLocation", menuName = "Location")]
public class LocationScriptable : ScriptableObject
{
    public Sprite[] InvestigatorPortrait { get; private set; } = null;
    public string Name { get; private set; } = string.Empty;
    public int Number { get; private set; } = 0;
    public DiceTask[] DiceTasks { get; private set; } = null;
    public Sprite Sprite { get; private set; } = null;
    public AudioClip Clip { get; private set; } = null;
}
