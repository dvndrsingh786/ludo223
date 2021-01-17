using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSelectionScript : MonoBehaviour
{
    public static DiceSelectionScript instance;
    private void Awake()
    {
        instance = this;
    }
    public Sprite[] Dice1;
    public Sprite[] Dice2;
    public Sprite[] Dice3;
    public Sprite[] Dice4;
    public Sprite[] Dice5;
    public Sprite D1;
    public Sprite D2;
    public Sprite D3;
    public Sprite D4;
    public Sprite D5;
    
}
