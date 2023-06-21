using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Tile
{
    public PlantType type;

    public int remainingDist;

    public Dir inDir;
    public Dir outDir = Dir.None;

    // Start is called before the first frame update
    void Start()
    {
        Plant neighbour = FindSmallestNeighbour();
        remainingDist = neighbour.remainingDist - 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Plant FindSmallestNeighbour() {
        Tile[] Tiles = Board.Instance.GetAllAdjacentTiles((Tile)this);
        List<Plant> plantList = null;
        foreach (Tile t in Tiles)
        {
            if (t is Plant p) {
                plantList.Add(p);
            }
        }
        Plant smallest = null;
        if (plantList.Count >= 1) {
            smallest = plantList[0];
            for (int i = 1; i < plantList.Count; i++) {
                if (plantList[i].remainingDist < smallest.remainingDist) {
                    smallest = plantList[i];
                }
            }
        }
        return smallest;
    }
}
