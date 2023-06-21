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
}