using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSymbolToggle : MonoBehaviour
{
    private Text tx;

    void Start()
    {
        tx = GetComponentInChildren<Text>();
        if (tx)
        {
            tx.text = "Play X";
        }
    }

    public void ChangeText(bool value)
    {
        if(tx != null){
            if (value)
            { tx.text = "Play X"; }
            else { tx.text = "Play O"; }
        }
    }
}
