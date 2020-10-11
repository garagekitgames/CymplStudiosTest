using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : Entity
{
    public List<Spot> path = new List<Spot>();
    public float nextWaypointDistance = 0.001f;



    public bool reachedEndOfPath;



    private Astar astar;
    public float speed = 1;

    public Vector2Int destination = new Vector2Int(5, 5);

    public bool pathReached;
    public int id = 0;
    //public 
    // Start is called before the first frame update
    private void Start()
    {
        astar = new Astar(GridManager.instance.Grid);
        destination = new Vector2Int(X, Y);
    }


    public void Move(Cell[,] grid, int x, int y)
    {
        if (path != null && path.Count > 0)
        {
            for (int i = 0; i < path.Count; i++)
                grid[path[i].X, path[i].Y].Renderer.color = Color.white;
            path.Clear();
        }
        //if (corutine != null)
        //    StopCoroutine(corutine);
        path = astar.CreatePath(grid, new Vector2Int(X, Y), new Vector2Int(x, y));

        if (path == null || path.Count <= 0)
            return;
        //corutine = Moving(grid, path);
        //StartCoroutine(corutine);
        Moving(grid, path);
        //pathReached = false;

    }


    void Moving(Cell[,] grid, List<Spot> path)
    {


        var pathIndex = path.Count - 1;
        if (pathIndex < 0)
            return;
        while (pathIndex >= 0 && path != null)
        {
            Vector3 goal = grid[path[pathIndex].X, path[pathIndex].Y].gameObject.transform.position;

            //Debug.Log("distance To : " + path[pathIndex].X + ", " + path[pathIndex].Y + " | " + Vector3.Distance(transform.position, goal)+" | Index : "+ pathIndex);
            if (Vector3.Distance(transform.position, goal) < 0.001f)
            {
                //Debug.Log("INSIDE : distance To : " + path[pathIndex].X + ", " + path[pathIndex].Y + " | " + Vector3.Distance(transform.position, goal) + " | Index : " + pathIndex);
                grid[path[pathIndex].X, path[pathIndex].Y].Renderer.color = Color.white;
                grid[path[pathIndex].X, path[pathIndex].Y].entity = null;
                
                path.RemoveAt(pathIndex);
                PlaceOnGrid(grid, X, Y);
                transform.position = goal;
                pathIndex--;
                if (pathIndex >= 0)
                {
                    X = path[pathIndex].X;
                    Y = path[pathIndex].Y;
                    GetComponent<SpriteRenderer>().sortingOrder = Utils.Rows - Y + 1;
                    grid[X, Y].entity = this;
                    AudioManager.instance.Play("Walk"+ id);
                    //if (pathIndex != 0)
                    //{
                    //    var newX = path[0].X;
                    //    var newY = path[0].Y;
                    //    path = GridManager.instance.astar.CreatePath(grid, new Vector2Int(X, Y), new Vector2Int(newX, newY), walkDistance);
                    //}
                }
            }
            else
            {
                break;
            }


        }
        //Debug.Log(pathIndex);
        if (pathIndex >= 0)
        {
            pathReached = false;
            //transform.Translate((grid[path[pathIndex].X, path[pathIndex].Y].gameObject.transform.position - transform.position) * Time.deltaTime * 3f);
            transform.position = Vector3.MoveTowards(transform.position, grid[path[pathIndex].X, path[pathIndex].Y].gameObject.transform.position, speed * Time.deltaTime);
        }
        else
        {
            pathReached = true;
        }


    }

    private void OnDestroy()
    {
        GridManager.instance.Grid[X, Y].entity = null;
    }
    // Update is called once per frame
    private void Update()
    {
        Move(GridManager.instance.Grid, destination.x, destination.y);
    }
}
