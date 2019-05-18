using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTAI : MonoBehaviour
{
    TTTManager Grid;

    [SerializeField]
    private float SecondsToChange = 2;

    private float _timeRemaining;
    private bool _symbol;

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
        float dt = Time.deltaTime;

        _timeRemaining = _timeRemaining - dt;
        //Debug.Log("TimeRemaining: " + _timeRemaining);
        if(_timeRemaining <= 0)
        {
            //ChooseRandomSpot();
            SelectOpenSpace();
            _timeRemaining = SecondsToChange;
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

        Debug.Log("Inserting Into Nth position: " + rand);
        Grid.InsertNthSpace(rand, _symbol);

        _symbol = !_symbol;
    }

    public int ChooseRandomSpot()
    {
        int rand = Random.Range(0, 10);
        bool symbol = (Random.Range(0.0f, 1.0f) > 0.5f) ;

        //Debug.Log("Changing position " + rand + " to " + symbol);

        Grid.InsertNthSpace(rand, _symbol);
        return rand;
    }
}
