using System;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
public class UserStaticScript : MonoBehaviour
{
    [Header("Player Profile Data")]

    public Text userName;
    public Text playerCoinsEarning;
    public Text gamePlayed;
    public Text twoPlayer;
    public Text fourPlayer;

    public string playerName;
    public string coins;
    public string totalgamePlayed;
    public string twoplayerPlayed;
    public string fourplayerPlayed;
    public string currentscreen;
    
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
  
}
