using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
struct AINode
{
    AINode parent;

    bool?[,] grid;
}*/

public class TTTAI : MonoBehaviour
{
    public enum DifficultyModes { dm_Easy, dm_Medium, dm_Hard }

    TTTManager Grid = null;

    [SerializeField]
    private float SecondsToChange = 2;

    [SerializeField]
    private DifficultyModes difficulty = DifficultyModes.dm_Easy;
    public DifficultyModes Difficulty { get { return difficulty; }
        set {
            Debug.Log("Difficulty set to: " + DifficultytoString(value));
            difficulty = value;
        } }

    private float _timeRemaining = 1.0f;
    [SerializeField]
    private bool _symbol = false;
    public bool Symbol { get { return _symbol; } set { _symbol = value; } }

    public bool IsActive = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Grid");
        if(temp == null)
        {
            //Debug.Log("Could not find object tagged \"Grid\", AI shutting down");
            this.enabled = false;
            return;
        }
        Grid = temp.GetComponent<TTTManager>();
        if(Grid==null)
        {
            //Debug.Log("Could not find Grid Component, AI shutting down");
            this.enabled = false;
            return;
        }

        _timeRemaining = SecondsToChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (Grid.GetWinner() != null)
        {
            //Debug.Log("Game is over.  AI deactivating");
            //this.enabled = false;
        }
        else if (IsActive)
        {
            TakeTurn();
        }
    }

    public void SelectOpenSpace()
    {
        int num = Grid.NumberOfEmptySpaces();
        if (num == 0)
        {
            this.enabled = false;
            //Debug.Log("Grid Full. Ai turning off.");
        }

        int rand = Random.Range(0, num-1);

        //Debug.Log("Inserting Into Nth position: " + rand);
        Grid.InsertNthSpace(rand, _symbol);
    }

    public int ChooseRandomSpot()
    {
        int rand = Random.Range(0, 10);
        bool symbol = (Random.Range(0.0f, 1.0f) > 0.5f) ;

        //Debug.Log("Changing position " + rand + " to " + symbol);

        Grid.InsertNthSpace(rand, _symbol);
        return rand;
    }

    public void Testing()
    {
        float dt = Time.deltaTime;
        _timeRemaining = _timeRemaining - dt;
        //Debug.Log("TimeRemaining: " + _timeRemaining);
        if (_timeRemaining <= 0)
        {
            //ChooseRandomSpot();
            SelectOpenSpace();
            _timeRemaining = SecondsToChange;
            _symbol = !_symbol;
        }
    }

    public void TakeTurn()
    {
        if (Grid.WhoseTurn == _symbol)
        {

            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining < 0.0)
            {
                switch (difficulty)
                {
                    case DifficultyModes.dm_Easy: easyMode(); break;
                    case DifficultyModes.dm_Medium: mediumMode(); break;
                    case DifficultyModes.dm_Hard: mediumMode();break;
                    default: easyMode();break;
                }
                _timeRemaining = SecondsToChange;
            }
        }
    }

    private void easyMode()
    {
        SelectOpenSpace();
    }

    private void mediumMode()
    {
        bool?[,] g =  Grid.GetGrid();

        // Check to see if AI can win.
        for(int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                g = Grid.GetGrid();
                if (g[x,y] == null)
                {
                    g[x, y] = _symbol;
                    if(Grid.CheckForWin(g))
                    {
                        Grid.Set(x, y,_symbol);
                        return;
                    }
                }
            }
        }
        // Else, can the player win in 1 more move?
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                g = Grid.GetGrid();
                if (g[x, y] == null)
                {
                    g[x, y] = !_symbol;
                    if (Grid.CheckForWin(g))
                    {
                        Grid.Set(x, y, _symbol);
                        return;
                    }
                }
            }
        }

        // Else, random spot? 
        SelectOpenSpace();
        return;
    }

    static string DifficultytoString(DifficultyModes dm)
    {
        string ret = "";
        switch (dm)
        {
            case DifficultyModes.dm_Easy:
                ret = "Easy";
                break;
            case DifficultyModes.dm_Medium:
                ret = "Medium";
                break;
            case DifficultyModes.dm_Hard:
                ret = "Hard";
                break;
        }
        return ret;
    }
}
