using UnityEngine;

public class PlantPoint: Tile
{
    private bool connected;
    private int distance;
    public PlantObject species;

    void Start() {
        connected = false;
        distance = 0;
    }

    public void Connect(int remainingDist, int maxDist) {
        connected = true;
        distance = maxDist - remainingDist;
    }

    // void OnDrawGizmos() {
    //     Gizmos.color = new Color(0, 1, 0, 0.5f);
    //     Gizmos.DrawSphere(Board.Instance.GetTile(pos).transform.position, 0.25f);
    // }
}