using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelGameDirection : MonoBehaviour
{

    public ReelNumberGenerator RNGReel;
    public GameManager GameManager;

    private void Start()
    {

    }


    public void StartGame()
    {
        RNGReel.SpinReels();
    }



    public void AnimateReels()
    {


    }







}