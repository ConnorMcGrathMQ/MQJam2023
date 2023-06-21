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
        Plant smallestSoFar = null;
        foreach (Tile t in Tiles)
        {
            if (t is Plant p) {
                if (p.remainingDist < smallestSoFar.remainingDist) {
                    smallestSoFar = p;
                }
            }
        }
    }
}
