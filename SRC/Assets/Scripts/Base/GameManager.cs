using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public double gameCredits = 100000;
    public TextMeshProUGUI playerCredits;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI betText;

    public double playerBet;
    public double playerWin;



    // Start is called before the first frame update
    void Start()
    {
        InitializeCredits();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitializeCredits()
    {
        playerCredits.text = gameCredits.ToString();

        betText.text = playerBet.ToString();
        winText.text = "0";
    }







}
