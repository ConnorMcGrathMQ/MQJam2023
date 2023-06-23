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
    public List<Color> vineColours;
}