public class Plant : Tile
{
    public PlantType type;

    public int remainingDist;

    public Dir inDir;
    public Dir outDir = None;

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
        Tile[] Tiles = Board.Instance.GetAllAdjacentTiles();
        List<Plant> plantList = null;
        foreach (Tile t in Tiles)
        {
            if (t is Plant p) {
                plantList.add(p);
            }
        }
        Plant smallest = null;
        if (plantList.length >= 1) {
            smallest = plantList[0];
            for (int i = 1; i < plantList.length; i++) {
                if (plantList[i].remainingDist < smallest.remainingDist) {
                    smallest = plantList[i];
                }
            }
        }
        return smallest;
    }
}
