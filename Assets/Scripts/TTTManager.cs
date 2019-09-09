using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTManager : MonoBehaviour
{
    private bool?[,] grid = null; // null is empty, false is X, true is 0
    private bool? winner = null;

    [SerializeField]
    private bool active = false;
    public bool Active { get { return active; } set { active = value; } }
    private bool turn = true;

    public bool WhoseTurn
    { get { return turn; } }

    [SerializeField]
    GameObject[] NotsAndCrosses = null;

    [SerializeField]
    GameManager Game_Manager = null;

    private void Start()
    {
        try
        {
            ResetLevel();
        }
        catch
        {
            active = false;
        }
    }
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.R))
        { ResetLevel(); }*/
    }

    public bool?[,] GetGrid()
    {
        return (bool?[,])grid.Clone();
    }

    public void Set(int x, int y, bool? value)
    {
        if (active)
        {
            SafeSet(x, y, value);
        }
    }

    public void Set(int n, bool? value)
    {
        Debug.Log("SET: " + n + " - " + value + " | " + WhoseTurn + " | " + active);//(value == null? "null" : (value == true ? "True" : "False")));
        if (winner == null && value == WhoseTurn && active)
        {
            //Debug.Log("SET: " + n + " - " + value);//(value == null? "null" : (value == true ? "True" : "False")));
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
    }

    private void SafeSet(int x, int y, bool? value)
    {
        if (IsClamped(x, 0, 2) && IsClamped(y, 0, 2) && winner == null)
        {
            //Debug.Log("Attempting Safe Insert at " + x + ", " + y);
            if (grid[x, y] == null)
            {
                //Debug.Log("Insert Success");
                grid[x, y] = value;
                NotsAndCrosses[x * 3 + y].GetComponent<NotAndCrossSpace>().ChangeState(value);
                CheckForWin();
                turn = !turn;
            }
        }
    }

    public void SetSpace(NotAndCrossSpace NS, bool? state)
    {
        //Debug.Log("SetSpace Called for: " + NS.name);
        for (int i = 0; i < 9; i++)
        {
            //Debug.Log("Space " + i + " : " + NotsAndCrosses[i].GetComponent<NotAndCrossSpace>().name + " ?= " + NS.name);
            //Debug.Log("Space " + i + " : " + Object.ReferenceEquals(NotsAndCrosses[i].GetComponent<NotAndCrossSpace>(), NS));
            //Debug.Log("Space " + i + " : " + NotsAndCrosses[i].GetHashCode() + " ?= " + NS.GetHashCode());
            //Debug.Log("State: " + state);
            if (Object.ReferenceEquals(NotsAndCrosses[i].GetComponent<NotAndCrossSpace>(), NS)
                && state == WhoseTurn)
            {
                //NS.ChangeState(state);
                Set(i, state);
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
                //Debug.Log("Nth space: " + iter + " = " + (GetNthSpace(iter) == null));
                if (GetNthSpace(iter) == null)
                {
                    count++;
                }
            }
            //Debug.Log("Count: " + count);
            if (iter < 10)
            {
                switch (iter - 1)
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

    public bool CheckForWin()
    {
        bool isWinner = CheckForWin(grid);
        if (Game_Manager != null)
        {
            if (isWinner || IsBoardFull())
            {
                Game_Manager.UpdateWinner(GetWinner());
            }
        }
        return isWinner;
    }

    public bool CheckForWin(bool?[,] grid)
    {
        //DebugGridPrintout();
        bool debug = false;
        bool? winner = null;

        // ROWS
        // TOP
        if (grid[0, 0] != null && grid[0, 0] == grid[0, 1] && grid[0, 0] == grid[0, 2])
        { if (debug) { Debug.Log("Top Win"); } winner = grid[0, 0]; }
        //Middle
        else if (grid[1, 0] != null && grid[1, 1] == grid[1, 0] && grid[1, 0] == grid[1, 2])
        { if (debug) { Debug.Log("Middle Row Win"); } winner = grid[1, 0]; }
        // Bottom
        else if (grid[2, 0] != null && grid[2, 2] == grid[2, 1] && grid[2, 2] == grid[2, 0])
        { if (debug) { Debug.Log("Bottom Win"); } winner = grid[2, 2]; }

        //COLUMNS 
        //LEFT
        else if (grid[0, 0] != null && grid[0, 0] == grid[1, 0] && grid[0, 0] == grid[2, 0])
        { if (debug) { Debug.Log("Left Win"); } winner = grid[0, 0]; }
        // Middle
        else if (grid[0, 1] != null && grid[0, 1] == grid[1, 1] && grid[1, 1] == grid[2, 1])
        { if (debug) { Debug.Log("Middle Column Win"); } winner = grid[1, 1]; }
        //Right
        else if (grid[0, 2] != null && grid[2, 2] == grid[1, 2] && grid[2, 2] == grid[0, 2])
        { if (debug) { Debug.Log("Right Win"); } winner = grid[2, 2]; }

        //Slash
        else if (grid[1, 1] != null && grid[1, 1] == grid[0, 2] && grid[1, 1] == grid[2, 0])
        { if (debug) { Debug.Log("Slash Win"); } winner = grid[1, 1]; }
        // Back-Slash
        else if (grid[0, 0] != null && grid[0, 0] == grid[1, 1] && grid[0, 0] == grid[2, 2])
        { if (debug) { Debug.Log("Backslash Win"); } winner = grid[0, 0]; }

        // Handle winner
        if (grid == this.grid)
        {
            this.winner = winner;

            if (winner == false)
            { Debug.Log("Circle Wins"); }
            else if (winner == true)
            { Debug.Log("Cross Wins"); }
        }
        return (winner != null);
    }

    private void DebugGridPrintout()
    {
        Debug.Log('\n' +
            (grid[0, 0] == null ? "-1" : grid[0, 0] == true ? "X" : "O") + " " +
            (grid[0, 1] == null ? "-1" : grid[0, 1] == true ? "X" : "O") + " " +
            (grid[0, 2] == null ? "-1" : grid[0, 2] == true ? "X" : "O") + '\n' +

            (grid[1, 0] == null ? "-1" : grid[1, 0] == true ? "X" : "O") + " " +
            (grid[1, 1] == null ? "-1" : grid[1, 1] == true ? "X" : "O") + " " +
            (grid[1, 2] == null ? "-1" : grid[1, 2] == true ? "X" : "O") + '\n' +

            (grid[2, 0] == null ? "-1" : grid[2, 0] == true ? "X" : "O") + " " +
            (grid[2, 1] == null ? "-1" : grid[2, 1] == true ? "X" : "O") + " " +
            (grid[2, 2] == null ? "-1" : grid[2, 2] == true ? "X" : "O") + '\n'
            );
    }

    public bool? GetWinner()
    { return winner; }

    public void ResetLevel()
    {
        winner = null;
        turn = true;
        //initialize the Grid
        grid = new bool?[3, 3];
        for (int i = 0; i < 3; i++)
        {
            //Debug.Log("Grid i: " + i);
            grid[i, 0] = null;
            grid[i, 1] = null;
            grid[i, 2] = null;
        }
        for (int i = 0; i < 9; i++)
        {
            NotsAndCrosses[i].GetComponent<NotAndCrossSpace>().Reset();
        }
    }

    /*************
     *  Grid Checks
     */
    public enum TranslationType { NONE, Equal, Right, Left, Full };

    public bool AreTranslations(bool?[,] gridA, bool?[,] gridB)
    {
        TranslationType tt;

        return AreTranslations(gridA, gridB, out tt);
    }

    public bool AreTranslations(bool?[,] gridA, bool?[,] gridB, out TranslationType tt)
    {
        tt = TranslationType.NONE;
        bool ret = false;
        // Translation Type: Equal
        if (GridsEqual(gridA, gridB))
        {
            tt = TranslationType.Equal;
            ret = true;
        }
        else if (gridA[1, 1] == gridB[1, 1]) // Check center
        {
            // Translation Type: Right
            if (
                // Corners
                gridA[0, 0] == gridB[0, 2]
                && gridA[0, 2] == gridB[2, 2]
                && gridA[2, 2] == gridB[2, 0]
                && gridA[2, 0] == gridB[0, 0]

                // Edges
                && gridA[0, 1] == gridB[1, 2]
                && gridA[1, 2] == gridB[2, 1]
                && gridA[2, 1] == gridB[1, 0]
                && gridA[1, 0] == gridB[0, 1]
                )
            {
                tt = TranslationType.Right;
                ret = true;
            }
            // Translation Type: Left
            if (
                // Corners
                gridB[0, 0] == gridA[0, 2]
                && gridB[0, 2] == gridA[2, 2]
                && gridB[2, 2] == gridA[2, 0]
                && gridB[2, 0] == gridA[0, 0]

                // Edges
                && gridB[0, 1] == gridA[1, 2]
                && gridB[1, 2] == gridA[2, 1]
                && gridB[2, 1] == gridA[1, 0]
                && gridB[1, 0] == gridA[0, 1]
                )
            {
                tt = TranslationType.Left;
                ret = true;
            }
            // Translation Type: Full
            if (
                // Corners
                gridA[0, 0] == gridB[2, 2]
                && gridA[0, 2] == gridB[2, 0]
                && gridA[2, 2] == gridB[0, 0]
                && gridA[2, 0] == gridB[0, 2]

                // Edges
                && gridA[0, 1] == gridB[2, 1]
                && gridA[1, 2] == gridB[1, 0]
                && gridA[2, 1] == gridB[0, 1]
                && gridA[1, 0] == gridB[1, 2]
                )
            {
                tt = TranslationType.Full;
                ret = true;
            }

        }
        return ret;
    }

    public bool GridsEqual(bool?[,] gridA, bool?[,] gridB)
    {
        bool ret = false;
        if (gridA[0, 0] == gridB[0, 0]
            && gridA[1, 0] == gridB[1, 0]
            && gridA[2, 0] == gridB[2, 0]
            && gridA[0, 1] == gridB[0, 1]
            && gridA[1, 1] == gridB[1, 1]
            && gridA[2, 1] == gridB[2, 1]
            && gridA[0, 2] == gridB[0, 2]
            && gridA[1, 2] == gridB[1, 2]
            && gridA[2, 2] == gridB[2, 2]
            )
        {
            ret = true;
        }
        return ret;
    }

    public bool?[,] TransformGrid(bool?[,] inputGrid, TranslationType tt)
    {
        bool?[,] newGrid = new bool?[3, 3];
        if (tt == TranslationType.Right)
        {
            newGrid[0, 0] = inputGrid[2, 0];
            newGrid[1, 0] = inputGrid[2, 1];
            newGrid[2, 0] = inputGrid[2, 2];
            newGrid[0, 1] = inputGrid[1, 0];
            newGrid[1, 1] = inputGrid[1, 1];
            newGrid[2, 1] = inputGrid[1, 2];
            newGrid[0, 2] = inputGrid[0, 0];
            newGrid[1, 2] = inputGrid[0, 1];
            newGrid[2, 2] = inputGrid[0, 2];
        }
        else if (tt == TranslationType.Left)
        {
            newGrid[0, 0] = inputGrid[0, 2];
            newGrid[1, 0] = inputGrid[0, 1];
            newGrid[2, 0] = inputGrid[2, 2];
            newGrid[0, 1] = inputGrid[1, 2];
            newGrid[1, 1] = inputGrid[1, 1];
            newGrid[2, 1] = inputGrid[1, 0];
            newGrid[0, 2] = inputGrid[2, 2];
            newGrid[1, 2] = inputGrid[2, 1];
            newGrid[2, 2] = inputGrid[2, 0];
        }
        else if (tt == TranslationType.Full)
        {
            newGrid[0, 0] = inputGrid[2, 2];
            newGrid[1, 0] = inputGrid[1, 2];
            newGrid[2, 0] = inputGrid[0, 2];
            newGrid[0, 1] = inputGrid[2, 1];
            newGrid[1, 1] = inputGrid[1, 1];
            newGrid[2, 1] = inputGrid[0, 1];
            newGrid[0, 2] = inputGrid[2, 0];
            newGrid[1, 2] = inputGrid[1, 0];
            newGrid[2, 2] = inputGrid[0, 0];
        }
        else
        {
            newGrid[0, 0] = inputGrid[0, 0];
            newGrid[1, 0] = inputGrid[1, 0];
            newGrid[2, 0] = inputGrid[2, 0];
            newGrid[0, 1] = inputGrid[0, 1];
            newGrid[1, 1] = inputGrid[1, 1];
            newGrid[2, 1] = inputGrid[2, 1];
            newGrid[0, 2] = inputGrid[0, 2];
            newGrid[1, 2] = inputGrid[1, 2];
            newGrid[2, 2] = inputGrid[2, 2];
        }
        return newGrid;
    }

    public string TranslationTypeToString(TranslationType input){
        string ret = "";
        switch (input)
        {
            case TranslationType.Equal: ret = "Equal"; break;
            case TranslationType.Right: ret = "Right"; break;
            case TranslationType.Left: ret = "Lift"; break;
            case TranslationType.Full: ret = "Full"; break;
            default: ret = "NONE"; break;
        }

        return ret;
    }
}
