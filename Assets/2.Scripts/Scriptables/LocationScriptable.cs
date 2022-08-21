using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "newLocation", menuName = "Location")]
public class LocationScriptable : ScriptableObject
{
    public new string name;
    public List<Dice> firstDiceTest;
    public List<Dice> secondDiceTest;
    public List<Dice> thirdDiceTest;
    public Sprite sprite;
}
