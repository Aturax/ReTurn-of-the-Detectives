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
    [field: SerializeField] public Sprite[] InvestigatorPortrait { get; private set; } = null;
    [field: SerializeField] public string Name { get; private set; } = string.Empty;
    [field: SerializeField] public int Number { get; private set; } = 0;
    [field: SerializeField] public DiceTask[] DiceTasks { get; private set; } = null;
    [field: SerializeField] public Sprite Sprite { get; private set; } = null;
    [field: SerializeField] public AudioClip Clip { get; private set; } = null;
    [field: SerializeField, TextArea] public string Description { get; private set; } = null;
}
