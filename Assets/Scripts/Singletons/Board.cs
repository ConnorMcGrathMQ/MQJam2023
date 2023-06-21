using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class Board : MonoBehaviour
{
    private static Board instance;
    public static Board Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private Grid grid;
    public static int GridSize = 15;
    public Tile[,] tiles = new Tile[GridSize,GridSize];

    void Awake() {
        if(Instance == null) {
            grid = GetComponent<Grid>();
            Instance = this;
        } else {
            Debug.Log("Duplicate boards exist! Destroying...");
        }

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
            newTile = Instantiate(tilePrefab, grid.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0)), 
                Quaternion.identity, this.transform);
            old = location;
            location = newTile;
        }
        if (old != null) {
            Destroy(old.gameObject);
        }
    }

    public Tile GetTile(int x, int y) {
        return tiles[x, y];
    }
    
    public Tile GetTile(Vector2Int target) {
        return tiles[target.x, target.y];
    }

    public Tile GetAdjacentTile(Dir dir, Tile tile) {
        switch (dir) {
            case Dir.Up :
                if(tile.pos.y >= tiles.GetLength(1)) {
                    return null; // out of bounds
                }
                return tiles[tile.pos.x, tile.pos.y + 1];
            case Dir.Left :
                if(tile.pos.x <= 0) {
                    return null; // out of bounds
                }
                return tiles[tile.pos.x - 1, tile.pos.y];

            case Dir.Down :
                if(tile.pos.y <= 0) {
                    return null; // out of bounds
                }
                return tiles[tile.pos.x, tile.pos.y - 1];

            case Dir.Right :
                if(tile.pos.x >= tiles.GetLength(0)) {
                    return null; // out of bounds
                }
                return tiles[tile.pos.x - 1, tile.pos.y];

            default :
                Debug.LogWarning("Invalid Direction passed");
                return null;
        }
    }

    public Tile[] GetAllAdjacentTiles(Tile tile) {
        Tile[] result = new Tile[4];
        for(int i = 0; i<4; i++) {
            result[i] = GetAdjacentTile((Dir)i, tile);
        }
        return result;
    }
}