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

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        Plant prev = FindSmallestNeighbour();
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
        } else {
            //PLANT START POINT STUFF GOES HERE
        }
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSprite() {
        spriteRenderer.color = species.vineColour;
        transform.rotation.eulerAngles = new Vector3(0, 0, 0);
        spriteRenderer.flipX = false;
        if (outDir == Dir.None) {
            spriteRenderer.sprite = species.endSprite;
        }
        switch (inDir)
        {
            Dir.Left:
                switch (outDir)
                {
                    Dir.Left:
                        //Cannot lead to self
                    Dir.Right:
                        //Rot -90
                        spriteRenderer.sprite = species.straightSprite;
                        transform.rotation.eulerAngles = new Vector3(0, 0, -90);
                    Dir.Up:
                        //Rot 90
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.rotation.eulerAngles = new Vector3(0, 0, 90);
                    Dir.Down:
                        //Rot -90 Flip X
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.rotation.eulerAngles = new Vector3(0, 0, -90);
                        spriteRenderer.flipX = true;
                }
            Dir.Right:
                switch (outDir)
                {
                    Dir.Left:
                        //Rot +90
                        spriteRenderer.sprite = species.straightSprite;
                        transform.rotation.eulerAngles = new Vector3(0, 0, 90);
                    Dir.Right:
                        //Cannot lead to self
                    Dir.Up:
                        //Rot -90
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.rotation.eulerAngles = new Vector3(0, 0, -90);
                    Dir.Down:
                        //Rot +90
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.rotation.eulerAngles = new Vector3(0, 0, 90);
                }
            Dir.Up:
                switch (outDir)
                {
                    Dir.Left:
                        //Rot 180 and Flip X
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.rotation.eulerAngles = new Vector3(0, 0, 180);
                        spriteRenderer.flipX = true;
                    Dir.Right:
                        //Rot 180
                        spriteRenderer.sprite = species.cornerSprite;
                        transform.rotation.eulerAngles = new Vector3(0, 0, 180);
                    Dir.Up:
                        //Cannot lead to self
                    Dir.Down:
                        //Rot 180
                        spriteRenderer.sprite = species.straightSprite;
                        transform.rotation.eulerAngles = new Vector3(0, 0, 180);
                }
            Dir.Down:
                switch (outDir)
                {
                    Dir.Left:
                        //Default
                        spriteRenderer.sprite = species.cornerSprite;
                    Dir.Right:
                        //Flip X
                        spriteRenderer.sprite = species.cornerSprite;
                        spriteRenderer.flipX = true;
                    Dir.Up:
                        //Default
                        spriteRenderer.sprite = species.straightSprite;
                    Dir.Down:
                        //Cannot lead to self
                }
        }
    }

    public Plant FindSmallestNeighbour() {
        Tile[] Tiles = Board.Instance.GetAllAdjacentTiles((Tile)this);
        List<Plant> plantList = null;
        foreach (Tile t in Tiles)
        {
            if (t is Plant p) {
                if (p.species == this.species) {
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
}
