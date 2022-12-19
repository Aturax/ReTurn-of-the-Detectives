using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDice", menuName = "Dice")]
public class DiceScriptable : ScriptableObject
{
    [SerializeField] private List<Sprite> _faces = null;
    public List<Sprite> Faces => _faces;

    public Sprite GetDiceFace(int faceNumber)
    {
        return Faces[faceNumber];
    }
}
