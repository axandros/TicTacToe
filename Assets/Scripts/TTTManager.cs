using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTManager : MonoBehaviour
{
    private bool?[,] grid; // null is empty, false is X, true is 0

    [SerializeField]
    GameObject[] NotsAndCrosses;

    private void Start()
    {
        //initialize the Grid
        grid = new bool?[3, 3];
        for (int i = 0; i < 3; i++)
        {
            //Debug.Log("Grid i: " + i);
            grid[i, 0] = null;
            grid[i, 1] = null;
            grid[i, 2] = null;
        }
    }

    public bool?[,] GetGrid()
    {
        return grid;
    }

    public void Set(int x, int y, bool? value)
    {
        //Debug.Log("TTTGrid: Setting " + x + ", " + y + " to " + value);
        if (IsClamped(x, 0, 2) && IsClamped(y, 0, 2))
        {
            grid[x, y] = value;
            //Debug.Log("Long position: " + (x * 3 + y));
            NotsAndCrosses[x * 3 + y].GetComponent<NotAndCrossSpace>().ChangeState(value);
        }
    }

    public void Set(int n, bool? value)
    {
        switch (n)
        {
            case 0: SafeSet(0, 0, value); break;
            case 1: SafeSet(0, 1, value); break;
            case 2: SafeSet(0, 2, value); break;
            case 3: SafeSet(1, 0, value); break;
            case 4: SafeSet(1, 1, value); break;
            case 5: SafeSet(1, 2, value); break;
            case 6: SafeSet(2, 0, value); break;
            case 7: SafeSet(2, 1, value); break;
            case 8: SafeSet(2, 2, value); break;
        }
    }

    public void SafeSet(int x, int y, bool? value)
    {
        if (IsClamped(x, 0, 2) && IsClamped(y, 0, 2))
        {
            Debug.Log("Attempting Safe Insert at " + x + ", " + y);
            if (grid[x, y] == null)
            {
                //Debug.Log("Insert Success");
                grid[x, y] = value;
                NotsAndCrosses[x * 3 + y].GetComponent<NotAndCrossSpace>().ChangeState(value);
            }
        }
    }
    
    public bool IsBoardFull()
    {
        bool?[,] board = GetGrid();
        bool ret = true;

        for (int x = 0; x < 3 && ret; x++)
        {
            //Debug.Log("Checking x " + x);
            for (int y = 0; y < 3 && ret; y++)
            {
                //Debug.Log("Checking y " + y);
                ret = (board[x, y] != null);
            }
        }

        return ret;
    }

    public int NumberOfEmptySpaces()
    {
        bool?[,] board = GetGrid();
        int count = 0;

        for (int x = 0; x < 3; x++)
        {
            //Debug.Log("Checking x " + x);
            for (int y = 0; y < 3; y++)
            {
                if (board[x, y] == null)
                {
                    count++;
                }
            }
        }

        //Debug.Log("Counted " + count + "  open spaces");
        return count;
    }

    public void InsertNthSpace(int n, bool value)
    {
        if (n < NumberOfEmptySpaces())
        {
            int count = 0;
            int iter;
            for (iter = 0; count <= n && iter < 9; iter++)
            {
                Debug.Log("Nth space: " + iter + " = " + (GetNthSpace(iter) == null));
                if (GetNthSpace(iter) == null)
                {
                    count++;
                }
            }
            Debug.Log("Count: " + count);
            if (iter < 10)
            {
                switch (iter-1)
                {
                    case 0: SafeSet(0, 0, value); break;
                    case 1: SafeSet(0, 1, value); break;
                    case 2: SafeSet(0, 2, value); break;
                    case 3: SafeSet(1, 0, value); break;
                    case 4: SafeSet(1, 1, value); break;
                    case 5: SafeSet(1, 2, value); break;
                    case 6: SafeSet(2, 0, value); break;
                    case 7: SafeSet(2, 1, value); break;
                    case 8: SafeSet(2, 2, value); break;
                }
            }
            else
            {
                //Debug.Log("Looping to long.");
            }
        }
    }

    private bool IsClamped(int test, int low, int high)
    {
        bool ret = false;
        if (test >= low && test <= high) { ret = true; }
        return ret;
    }



    public bool? GetNthSpace(int n)
    {
        switch (n)
        {
            case 0: return grid[0, 0];
            case 1: return grid[0, 1];
            case 2: return grid[0, 2];
            case 3: return grid[1, 0];
            case 4: return grid[1, 1];
            case 5: return grid[1, 2];
            case 6: return grid[2, 0];
            case 7: return grid[2, 1];
            case 8: return grid[2, 2];
            default: return null;
        }
    }

    public void SetSpace(NotAndCrossSpace NS, bool? state)
    {
        //TODO: I'm tired.  This needs set properly
        for (int i = 0; i < 9; i++)
        {
            if (NotsAndCrosses[i].Equals(NS))
            {
                //NS.ChangeState(state);
                Set(i, state);
            }
        }
    }
}