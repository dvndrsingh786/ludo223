using System;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
public class StoreUpdateScript : MonoBehaviour
{
    public string playerName;
    public static string name;
    public string coins;

    public Text player;
    public Text coin;
    public static string coinId;
    string playerUrl;
    public string choose;
    
    public static StoreUpdateScript storeInstance;
    //public GameGUIController gUIController;

    // Start is called before the first frame update
    private void Awake()
    {
        if (storeInstance == null)
            storeInstance = this;
        
 
    }
 
}
