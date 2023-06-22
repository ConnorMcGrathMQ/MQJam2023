using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Plant : Tile
{
    public PlantObject species;

    public int remainingDist;

    public Dir inDir;
    public Dir outDir = Dir.None;
    public Plant prev;
    public Plant next;
    public int identifier;
    public Vector3 thornOffset;
    public Tile assistTile;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        } 
        prev = null;
        next = null;
        prev = FindSmallestNeighbour();
        
        if (prev != null) {
            if (prev.pos.x > this.pos.x) {
                //Prev on Rightside
                this.inDir = Dir.Right;
                prev.outDir = Dir.Left;
            } else if (prev.pos.x < this.pos.x) {
                //Prev on Leftside
                this.inDir = Dir.Left;
                prev.outDir = Dir.Right;
            } else if (prev.pos.y > this.pos.y) {
                //Prev on Top
                this.inDir = Dir.Up;
                prev.outDir = Dir.Down;
            } else if (prev.pos.y < this.pos.y) {
                //Prev on Bottom
                this.inDir = Dir.Down;
                prev.outDir = Dir.Up;
            }
            remainingDist = prev.remainingDist - 1;
            if(remainingDist == 0) {
                PlayerController.Instance.StopDrawing();
            }
            prev.UpdateSprite();
            prev.next = this;
            // if(prev == next) {
            //     next = null;
            // }
        } else {
            //PLANT START POINT STUFF GOES HERE
        }
        UpdateSprite();
        UIManager.Instance.UpdateLengthText(this);
        if(species.Equals(Board.Instance.roseType)) {
            Dir thornDirection = Dir.None;
            if(prev != null && prev.remainingDist % 2 == 0) {
                switch(prev.outDir) {
                    case Dir.Up :
                        thornDirection = Dir.Left;
                        break;
                    case Dir.Left :
                        thornDirection = Dir.Down;
                        break;
                    case Dir.Right :
                        thornDirection = Dir.Up;
                        break;
                    case Dir.Down :
                        thornDirection = Dir.Right;
                        break;
                }
            } else {
                switch(prev.outDir) {
                    case Dir.Up :
                        thornDirection = Dir.Right;
                        break;
                    case Dir.Left :
                        thornDirection = Dir.Up;
                        break;
                    case Dir.Right :
                        thornDirection = Dir.Down;
                        break;
                    case Dir.Down :
                        thornDirection = Dir.Left;
                        break;
                }
            }
            Tile thornTile = Board.Instance.GetAdjacentTile(thornDirection, 
                Board.Instance.GetTile(prev.pos));
            if(thornTile == null) {
                return;
            }
            if(thornTile is Empty) {
                Board.Instance.AddTile(Board.Instance.thornPrefab, thornTile.pos);
                thornTile = Board.Instance.GetTile(thornTile.pos); // get newly made one
                switch(thornDirection) {
                    case Dir.None :
                        Debug.LogWarning("Invalid direction for a Thorn!");
                        break;
                    case Dir.Left :
                        thornTile.transform.eulerAngles = new Vector3(0, 0, 90);
                        break;
                    case Dir.Up :
                        thornTile.transform.eulerAngles = new Vector3(0, 0, 0);
                        break;
                    case Dir.Down :
                        thornTile.transform.eulerAngles = new Vector3(0, 0, 180);
                        break;
                    case Dir.Right :
                        thornTile.transform.eulerAngles = new Vector3(0, 0, -90);
                        break;
                    
                }
                thornTile.transform.Translate(thornOffset, Space.Self);
                prev.assistTile = thornTile;
            }
        }
    }

    public void UpdateSprite() {
        spriteRenderer.color = species.colorList[identifier].vineColour;
        transform.eulerAngles = new Vector3(0, 0, 0);
        spriteRenderer.flipX = false;
        switch (inDir)
        {
            case Dir.None:
                spriteRenderer.sprite = species.startSprite;
                switch (outDir)
                {
                    case Dir.None:
                        //Panic
                        break;
                    case Dir.Left:
                        transform.eulerAngles = new Vector3(0, 0, -90);
                        break;
                    case Dir.Right:
                        transform.eulerAngles = new Vector3(0, 0, 90);
                        break;
                    case Dir.Up:
                        transform.eulerAngles = new Vector3(0, 0, 180);
                        break;
                    case Dir.Down:
                        break;
                }
                break;
            case Dir.Left:
                switch (outDir)
                {
                    case Dir.None:
                        spriteRenderer.sprite = species.endSprite;
                        transform.eulerAngles = new Vector3(0, 0, -90);
                        break;
                    case Dir.Left:
                        //Cannot lead to self
                        break;
                    case Dir.Right:
                        //Rot -90
                        spriteRenderer.sprite = species.straightSprite;
                        transform.eulerAngles = new Vector3(0, 0, -90);
                        break;
                    case Dir.Up:
                        //Rot -90
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.eulerAngles = new Vector3(0, 0, -90);
                        break;
                    case Dir.Down:
                        //Rot -90 Flip X
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.eulerAngles = new Vector3(0, 0, -90);
                        spriteRenderer.flipX = true;
                        break;
                }
                break;
            case Dir.Right:
                switch (outDir)
                {
                    case Dir.None:
                        spriteRenderer.sprite = species.endSprite;
                        transform.eulerAngles = new Vector3(0, 0, 90);
                        break;
                    case Dir.Left:
                        //Rot +90
                        spriteRenderer.sprite = species.straightSprite;
                        transform.eulerAngles = new Vector3(0, 0, 90);
                        break;
                    case Dir.Right:
                        //Cannot lead to self
                        break;
                    case Dir.Up:
                        //Rot 90
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.eulerAngles = new Vector3(0, 0, 90);
                        spriteRenderer.flipX = true;
                        break;
                    case Dir.Down:
                        //Rot +90
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.eulerAngles = new Vector3(0, 0, 90);
                        break;
                }
                break;
            case Dir.Up:
                switch (outDir)
                {
                    case Dir.None:
                        spriteRenderer.sprite = species.endSprite;
                        transform.eulerAngles = new Vector3(0, 0, 180);
                        break;
                    case Dir.Left:
                        //Rot 180 and Flip X
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.eulerAngles = new Vector3(0, 0, 180);
                        spriteRenderer.flipX = true;
                        break;
                    case Dir.Right:
                        //Rot 180
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.eulerAngles = new Vector3(0, 0, 180);
                        break;
                    case Dir.Up:
                        //Cannot lead to self
                        break;
                    case Dir.Down:
                        //Rot 180
                        spriteRenderer.sprite = species.straightSprite;
                        transform.eulerAngles = new Vector3(0, 0, 180);
                        break;
                }
                break;
            case Dir.Down:
                switch (outDir)
                {
                    case Dir.None:
                        spriteRenderer.sprite = species.endSprite;
                        break;
                    case Dir.Left:
                        //Default
                        spriteRenderer.sprite = species.cornerSprite;
                        break;
                    case Dir.Right:
                        //Flip X
                        spriteRenderer.sprite = species.cornerSprite;
                        spriteRenderer.flipX = true;
                        break;
                    case Dir.Up:
                        //Default
                        spriteRenderer.sprite = species.straightSprite;
                        break;
                    case Dir.Down:
                        //Cannot lead to self
                        break;
                }
                break;
        }
    }

    public Plant FindSmallestNeighbour() {
        Tile[] Tiles = Board.Instance.GetAllAdjacentTiles((Tile)this);
        List<Plant> plantList = new List<Plant>();
        foreach (Tile t in Tiles)
        {
            if (t is Plant p) {
                if (p.species == this.species && p.identifier == this.identifier) {
                    plantList.Add(p);
                }
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

    void OnDisable() {
        if(prev != null) {
            prev.next = null;
        }
    }
}
