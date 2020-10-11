using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar
{
    public Spot[,] Spots;
    public Spot current;
    public List<Spot> OpenSet = new List<Spot>();
    public List<Spot> ClosedSet = new List<Spot>();
    //public List<Spot> Path = new List<Spot>();
    Spot Start;
    public Spot End;
    public Astar(Cell[,] grid = null)
    {
        Spots = new Spot[Utils.Columns, Utils.Rows];
    }
    public List<Spot> CreatePath(Cell[,] grid, Vector2Int start, Vector2Int end)
    {
        //if ((end.x == -1 || end.y == -1) || (start.x == end.x && start.y == end.y))
        //    return null;

        //if (grid[end.x, end.y].entity)
        //    return null;
        //if (grid[end.x, end.y].Height >= 1)
        //    return null;

        OpenSet.Clear();
        ClosedSet.Clear();


        for (int i = 0; i < Utils.Columns; i++)
        {
            for (int j = 0; j < Utils.Rows; j++)
            {
                Spots[i, j] = new Spot(i, j, 0, 0, 0, grid[i, j].Height);
                //if (Spots[i, j].Height < 1 && grid[i, j].entity)
                //    Spots[i, j].Height = 1;
            }
        }
        Start = Spots[start.x, start.y];
        End = Spots[end.x, end.y];
        OpenSet.Add(Start);

        while (OpenSet.Count > 0)
        {
            int winner = 0;
            for (int i = 0; i < OpenSet.Count; i++)
                if (OpenSet[i].F < OpenSet[winner].F)
                    winner = i;
                else if (OpenSet[i].F == OpenSet[winner].F)//tie breaking
                    if (OpenSet[i].H < OpenSet[winner].H)
                        winner = i;

            current = OpenSet[winner];

            if (OpenSet[winner] == End)
            {
                List<Spot> Path = new List<Spot>();
                var temp = current;
                Path.Add(temp);
                while (temp.previous != null)
                {
                    Path.Add(temp.previous);
                    temp = temp.previous;
                }
                //if (length - (Path.Count - 1) < 0)
                //{
                //    Path.RemoveRange(0, (Path.Count - 1) - length);
                //}
                for (int i = 0; i < Path.Count; i++)
                    grid[Path[i].X, Path[i].Y].Renderer.color = new Color(0.8f, 1, 1);
                return Path;
            }

            OpenSet.Remove(current);
            ClosedSet.Add(current);


            if (current.Neighboors.Count == 0)
                current.AddNeighboors(Spots);

            var neighboors = current.Neighboors;
            for (int i = 0; i < neighboors.Count; i++)
            {
                var n = neighboors[i];
                if (!ClosedSet.Contains(n) && n.Height < 1)
                {
                    var tempG = current.G + 1;

                    bool newPath = false;
                    if (OpenSet.Contains(n))
                    {
                        if (tempG < n.G)
                        {
                            n.G = tempG;
                            newPath = true;
                        }
                    }
                    else
                    {
                        n.G = tempG;
                        newPath = true;
                        OpenSet.Add(n);
                    }
                    if (newPath)
                    {
                        n.H = Heuristic(n, End);
                        n.F = n.G + n.H;
                        n.previous = current;
                    }
                }
            }

        }
        return null;
    }

    private int Heuristic(Spot a, Spot b)
    {
        //manhattan
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        return 1 * (dx + dy);

        #region diagonal
        //diagonal
        // Chebyshev distance
        //var D = 1;
        // var D2 = 1;
        //octile distance
        //var D = 1;
        //var D2 = 1;
        //var dx = Math.Abs(a.X - b.X);
        //var dy = Math.Abs(a.Y - b.Y);
        //var result = (int)(1 * (dx + dy) + (D2 - 2 * D));
        //return result;// *= (1 + (1 / 1000));
        //return (int)Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        #endregion
    }
}

[Serializable]
public class Spot
{
    public int X;
    public int Y;
    public int F;
    public int G;
    public int H;
    public int Height = 0;
    public List<Spot> Neighboors;
    public Spot previous = null;
    public Spot(int x, int y, int f, int g, int h, int height)
    {
        X = x;
        Y = y;
        F = f;
        G = g;
        H = h;
        Neighboors = new List<Spot>();
        Height = height;
    }
    public void AddNeighboors(Spot[,] grid)
    {
        if (X < Utils.Columns - 1)
            Neighboors.Add(grid[X + 1, Y]);
        if (X > 0)
            Neighboors.Add(grid[X - 1, Y]);
        if (Y < Utils.Rows - 1)
            Neighboors.Add(grid[X, Y + 1]);
        if (Y > 0)
            Neighboors.Add(grid[X, Y - 1]);
        #region diagonal
        //if (X > 0 && Y > 0)
        //    Neighboors.Add(grid[X - 1, Y - 1]);
        //if (X < Utils.Columns - 1 && Y > 0)
        //    Neighboors.Add(grid[X + 1, Y - 1]);
        //if (X > 0 && Y < Utils.Rows - 1)
        //    Neighboors.Add(grid[X - 1, Y + 1]);
        //if (X < Utils.Columns - 1 && Y < Utils.Rows - 1)
        //    Neighboors.Add(grid[X + 1, Y + 1]);
        #endregion
    }


}