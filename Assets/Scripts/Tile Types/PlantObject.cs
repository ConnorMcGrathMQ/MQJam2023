using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Plant Object", menuName = "Recomposition/Plant")]
public class PlantObject : ScriptableObject
{
    public PlantType type;
    public int maxLength;
    public int effectSpace;
    public Sprite straightSprite;
    public Sprite cornerSprite;
    public Sprite startSprite;
    public Sprite endSprite;
    public Material particleMaterial;
    // public Sprite endPointSprite;
    public List<ColorPair> colorList;
}
[System.Serializable]
public struct ColorPair {
    public Color vineColour;
    public Color highlightColour;
}