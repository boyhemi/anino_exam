using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Bet_Button_Up : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    int bet;
    int lines = 20;


    public GameManager GameManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        Add();
    }


    public void OnPointerUp(PointerEventData eventData)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add()
    {
        if (GameManager.playerBet <= 2000)
        {
            bet = bet + 1;
            GameManager.playerBet = bet * lines;
            GameManager.betText.text = GameManager.playerBet.ToString();
        }
        else
        {
            Debug.Log("bet exceeded");
        }



    }




}
