using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using AssemblyCSharp;

public class TempGameManager : MonoBehaviour
{
    public bool lostFocus;
    public static TempGameManager tempGM;
    public List<int> theIndex;
    public List<bool> theIndexVal;
    public PhotonView view;
    public GameGUIController controller;
    public bool iamalive = true;

    void Start()
    {
        PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;
        tempGM = this;

        if (GameManager.Instance.type == MyGameType.Private)
        {
            ReferenceManager.refMngr.ChangeWinningAmountManually(GameManager.Instance.currentBetAmount, PhotonNetwork.room.PlayerCount);
        }

        if (PhotonNetwork.inRoom)
        {
            if (PhotonNetwork.room.CustomProperties != null)
            {
                if (PhotonNetwork.room.CustomProperties.ContainsKey("bt"))
                {
                    GameManager.Instance.doesContainBotMoves = true;
                }
            }
        }
        Invoke(nameof(SetTimerNames),0.8f);
    }

    void SetTimerNames()
    {
        GameGUIController contrlr = FindObjectOfType<GameGUIController>();
        UpdatePlayerTimer timr1 = contrlr.PlayersTimers[0].GetComponent<UpdatePlayerTimer>();
        UpdatePlayerTimer timr2 = contrlr.PlayersTimers[1].GetComponent<UpdatePlayerTimer>();
        UpdatePlayerTimer timr3 = contrlr.PlayersTimers[2].GetComponent<UpdatePlayerTimer>();
        UpdatePlayerTimer timr4 = contrlr.PlayersTimers[3].GetComponent<UpdatePlayerTimer>();
        if (contrlr.playerObjects.Count > 2)    
        {
            timr1.myPlayerName = contrlr.playerName.text;
            timr2.myPlayerName = contrlr.playerName2.text;
            timr3.myPlayerName = contrlr.playerName3.text;
            timr4.myPlayerName = contrlr.playerName4.text;
        }
        else
        {
            timr1.myPlayerName = contrlr.playerName.text;
            timr3.myPlayerName = contrlr.playerName3.text;
        }
    }

    [PunRPC]
    public void SetAliveState(bool state)
    {
        iamalive = state;
    }

    [PunRPC]
    public void SetCurrentPlayerIndex(int index)
    {
        FindObjectOfType<GameGUIController>().SetCurrentPlayerIndexDav(index);
    }

    //[PunRPC]
    //public void SetIndexes(int index, bool val)
    //{
    //    theIndex.Add(index);
    //    theIndexVal.Add(val);
    //    theIndex.Sort();
    //    theIndexVal.Sort();
    //}

    //[PunRPC]
    //public void SetIndex(int index, bool val, string info)
    //{
    //    Debug.LogError("Setting index from: " + info);
    //    for (int i = 0; i < theIndex.Count; i++)
    //    {
    //        if (theIndex[i].Equals(index))
    //        {
    //            Debug.LogError("Bruh" + i);
    //            theIndexVal[i] = val;
    //        }
    //    }
    //}

    //public bool GetIndex(int index)
    //{
    //    for (int i = 0; i < theIndex.Count; i++)
    //    {
    //        if (theIndex[i].Equals(index))
    //        {
    //            return theIndexVal[i]; 
    //        }
    //    }
    //    return false;
    //}

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus == false)
        {
            IamBackRPC();
        }
        Debug.LogWarning("On Application pause: " + pauseStatus);
    }

    void IamBackRPC()
    {
        view.RPC("SomeoneISBack", PhotonTargets.Others);
    }

    [PunRPC]
    public void SomeoneISBack()
    {
        Debug.LogWarning("Someone is back");
        SendTurns();
    }

    void SendTurns()
    {
        GameGUIController contrlr = FindObjectOfType<GameGUIController>();
        UpdatePlayerTimer timr1 = contrlr.PlayersTimers[0].GetComponent<UpdatePlayerTimer>();
        UpdatePlayerTimer timr2 = contrlr.PlayersTimers[1].GetComponent<UpdatePlayerTimer>();
        UpdatePlayerTimer timr3 = contrlr.PlayersTimers[2].GetComponent<UpdatePlayerTimer>();
        UpdatePlayerTimer timr4 = contrlr.PlayersTimers[3].GetComponent<UpdatePlayerTimer>();
        Debug.LogWarning("Send Turns");
        view.RPC("SendTurns", PhotonTargets.Others, timr1.turnCount, timr2.turnCount, timr3.turnCount, timr4.turnCount, timr1.myPlayerName,timr2.myPlayerName,timr3.myPlayerName,timr4.myPlayerName);
    }
    [PunRPC]
    public void SendTurns(int one, int two, int three, int four, string id1, string id2, string id3, string id4)
    {
        Debug.LogWarning("Send Turns with params");
        //for (int i = 0; i < 4; i++)
        //{
        if (FindObjectOfType<GameGUIController>().playerObjects.Count < 3)
        {
            FindObjectOfType<GameGUIController>().PlayersTimers[2].GetComponent<UpdatePlayerTimer>().SetTurn(one, id1);
            //FindObjectOfType<GameGUIController>().PlayersTimers[i].GetComponent<UpdatePlayerTimer>().SetTurn(two, id2);
            FindObjectOfType<GameGUIController>().PlayersTimers[0].GetComponent<UpdatePlayerTimer>().SetTurn(three, id3);
            //FindObjectOfType<GameGUIController>().PlayersTimers[i].GetComponent<UpdatePlayerTimer>().SetTurn(four, id4);
            //}
        }
    }
}