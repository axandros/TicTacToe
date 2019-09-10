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

        //Debug.Log("TEST 04: " + Test04());
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
        bool Assertion = false;
        TTTManager.TranslationType translation;

        bool?[,] grid = new bool?[3, 3]
            { {true, null, null},
              {null, null, null},
              {null, null, null} };

        bool?[,] grid2 = new bool?[3, 3]
            { {null, null, null},
              {null, null, null},
              {null, null, false} };

        Assertion = !Man.AreTranslations(grid, grid2, out translation);
        //Debug.Log("Assertion: " + Assertion + " | Translation Type: " + Man.TranslationTypeToString(translation));
        return Assertion && translation == TTTManager.TranslationType.Full;
    }
    
}
