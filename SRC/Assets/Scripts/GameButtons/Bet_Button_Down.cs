using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bet_Button_Down : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameManager GameManager;


    int bet;
    int lines = 20;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Sub();
    }


    public void OnPointerUp(PointerEventData eventData)
    {

    }



    void Sub()
    {
        if (GameManager.playerBet >= 20 )
        {
            bet = (int)GameManager.playerBet -  1 * lines;
            GameManager.playerBet = bet;
            GameManager.betText.text = GameManager.playerBet.ToString();
        }

    }





}
