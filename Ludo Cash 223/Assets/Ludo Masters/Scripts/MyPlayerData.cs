using System;
using System.Collections.Generic;
using AssemblyCSharp;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class MyPlayerData
{

    public static string TitleFirstLoginKey = "TitleFirstLogin";
    public static string TotalEarningsKey = "TotalEarnings";
    public static string GamesPlayedKey = "GamesPlayed";
    public static string TwoPlayerWinsKey = "TwoPlayerWins";
    public static string FourPlayerWinsKey = "FourPlayerWins";
    public static string PlayerName = "PlayerName";
    public static string CoinsKey = "Coins";
    public static string RealCoins = "RealCoins";

    public static string ChatsKey = "Chats";
    public static string EmojiKey = "Emoji";
    public static string AvatarIndexKey = "AvatarIndex";
    public static string FortuneWheelLastFreeKey = "FortuneWheelLastFreeTime";

    public Dictionary<string, UserDataRecord> data;
    private  InitMenuScript menuScript;
    
    void start()
    {
        menuScript = (InitMenuScript)UnityEngine.Object.FindObjectOfType(typeof(InitMenuScript));
    }
    public float GetCoins()
    {
        /*        Debug.Log(data.Keys.ToString());
                if (this.data != null && this.data.ContainsKey(LoginScript.coinv))
                {
                    return int.Parse(this.data[LoginScript.coinv].Value);
                    Debug.Log(int.Parse(this.data[LoginScript.coinv].Value));
                    Debug.Log("if");
                }
                Debug.Log("data" + data);
                Debug.Log(this.data.ContainsKey(LoginScript.coinv));
                Debug.Log(this.data[LoginScript.coinv].Value);
                Debug.Log(StoreUpdateScript.coinId);
               // Debug.Log(this.data.ContainsKey(StoreUpdateScript.coinId));

                if (this.data != null && this.data.ContainsKey(StoreUpdateScript.coinId))
                {
                    int tmp = int.Parse(this.data[StoreUpdateScript.coinId].Value);
                    if (tmp <= 0)
                    {
                        menuScript.dialog.SetActive(true);
                    }

                    return int.Parse(this.data[StoreUpdateScript.coinId].Value);
                    Debug.Log(int.Parse(this.data[StoreUpdateScript.coinId].Value));
                    Debug.Log("if");

                }
                else
                {
                    return 100;
                    Debug.Log("else");
                }*/
        return GameManager.Instance.coinsCount;
    }

    public int GetRealCoins()
    {
        // For Paytm

        int realMoney = PlayerPrefs.GetInt("RealC", 0);
        return realMoney;

        /*if (this.data != null && this.data.ContainsKey(RealCoins))
            return int.Parse(this.data[RealCoins].Value);
        else return 0;*/
    }

    public int GetTotalEarnings()
    {
        return int.Parse(this.data[TotalEarningsKey].Value);
    }

    public int GetTwoPlayerWins()
    {
        return int.Parse(this.data[TwoPlayerWinsKey].Value);
    }

    public int GetFourPlayerWins()
    {
        return int.Parse(this.data[FourPlayerWinsKey].Value);
    }

    public int GetPlayedGamesCount()
    {
        return PlayerPrefs.GetInt(GamesPlayedKey, 0);
        // if (this.data != null)
        //     return int.Parse(this.data[GamesPlayedKey].Value);
    }

    public string GetAvatarIndex()
    {
        Debug.Log(AvatarIndexKey);
        if(this.data.ContainsKey(AvatarIndexKey))
        return this.data[AvatarIndexKey].Value;
        return "0";
    }

    public string GetChats()
    {
        return this.data[ChatsKey].Value;
    }

    public string GetEmoji()
    {
        if (this.data.ContainsKey(EmojiKey))
            return this.data[EmojiKey].Value;
        else return "error";
    }

    public string GetPlayerName()
    {
        Debug.Log("Chal ja");
        if (this.data.ContainsKey(PlayerName))
            return this.data[PlayerName].Value;
        else return "Guest";
    }

   /* public string GetLastFortuneTime()
    {
      if (this.data.ContainsKey(FortuneWheelLastFreeKey))
        {
            return this.data[FortuneWheelLastFreeKey].Value;

        }
        else
        {
            string date = DateTime.Now.Ticks.ToString();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(FortuneWheelLastFreeKey, date);
            UpdateUserData(data);
            return date;
        }
    }*/

    public MyPlayerData() { }
    public MyPlayerData(Dictionary<string, UserDataRecord> data, bool myData)
    {
        this.data = data;

        if (myData)
        {
           
           // GameManager.Instance.avatarMy = GameManager.Instance.playfabManager.staticGameVariables.avatars[1];
           
           // GameManager.Instance.nameMy = GetPlayerName();
        }
        Debug.Log("MY DATA LOADED");

    }

    public void UpdateUserData(Dictionary<string, string> data)
    {

        if (this.data != null)
            foreach (var item in data)
            {
                Debug.Log("SAVE: " + item.Key);
                if (this.data.ContainsKey(item.Key))
                {
                    Debug.Log("AA");
                    this.data[item.Key].Value = item.Value;

                }
            }

        UpdateUserDataRequest userDataRequest = new UpdateUserDataRequest()
        {
            Data = data,
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(userDataRequest, (result1) =>
        {
            Debug.Log("Data updated successfull ");
        }, (error1) =>
        {
            Debug.Log("Data updated error " + error1.ErrorMessage);
        }, null);

    }

    public static Dictionary<string, string> InitialUserData(bool fb)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(TotalEarningsKey, "0");
        data.Add(ChatsKey, "");
        data.Add(EmojiKey, "");
        if (fb)
        {
            data.Add(CoinsKey, StaticStrings.initCoinsCountFacebook.ToString());
            data.Add(AvatarIndexKey, "fb");
        }
        else
        {
          /*  if (LoginScript.coinv != null) {
                string addcoin = (StaticStrings.initCoinsCountGuest + int.Parse(LoginScript.coinv)).ToString();
                Debug.Log(addcoin);


            data.Add(CoinsKey, addcoin);
            }
            else {
                data.Add(CoinsKey, StaticStrings.initCoinsCountGuest.ToString());
            }*/
            data.Add(CoinsKey, StaticStrings.initCoinsCountGuest.ToString());
            //data.Add (AvatarIndexKey, "0");
        }

        data.Add(GamesPlayedKey, "0");
        data.Add(TwoPlayerWinsKey, "0");
        data.Add(FourPlayerWinsKey, "0");

        data.Add(TitleFirstLoginKey, "1");
        data.Add(FortuneWheelLastFreeKey, DateTime.Now.Ticks.ToString());
        return data;
    }

}