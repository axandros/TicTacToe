using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTGridTranslation : MonoBehaviour
{
    TTTManager Man = null;

    // Start is called before the first frame update
    void Start()
    {
        Man = transform.gameObject.AddComponent<TTTManager>();

        Debug.Log("TEST 01: " + Test01());
        Debug.Log("TEST 02: " + Test02());
        Debug.Log("TEST 03: " + Test03());

        Debug.Log("TEST 04: " + Test04());

        Debug.Log("TEST 05: " + Test05());
        Debug.Log("TEST 06: " + Test06());
    }

    bool TestTranslations(bool?[,] grid1, bool?[,] grid2, bool IsTranslation, TTTManager.TranslationType translation)
    {
        TTTManager.TranslationType tran;
        bool Assertion = Assertion = Man.AreTranslations(grid1, grid2, out tran); ;
        
        bool ret = false;
        if( Assertion == IsTranslation)
        {
            if (tran == translation) {
                return ret = true; }
            else
            { Debug.Log("Translation Type: " + Man.TranslationTypeToString(tran)); } }
        else{ Debug.Log("Assertion: " + Assertion); }

        return ret;
    }

    bool Test01()
    {
        bool?[,] grid = new bool?[3,3]
            { {null, null, null},
              {null, null, null},
              {null, null, null} };

        bool?[,] grid2 = new bool?[3, 3]
            { {null, null, null},
              {null, null, null},
              {null, null, null} };
        return TestTranslations(grid, grid2, true, TTTManager.TranslationType.Equal);
    }
    bool Test02()
    {
        bool?[,] grid = new bool?[3, 3]
            { {null, null, null},
              {null, null, null},
              {null, null, null} };

        bool?[,] grid2 = new bool?[3, 3]
            { {null, null, null},
              {null, true, null},
              {null, null, null} };

        return TestTranslations(grid, grid2, false, TTTManager.TranslationType.NONE);
    }
    bool Test03()
    {
        bool?[,] grid = new bool?[3, 3]
            { {null, null, null},
              {null, null, null},
              {null, null, null} };

        bool?[,] grid2 = new bool?[3, 3]
            { {null, null, null},
              {null, false, null},
              {null, null, null} };

        return TestTranslations(grid, grid2, false, TTTManager.TranslationType.NONE);
    }

    bool Test04()
    {
        bool?[,] grid = new bool?[3, 3]
            { {true, null, null},
              {null, null, null},
              {null, null, null} };

        bool?[,] grid2 = new bool?[3, 3]
            { {null, null, null},
              {null, null, null},
              {null, null, false} };

        return TestTranslations(grid, grid2, false, TTTManager.TranslationType.NONE);
    }

    bool Test05()
    {
        bool?[,] grid = new bool?[3, 3]
            { {true, false, true},
              {true, false, false},
              {false, true, true} };

        bool?[,] gridL = new bool?[3, 3]
            { {true, false, true},
              {false, false, true},
              {true, true, false} };

        bool?[,] gridR = new bool?[3, 3]
            { {false, true, true},
              {true, false, false},
              {true, false, true} };

        bool?[,] gridF = new bool?[3, 3]
            { {true, true, false},
              {false, false, true},
              {true, false, true} };

        bool Right = TestTranslations(grid, gridR, true, TTTManager.TranslationType.Right);
        bool Left = TestTranslations(grid, gridL, true, TTTManager.TranslationType.Left);
        bool Full = TestTranslations(grid, gridF, true, TTTManager.TranslationType.Full);

        return Right && Left && Full;
    }

    bool Test06()
    {
        bool?[,] grid = new bool?[3, 3]
            { {false, true, false},
              {false, true, true},
              {true, false, true} };

        bool?[,] gridL = new bool?[3, 3]
            { {false, true, true},
              {true, true, false},
              {false, false, true} };

        bool?[,] gridR = new bool?[3, 3]
            { {true, false, false},
              {false, true, true},
              {true, true, false} };

        bool?[,] gridF = new bool?[3, 3]
            { {true, false, true},
              {true, true, false},
              {false, true, false} };

        bool Right = TestTranslations(grid, gridR, true, TTTManager.TranslationType.Right);
        bool Left = TestTranslations(grid, gridL, true, TTTManager.TranslationType.Left);
        bool Full = TestTranslations(grid, gridF, true, TTTManager.TranslationType.Full);

        return Right && Left && Full;
    }

}
