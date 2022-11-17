using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string[] words;
    public GameObject[] drawings;
    public Text[] characterHolders;

    private int wordLength;

    private bool drawingComplete = false;
    private bool wordComplete = false;

    public void ChooseRandomWord()
    {
        //https://www.youtube.com/watch?v=EIUHnp-fxAg
    }
}
