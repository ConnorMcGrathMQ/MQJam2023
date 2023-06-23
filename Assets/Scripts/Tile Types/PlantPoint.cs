using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlantPoint: Tile
{
    private bool connected;
    private int distance;
    public PlantObject species;
    public PlantPoint partner;
    public Plant next;

    void Start() {
        connected = false;
        distance = 0;
    }

    public void Connected(Plant plant) {
        if(connected) {
            return;
        }
        connected = true;
        if(partner.connected) {
            distance = species.maxLength - plant.remainingDist;
            Board.Instance.PairComplete();
            PlayerController.Instance.StopDrawing();
            
        }
    }

    public void FindPartner(Level level) {
        Objective obj = new Objective();
        foreach (Objective o in level.objectives)
        {
            if(pos.x == o.firstPointPosition.x && pos.y == o.firstPointPosition.y) {
                if(Board.Instance.GetTile(o.secondPointPosition) != null &&
                    Board.Instance.GetTile(o.secondPointPosition) is PlantPoint point) {
                    partner = point;
                    obj = o;
                    break;
                }
            } else if (pos.x == o.secondPointPosition.x && pos.y == o.secondPointPosition.y) {
                if(Board.Instance.GetTile(o.firstPointPosition) != null &&
                    Board.Instance.GetTile(o.firstPointPosition) is PlantPoint point) {
                    partner = point;
                    obj = o;
                    break;
                }
            }
        }
        if(partner != null) {
            GetComponent<SpriteRenderer>().color = obj.pairColour;
            partner.GetComponent<SpriteRenderer>().color = obj.pairColour;
            partner.partner = this;
            species = obj.species;
            partner.species = obj.species;
        }
    }

    // void OnDrawGizmos() {
    //     Gizmos.color = new Color(0, 1, 0, 0.5f);
    //     Gizmos.DrawSphere(Board.Instance.GetTile(pos).transform.position, 0.25f);
    // }
}