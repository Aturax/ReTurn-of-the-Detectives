using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLocation", menuName = "Location")]
public class LocationScriptable : ScriptableObject
{
    public Sprite[] investigatorPortrait;
    public new string name;
    public List<Dice> firstDiceTest;
    public List<Dice> secondDiceTest;
    public List<Dice> thirdDiceTest;
    public Sprite sprite;
}
