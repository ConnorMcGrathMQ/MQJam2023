using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant Object", menuName = "ScriptableObjects/Plant")]
public class PlantObject : ScriptableObject
{
    public PlantType type;
    public int maxLength;
    public List<ColorPair> colorList;
}
[System.Serializable]
public struct ColorPair {
    public Color vineColour;
    public Color highlightColour;
}