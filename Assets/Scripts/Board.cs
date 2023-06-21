using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class Board : MonoBehaviour
{
    private Grid grid;
    public static int GridSize = 15;
    public Tile[,] tiles = new Tile[GridSize,GridSize];

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTile(Tile tilePrefab, Vector2Int pos) {
        AddTile(tilePrefab, pos, false);
    }

    public void AddTile(Tile tilePrefab, Vector2Int pos, bool replace) {
        Tile location = tiles[pos.x,pos.y];
        Tile old = null;
        Tile newTile = null;
        if (location != null && replace == false) {
            return;
        } else {
            newTile = Instantiate(tilePrefab, grid.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0)), Quaternion.identity, this.transform);
            old = location;
            location = newTile;
        }
        if (old != null) {
            Destroy(old.gameObject);
        }
    }
}