using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  BettingType{TwoPlayer,FourPlayer }
[System.Serializable]
public class Betting 
{
    public BettingType bettingType;
    public float bettingValue = 0;
    public float winningAmount = 0;

    public Betting(BettingType b,float bv,float wa)
    {
        bettingType = b;
        bettingValue = bv;
        winningAmount = wa;
        
    }
}
