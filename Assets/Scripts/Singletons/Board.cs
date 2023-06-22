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
    public int width = 15;
    public int height = 15;
    public Tile[,] tiles;
    public Tile emptyTilePrefab;
    public PlantPoint pointPrefab;
    public Tile obstaclePrefab;
    public Vector2Int startPoint;
    public Vector2Int endPoint;

    public List<Level> levels;
    private int currentLevel;

    private int pairsComplete;

    void Awake() {
        if(Instance == null) {
            grid = GetComponent<Grid>();
            tiles = new Tile[width, height];
            Instance = this;
            LoadLevel();
        } else {
            Debug.Log("Duplicate boards exist! Destroying...");
        }

    }

    public void LoadLevel(int level) {
        currentLevel = level;
        LoadLevel();
    }

    public void LoadLevel() {
        Level level = levels[currentLevel];
        for(int x = 0; x< width; x++) {
            for(int y = 0; y<height; y++) {
                Vector2Int thisPos = new Vector2Int(x, y);
                if(TileIsPoint(x, y, level)) {
                    AddTile(pointPrefab, thisPos);
                } else if (level.obstacles.Contains(thisPos)){
                    AddTile(obstaclePrefab, thisPos);
                } else{
                    AddTile(emptyTilePrefab, thisPos);
                }

            }
        }
    }

    public bool TileIsPoint(int x, int y, Level level) {
        foreach (Objective o in level.objectives)
        {
            if((x == o.firstPointPosition.x && y == o.firstPointPosition.y) || (x == o.secondPointPosition.x && y == o.secondPointPosition.y)) {
                return true;
            }
        }
        return false;
    }

    public void AddTile(Tile tilePrefab, Vector2Int pos) {
        AddTile(tilePrefab, pos, false);
    }

    public void AddTile(Tile tilePrefab, Vector2Int pos, bool replace) {
        Tile location = tiles[pos.x,pos.y];
        Tile old = null;
        Tile newTile = null;
        if (location != null && !(location is Empty) && replace == false) {
            // Debug.Log($"{location.ToString()} Is Null! Hahah");
            return;
        } else {
            newTile = Instantiate(tilePrefab, grid.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0)), 
                Quaternion.identity, this.transform);
            old = location;
            tiles[pos.x, pos.y] = newTile;
            newTile.pos = pos;
            newTile.gameObject.name = newTile.ToString();
            if(newTile is PlantPoint point) {
                point.FindPartner(levels[currentLevel]);
            }
        }
        if (old != null) {
            Destroy(old.gameObject);
        } else if (old != null && !(old is Empty)) {
            Destroy(old.gameObject);
        }
    }

    public Tile GetTile(int x, int y) {
        return tiles[x, y];
    }
    
    public Tile GetTile(Vector2Int target) {
        return tiles[target.x, target.y];
    }

    public Tile GetTile(Vector3Int target) {
        return tiles[target.x, target.y];
    }

    public Tile GetAdjacentTile(Dir dir, Tile tile) {
        Debug.Log($"Checking {tile.ToString()}'s Adjacent at {dir}");
        switch (dir) {
            case Dir.Up :
                if(tile.pos.y + 1 >= tiles.GetLength(1)) {
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
                if(tile.pos.x + 1 >= tiles.GetLength(0)) {
                    return null; // out of bounds
                }
                return tiles[tile.pos.x + 1, tile.pos.y];

            default :
                Debug.LogWarning("Invalid Direction passed");
                return null;
        }
    }

    public Tile[] GetAllAdjacentTiles(Tile tile) {
        Tile[] result = new Tile[4];
        for(int i = 0; i<4; i++) {
            result[i] = GetAdjacentTile((Dir)(i+1), tile);
        }
        return result;
    }

    public bool AreAnyAdjacentPlants(Tile origin) {
        Tile[] adjacents = GetAllAdjacentTiles(origin);
        for(int i = 0; i<adjacents.Length; i++) {
            if(adjacents[i] is Plant || adjacents[i] is PlantPoint) {
                return true;
            }
        }
        return false;
    }

    public void PairComplete() {
        pairsComplete++;
        Debug.Log("Pair made!");
        if(pairsComplete == levels[currentLevel].objectives.Count) {
            Debug.Log("You beat the level!");
        }
    }
}