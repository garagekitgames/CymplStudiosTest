using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static int Vertical, Horizontal, Columns, Rows;

    public static  Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3(x - (Horizontal - 0.5f), y - (Vertical - 0.5f));
    }

    public static int GetTile(int x, int y)
    {
        if (y == Rows - 1 && x == 0)
            return 0;
        else if (y == Rows - 1 && x != 0 && x != Columns - 1)
            return 1;
        else if (y == Rows - 1 && x == Columns - 1)
            return 2;
        else if (x == 0 && y != 0 && y != Rows - 1)
            return 3;
        else if (x == Columns - 1 && y != 0 && y != Rows - 1)
            return 5;
        else if (x == 0 && y == 0)
            return 6;
        else if (x != 0 && x != Columns - 1 && y == 0)
            return 7;
        else if (x == Columns - 1 && y == 0)
            return 8;
        else
            return 4;

    }


    public static T[] ShuffleArray<T>(T[] array, int seed = 1)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }

        return array;
    }
}
