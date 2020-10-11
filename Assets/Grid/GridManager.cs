using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SO;
public class GridManager : MonoBehaviour
{
    #region Instance
    public static GridManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }
    #endregion
    public GameObject tilePrefab;
    public Transform obstaclePrefab;
    public Transform goalPrefab;
    public Transform coinPrefab;
    //public Sprite[] sprites;
    public TileThemeObject[] themes;
    public int currentTheme;
    public Cell[,] Grid;
    public Astar astar;
    Coroutine createPath;

    public List<Coord> allTileCoords;

    Queue<Coord> shuffledTileCoords;
    Queue<Coord> shuffledOpenTileCoords;

    public int seed = 10;

    public IntVariable level;

    [Range(0, 1)]
    public float obstaclePercent;

    Coord mapCentre;
    Coord startPos;
    Coord endPos;

    Transform[,] tileMap;
    Cell[,] openCells;
    // Start is called before the first frame update
    void Start()
    {
        SetupGrid();
        astar = new Astar(Grid);
    }

    public void SetupGrid()
    {

        seed = level.value;
        //Vertical = (int)Camera.main.orthographicSize;
        //Horizontal = Vertical * (Screen.width / Screen.height);
        //Columns = Horizontal * 2;
        //Rows = Vertical * 2;
        Utils.Vertical = 6;
        Utils.Horizontal = Utils.Vertical;
        Utils.Columns = Utils.Horizontal;
        Utils.Rows = Utils.Vertical;
        Grid = new Cell[Utils.Columns, Utils.Rows];

        tileMap = new Transform[Utils.Horizontal, Utils.Vertical];
        //openCells = new Cell[]
        allTileCoords = new List<Coord>();
        for (int x = 0; x < Utils.Columns; x++)
        {
            for (int y = 0; y < Utils.Rows; y++)
            {
                allTileCoords.Add(new Coord(x, y));
                //SpawnTile(i, j, Grid[i, j]);
            }
        }

        shuffledTileCoords = new Queue<Coord>(Utils.ShuffleArray(allTileCoords.ToArray(), seed));

        mapCentre = new Coord((int)Utils.Columns / 2, (int)Utils.Rows / 2);
        startPos = new Coord(0, 0);
        endPos = new Coord((int)Utils.Columns - 1, (int)Utils.Rows - 1);


        bool[,] obstacleMap = new bool[Utils.Columns, Utils.Rows];
        obstaclePercent = Mathf.Clamp01(0.05f * level.value);
        int obstacleCount = (int) (Utils.Columns * Utils.Rows * obstaclePercent);
        int currentObstacleCount = 0;
        List<Coord> allOpenCoords = new List<Coord>(allTileCoords);

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (randomCoord != mapCentre && randomCoord != endPos && randomCoord != startPos && MapIsFullyAccessible(obstacleMap, currentObstacleCount) )
            {
                Vector3 obstaclePosition = Utils.GridToWorldPosition(randomCoord.x, randomCoord.y);

                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity) as Transform;

                allOpenCoords.Remove(randomCoord);
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }

            
        }

        allOpenCoords.Remove(endPos);
        shuffledOpenTileCoords = new Queue<Coord>(Utils.ShuffleArray(allOpenCoords.ToArray(), seed));



        for (int i = 0; i < Utils.Columns; i++)
        {
            for (int j = 0; j < Utils.Rows; j++)
            {
                Grid[i, j] = new Cell(tilePrefab, themes[currentTheme].tiles[Utils.GetTile(i, j)], i, j, obstacleMap[i, j] == true ? 1 : 0);
                tileMap[i, j] = Grid[i, j].gameObject.transform;
                //SpawnTile(i, j, Grid[i, j]);
            }
        }

        Vector3 goalPosition = Utils.GridToWorldPosition(endPos.x, endPos.y);

        Transform goalTransform = Instantiate(goalPrefab, goalPosition, Quaternion.identity) as Transform;

        List<GridManager.Coord> freeCoords = this.GetAllOpenCoords();

        //GateLock();

        for (int i = 0; i < freeCoords.Count; i++)
        {
            Vector3 coinPosition = Utils.GridToWorldPosition(freeCoords[i].x, freeCoords[i].y);

            Transform coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity) as Transform;
        }

    }


    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCentre);
        mapFlags[mapCentre.x, mapCentre.y] = true;
        //queue.Enqueue(startPos);
        //mapFlags[startPos.x, startPos.y] = true;
        //queue.Enqueue(endPos);
        //mapFlags[endPos.x, endPos.y] = true;
        int accessibleTileCount = 1;
        while(queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;

                    if(x ==0 || y ==0)
                    {
                        if(neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if(!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(Utils.Columns * Utils.Rows - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }
   

    public void UpdateTileTheme(int index)
    {
        currentTheme = index;
        for (int i = 0; i < Utils.Columns; i++)
        {
            for (int j = 0; j < Utils.Rows; j++)
            {
                Grid[i, j].UpdateTile(themes[currentTheme].tiles[Utils.GetTile(i, j)]);
            }
        }
    }

    public void Update()
    {
        //Test
        if (Input.GetKeyDown(KeyCode.Space))
            UpdateTileTheme(currentTheme < themes.Length - 1 ? currentTheme += 1 : 0);

        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    //astar.CreatePath(Grid);
        //    if (createPath != null)
        //    {
        //        StopCoroutine(createPath);
        //    }
        //    createPath = StartCoroutine(astar.CreatePath(Grid));
        //}



        //for (int i = 0; i < astar.closedSet.Count; i++)
        //{
        //    Grid[astar.closedSet[i].X, astar.closedSet[i].Y].Renderer.color = Color.red;
        //}
        //for (int i = 0; i < astar.openSet.Count; i++)
        //{
        //    Grid[astar.openSet[i].X, astar.openSet[i].Y].Renderer.color = Color.green;
        //}
        //for (int i = 0; i < astar.path.Count; i++)
        //{
        //    Grid[astar.path[i].X, astar.path[i].Y].Renderer.color = Color.blue;
        //}

        //if (astar.start != null)
        //{
        //    Grid[astar.start.X, astar.start.Y].Renderer.color = Color.cyan;
        //}
        //if (astar.end != null)
        //{
        //    Grid[astar.end.X, astar.end.Y].Renderer.color = Color.black;
        //}
    }


    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public Transform GetRandomOpenTile()
    {
        Coord randomCoord = shuffledOpenTileCoords.Dequeue();
        shuffledOpenTileCoords.Enqueue(randomCoord);
        return tileMap[randomCoord.x, randomCoord.y];
    }

    public Coord GetRandomOpenTileCoord()
    {
        Coord randomCoord = shuffledOpenTileCoords.Dequeue();
        shuffledOpenTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public Vector2Int GetRandomOpenTileVector()
    {
        Coord randomCoord = shuffledOpenTileCoords.Dequeue();
        shuffledOpenTileCoords.Enqueue(randomCoord);
        return new Vector2Int(randomCoord.x, randomCoord.y);
    }

    public List<Coord> GetAllOpenCoords()
    {
        List<Coord> tempCoordList = shuffledOpenTileCoords.ToList();
        tempCoordList.Remove(startPos);
        tempCoordList.Remove(endPos);
        return tempCoordList;
    }

    public void GateUnlock()
    {
        Grid[endPos.x, endPos.y].Height = 0;
    }

    public void GateLock()
    {
        Grid[endPos.x, endPos.y].Height = 1;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coord c1, Coord c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }
    }

    //private void SpawnTile(int x, int y, float value)
    //{

    //    SpriteRenderer sr = Instantiate(tilePrefab, Utils.GridToWorldPosition(x, y), Quaternion.identity).GetComponent<SpriteRenderer>();

    //    sr.name = "x : " + x + " y: " + y;
    //    sr.sprite = sprites[Utils.GetTile(x, y)];

    //}



}
