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
    public Obstacle thornPrefab;
    public ParticleSystem energyParticles;

    public List<Level> levels;
    private int currentLevel;
    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
    }
    private int pairsComplete;
    public PlantObject roseType;

    void Awake() {
        if(Instance == null) {
            grid = GetComponent<Grid>();
            tiles = new Tile[width, height];
            Instance = this;
        } else {
            Debug.Log("Duplicate boards exist! Destroying...");
        }

    }

    void Start() {
        LoadLevel();
    }

    public void LoadLevel(int level) {
        currentLevel = level;
        LoadLevel();
    }

    public void LoadLevel() {
        PlayerController.Instance.plantsDrawn = 0;
        pairsComplete = 0;
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
    
    public Level GetCurrentLevel() {
        return levels[currentLevel];
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
        newTile = Instantiate(tilePrefab, grid.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0)), 
            Quaternion.identity, this.transform);
        old = location;
        tiles[pos.x, pos.y] = newTile;
        newTile.pos = pos;
        newTile.gameObject.name = newTile.ToString();
        if(newTile is PlantPoint point) {
            point.FindPartner(levels[currentLevel]);
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
        // Debug.Log($"Checking {tile.ToString()}'s Adjacent at {dir}");
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

    public void DestroyPlantFrom(Plant target) {
        Plant nextPlant;
        Plant originalPrev = target.prev;
        while(target != null) {
            nextPlant = target.next;
            if(target.prev == null) {
                Debug.Log("Is original, skipping...");
                target = nextPlant;
            } else {
                if(target.assistTile != null) {
                    AddTile(emptyTilePrefab, target.assistTile.pos);
                }
                AddTile(emptyTilePrefab, target.pos);
                target = nextPlant;
            }   
        }
        originalPrev.UpdateSprite();
    }

    public void PairComplete() {
        pairsComplete++;
        Debug.Log("Pair Complete!");
        if(pairsComplete == levels[currentLevel].objectives.Count) {
            currentLevel++;
            UIManager.Instance.OpenLevelComplete();
        }
    }

    public float GetFilledPercent() {
        int amountFilled = 0;
        for(int x = 0; x<width; x++) {
            for(int y = 0; y<height; y++) {
                if(!(GetTile(x, y) is Empty)) {
                    amountFilled++;
                }
            }
        }
        return (float)amountFilled / (float)(height * width) * 100;
    }

    public bool AreAdjacent(Tile a, Tile b) {
        if(Mathf.Abs(a.pos.x - b.pos.x) == 1 && a.pos.y.Equals(b.pos.y)) {
            return true;
        } else if(Mathf.Abs(a.pos.y - b.pos.y) == 1 && a.pos.x.Equals(b.pos.x)) {
            return true;
        } 
        return false;
    }

    public void CreateEnergyEffect(Plant origin) {
        ParticleSystem particles = Instantiate(energyParticles);
        particles.transform.SetParent(origin.transform);
        particles.transform.localPosition = Vector3.zero;
        var main = particles.main;
        main.startColor = origin.connector.GetComponent<SpriteRenderer>().color;
    }
}