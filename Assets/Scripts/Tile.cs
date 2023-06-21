using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Board board;
    public Vector2Int pos;

    // Start is called before the first frame update
    void Start()
    {
        board = transform.parent.GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePosition(Vector2Int newPos) {
        pos = newPos;
    }
}
