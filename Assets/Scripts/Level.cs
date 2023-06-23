using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Recomposition/Level")]
public class Level : ScriptableObject
{
    public List<Objective> objectives;
    public List<Vector2Int> obstacles;

    public Objective FindPair(Vector2Int objectivePosition) {
        foreach (Objective o in objectives) {
            if(o.firstPointPosition.Equals(objectivePosition)) {
                return o;
            }
        } 
        return new Objective(); // failed to find pair, return empty
    }
}

[System.Serializable]
public struct Objective {
    public Vector2Int firstPointPosition;
    public Vector2Int secondPointPosition;
    public Color pairColour;
    public PlantObject species;
}