using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TTTManager Board = null;
    [SerializeField]
    private TTTAI AI = null;
    [SerializeField]
    private PlayerController PC = null;

    private bool _menuActive = true;
    public bool MenuActive { get { return _menuActive; } set { _menuActive = value; } }

    [SerializeField]
    private GameObject MainMenu = null;
    [SerializeField]
    private Text GameText = null;
    [SerializeField]
    private GameObject GameTitle = null;

    private float _waiting = 0.0f;
    private float WaitTime = 1.0f;

    [SerializeField]
    private Dropdown dd;
    

    private void Start()
    {
    }
    private void Update()
    {
        if (_waiting > 0.0f)
        {
            if (Input.GetMouseButton(0) && (Time.time - _waiting) > WaitTime)
            {
                MainMenu.SetActive(true);
                _waiting = 0.0f;
            }
        }
    }

    public void Play()
    {
        Debug.Log("Play Activated");
        if (PC) { PC.Reading = true; }
        if(AI) { AI.IsActive = true; }
        if(Board) { Board.ResetLevel();
            Board.Active = true;
        }
        if(MainMenu) { MainMenu.SetActive(false); }
        if (GameTitle) { GameTitle.SetActive(false); }
    }

    public void UpdateWinner(bool? win)
    {
        string OWin = "O Wins!";
        string XWin = "X Wins!";
        string Tie = "Tie";

        if(win == null)
        { GameText.text = Tie; }
        else if (win == true)
        { GameText.text = XWin; }
        else { GameText.text = OWin; }
        EndGame();
    }

    public void EndGame()
    {
        PC.Reading = false;
        AI.IsActive = false;
        Board.Active = false;
        GameTitle.SetActive(true);
        _waiting = Time.time;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AIDropdown(int value)
    {
        switch (value)
        {
            case 0:
                AI.Difficulty = TTTAI.DifficultyModes.dm_Easy;
                break;
            case 1:
                AI.Difficulty = TTTAI.DifficultyModes.dm_Medium;
                break;
            case 2:
                AI.Difficulty = TTTAI.DifficultyModes.dm_Hard;
                break;
        }
         
    }

    public void SetPlayerSymbol(bool value)
    {
        PC.Symbol = value;
        AI.Symbol = !value;
    }
}
