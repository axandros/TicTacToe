using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIToggles : MonoBehaviour
{

    [SerializeField]
    private TTTAI.DifficultyModes Difficulty = TTTAI.DifficultyModes.dm_Easy;

    [SerializeField]
    private TTTAI AI = null;

    [SerializeField]
    private Toggle Toggle1 = null;
    [SerializeField]
    private Toggle Toggle2 = null;

    

    public void SetDifficulty()
    {
        AI.Difficulty = Difficulty;
        if (Toggle1) { Toggle1.isOn = false; }
        if (Toggle2) { Toggle2.isOn = false; }
    }
}
