using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spin_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ReelGameDirection ReelDir;
    // Start is called before the first frame update


    public void OnPointerDown(PointerEventData eventData)
    {
        // Spin the reels once the spin button is pressed
        if (ReelDir.GameManager.gameCredits >= ReelDir.GameManager.playerBet)
        {
            UpdateCredits();
            ReelDir.StartGame();
        }
        else
        {
            Debug.Log("Insufficient Credits");
        }

    }


    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void UpdateCredits()
    {
        ReelDir.GameManager.gameCredits -= ReelDir.GameManager.playerBet;
        ReelDir.GameManager.playerCredits.text = ReelDir.GameManager.gameCredits.ToString();
    }


}
