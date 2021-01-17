using System;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameConfigrationController : MonoBehaviour
{

    public GameObject TitleText;
    public GameObject bidText;
    public GameObject winText;
    public GameObject MinusButton;
    public GameObject PlusButton;
    public GameObject[] Toggles;
    private int currentBidIndex = 0;
    float betCoins;
    public string currentScreen;
    private MyGameMode[] modes = new MyGameMode[] { MyGameMode.Classic, MyGameMode.Quick, MyGameMode.Master };
    public GameObject privateRoomJoin;
    
    [Obsolete]
    WWW w;
    string status;
    string url;
    private string results;
    
    public string Results
    {
        get
        {
            return results;
        }
    }
    // Use this for initialization
    void Start()
    {
        
    }


 public void ChangeBettingAmount(int x)
    {
        Debug.Log(x+"power ");
        if (x > 0)
        {
            if (GameManager.Instance.type == MyGameType.TwoPlayer)
            {
                //if (GameManager.Instance.currentBettingIndex == GameManager.Instance.initMenuScript.twoPlayerBetting.Count - 1)
                //{
                //    print(GameManager.Instance.currentBettingIndex + "instance");
                //    GameManager.Instance.currentBettingIndex = 0;

                //}
                //else
                //{
                //    GameManager.Instance.currentBettingIndex++;
                //    print(GameManager.Instance.currentBettingIndex + "instance");
                //}
                 GameManager.Instance.currentBettingIndex = x;
                GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex];
                GameManager.Instance.currentBetAmount =float.Parse( GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString());
                GameManager.Instance.currentWinningAmount =float.Parse(GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString());
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
            }
            else
            {
                //if (GameManager.Instance.currentBettingIndex == GameManager.Instance.initMenuScript.fourPlayerBetting.Count - 1)
                //{
                //    print(GameManager.Instance.currentBettingIndex + "instance");
                //    GameManager.Instance.currentBettingIndex = 0;
                //}
                //else
                //{
                //    GameManager.Instance.currentBettingIndex++;
                //    print(GameManager.Instance.currentBettingIndex + "instance");
                //}

               GameManager.Instance.currentBettingIndex = x;
                GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex];
                GameManager.Instance.currentBetAmount =float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString());
                GameManager.Instance.currentWinningAmount =float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString());
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
            }
        }
        else
        {
             if (GameManager.Instance.type == MyGameType.TwoPlayer)
             {
                //if (GameManager.Instance.currentBettingIndex == 0)
                //{
                //    print(GameManager.Instance.currentBettingIndex + "instance");
                //    GameManager.Instance.currentBettingIndex = GameManager.Instance.initMenuScript.twoPlayerBetting.Count - 1;

                //}
                //else
                //{
                //    GameManager.Instance.currentBettingIndex--;
                //    print(GameManager.Instance.currentBettingIndex + "instance");
                //}
                GameManager.Instance.currentBettingIndex = x;
                GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex];
                GameManager.Instance.currentBetAmount = float.Parse( GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString());
                GameManager.Instance.currentWinningAmount = float.Parse(GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString());
                GameManager.Instance.payoutCoins = GameManager.Instance.currentBetAmount;
             }
            else
            {
                //if (GameManager.Instance.currentBettingIndex == 0)
                //{
                //    print(GameManager.Instance.currentBettingIndex + "instance");
                //    GameManager.Instance.currentBettingIndex = GameManager.Instance.initMenuScript.fourPlayerBetting.Count - 1;
                //}
                //else
                //{
                //    GameManager.Instance.currentBettingIndex--;
                //    print(GameManager.Instance.currentBettingIndex + "instance");
                //}
                 GameManager.Instance.currentBettingIndex = x;
                GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex];
                GameManager.Instance.currentBetAmount =float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString());
                GameManager.Instance.currentWinningAmount =float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString());
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
            }
        }

        ShowBettingText();
 }

    public void ShowBettingText()
    {
        Debug.Log("bet amount "+GameManager.Instance.currentBetAmount);
        bidText.GetComponent<Text>().text = "Entry Fee "+GameManager.Instance.currentBetAmount.ToString();
        winText.GetComponent<Text>().text = GameManager.Instance.currentWinningAmount.ToString();
    }

    public void Reconnect()
    {
        if (currentScreen == "Private")
        {
             SetNoOfPlayers(1);
        }
        else
        {
            SetNoOfPlayers(0);
        }
         Debug.Log(PhotonNetwork.connectedAndReady);
        if (PhotonNetwork.connectedAndReady ==false)
        {
            //  GameManager.Instance.playfabManager.Login();
        }
    }
    void OnEnable()
    {
        for (int i = 0; i < Toggles.Length; i++)
        {
            int index = i;
            Toggles[i].GetComponent<Toggle>().onValueChanged.AddListener((value) =>
                {
                    ChangeGameMode(value, modes[index]);
                }
            );
        }

        currentBidIndex = 0;
       // UpdateBid(true);
      //  ShowBettingText();
        Toggles[0].GetComponent<Toggle>().isOn = true;
        GameManager.Instance.mode = MyGameMode.Classic;

        switch (GameManager.Instance.type)
        {
            case MyGameType.TwoPlayer:
                TitleText.GetComponent<Text>().text = "Two Players";
                break;
            case MyGameType.FourPlayer:
                TitleText.GetComponent<Text>().text = "Four Players";
                break;
            case MyGameType.Private:
                TitleText.GetComponent<Text>().text = "Private Room";
                privateRoomJoin.SetActive(true);
                DisabledToggle();
                break;
        }

    }

    void DisabledToggle()
    {
       // bidText.GetComponent<Text>().text = "100";
        Toggles[1].SetActive(false);
        Toggles[2].SetActive(false);
        PlusButton.SetActive(false);
        MinusButton.SetActive(false);
       // currentBidIndex = 100;
       // GameManager.Instance.payoutCoins = 100;
    }

    void OnDisable()
    {
        for (int i = 0; i < Toggles.Length; i++)
        {
            int index = i;
            Toggles[i].GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        }

        privateRoomJoin.SetActive(false);
       // currentBidIndex = 0;
        //UpdateBid(false);
        Toggles[0].GetComponent<Toggle>().isOn = true;
        Toggles[1].GetComponent<Toggle>().isOn = false;
        Toggles[2].GetComponent<Toggle>().isOn = false;
    }

    public void setCreatedProvateRoom()
    {
        GameManager.Instance.JoinedByID = false;
    }

    public void SetNoOfPlayers(int index)
    {
        indexxx = index;
        StartCoroutine(TestRoutine());
        return;
        switch (index)
        {
            case 0:
                GameManager.Instance.type = MyGameType.TwoPlayer;
                GameManager.Instance.currentBettingIndex = 0;
                print(GameManager.Instance.currentBettingIndex + "index value");
                try { GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex]; }
                catch { Debug.LogError("Catch has been called"); }
                try { GameManager.Instance.currentBetAmount = float.Parse(GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString()); }
                catch { Debug.LogError("Catch has been called"); }
                try { GameManager.Instance.currentWinningAmount = float.Parse(GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString()); }
                catch { Debug.LogError("Catch has been called"); }
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
                FindObjectOfType<BetDataScript>().OnPlan(true);
                break;
          case 1:
                GameManager.Instance.type = MyGameType.FourPlayer;
                GameManager.Instance.currentBettingIndex = 0;
                GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex];
                GameManager.Instance.currentBetAmount =float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString());
                
                GameManager.Instance.currentWinningAmount =float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString());
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
                Debug.Log("index "+GameManager.Instance.currentBetAmount);
                Debug.Log("index "+GameManager.Instance.currentWinningAmount);
                FindObjectOfType<BetDataScript>().OnPlan(false);
                break;
        }
        ShowBettingText();
    }

    public int indexxx;

    IEnumerator TestRoutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        switch (indexxx)
        {
            case 0:
                GameManager.Instance.type = MyGameType.TwoPlayer;
                GameManager.Instance.currentBettingIndex = 0;
                print(GameManager.Instance.currentBettingIndex + "index value");
                try {  GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex]; }
                catch { Debug.LogWarning("Catch has been called"); }
                try { GameManager.Instance.currentBetAmount = float.Parse(GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString()); }
                catch { Debug.LogWarning("Catch has been called"); }
                try { GameManager.Instance.currentWinningAmount = float.Parse(GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString()); }
                catch { Debug.LogWarning("Catch has been called"); }
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
                FindObjectOfType<BetDataScript>().OnPlan(true);
                break;
            case 1:
                GameManager.Instance.type = MyGameType.FourPlayer;
                GameManager.Instance.currentBettingIndex = 0;
                GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex];
                GameManager.Instance.currentBetAmount = float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString());

                GameManager.Instance.currentWinningAmount = float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString());
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
                Debug.Log("index " + GameManager.Instance.currentBetAmount);
                Debug.Log("index " + GameManager.Instance.currentWinningAmount);
                FindObjectOfType<BetDataScript>().OnPlan(false);
                break;
        }
        ShowBettingText();
    }

    public void startGame()
    {
        betCoins = GameManager.Instance.currentBetAmount;
        if (betCoins <= GameManager.Instance.coinsCount)
        {
            
            if (GameManager.Instance.type != MyGameType.Private)
            {
                Debug.LogError("not Private");
                GameManager.Instance.facebookManager.startRandomGame();
            }
            else
            {
                if (GameManager.Instance.JoinedByID)
                {
                    Debug.LogError("Joined by id");
                    Debug.Log("Joined by id!");
                    GameManager.Instance.matchPlayerObject.GetComponent<SetMyData>().MatchPlayer();
                }
                else
                {
                    Debug.LogError("Joined and created");
                    Debug.Log("Joined and created");
                    // GameManager.Instance.playfabManager.CreatePrivateRoom();
                    GameManager.Instance.matchPlayerObject.GetComponent<SetMyData>().MatchPlayer();
                }

            }
        }
        else
        {
            GameManager.Instance.dialog.SetActive(true);
            Invoke(nameof(SetLoadingPanelFalse), 0.5f);
        }
    }

    void SetLoadingPanelFalse()
    {
        ReferenceManager.refMngr.loadingPanel.SetActive(false);
    }

    private void ChangeGameMode(bool isActive, MyGameMode mode)
    {
        if (isActive)
        {
            GameManager.Instance.mode = mode;
        }
    }


    public void HideThisScreen()
    {
        gameObject.SetActive(false);
    }
   
}
