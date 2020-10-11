using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int X;
    public int Y;
    
    public void PlaceOnGrid(Cell[,] grid, int x, int y)
    {
        X = x;
        Y = y;
        GetComponent<SpriteRenderer>().sortingOrder = Utils.Rows - y + 1;
        grid[x, y].entity = this;
        transform.position = grid[x, y].gameObject.transform.position;
    }

    private void FixedUpdate()
    {
       
    }
}
