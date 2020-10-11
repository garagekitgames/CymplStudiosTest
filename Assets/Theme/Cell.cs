using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell 
{
    public GameObject gameObject;
    public SpriteRenderer Renderer;
    public int X;
    public int Y;
    public int Height;
    public Entity entity;

    public Cell(GameObject prefab, Sprite sprite, int x, int y, int height)
    {
        X = x;
        Y = y;
        Height = height;
        entity = null;
        gameObject = GameObject.Instantiate(prefab, Utils.GridToWorldPosition(x, y), Quaternion.identity);
        Renderer = gameObject.GetComponent<SpriteRenderer>();

        //Delete Later just for debugging
        gameObject.name = "X: " + x + "Y: " + y+"H: "+height;
        Renderer.sprite = sprite;
    }

    public void UpdateTile(Sprite sprite)
    {
        Renderer.sprite = sprite;
    }

}
