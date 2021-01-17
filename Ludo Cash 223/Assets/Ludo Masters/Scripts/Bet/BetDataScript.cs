using System;
using LitJson;
using System.IO;
using SimpleJSON;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BetDataScript : MonoBehaviour
{

    [Header("Bet Attribute")]

    public string betURL;

    public GameObject betdataPrefab;
    public GameObject onlinebetdataPrefab;
    public Transform betdataPublic;
    public Transform betdataScheduled;
    public GameObject betscrollPanel;

    [Header("PivateBet Attribute")]

    public GameObject privatebetdataPrefab;
    public Transform privatebetdataPublic;
    public GameObject privatebetscrollPanel;

    public bool isTwoplayer = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("BEt data script: " + gameObject.name);
        StartCoroutine(GetBetting());
        StartCoroutine(privateBetting());
    }
    public void OnPlan(bool istwo)
    {
        for (int i = 0; i < betdataPublic.childCount; i++)
        {
            Destroy(betdataPublic.GetChild(i).gameObject);
        }
        for (int i = 0; i < betdataScheduled.childCount; i++)
        {
            Destroy(betdataScheduled.GetChild(i).gameObject);
        }
        isTwoplayer = istwo;
        StartCoroutine(GetBetting());
    }

    IEnumerator GetBetting()
    {
        string nowTime, nowDate;
        using (WWW www = new WWW(betURL))
        {
            yield return www;
            int twoplayerac = 0;
            nowTime = ReferenceManager.refMngr.GetTime();
            nowDate = ReferenceManager.refMngr.GetDate();
            JsonData jsonvale = JsonMapper.ToObject(www.text);
            for (int i = 0; i < jsonvale["result_push"].Count; i++)
            {
                string playerType = jsonvale["result_push"][i]["betting type"].ToString();
                string date = jsonvale["result_push"][i]["game_date"].ToString();
                string endDate = jsonvale["result_push"][i]["game_end_date"].ToString();
                Debug.LogError(endDate);
                string startTime = jsonvale["result_push"][i]["game_start_time"].ToString();
                string endTime = jsonvale["result_push"][i]["game_end_time"].ToString();
                if (isTwoplayer)
                {
                    if (playerType == "2")
                    {
                        //if (ReferenceManager.refMngr.CheckDate(date, nowDate))
                        //{
                        //    if (ReferenceManager.refMngr.CheckTime(startTime, endTime, nowTime))
                        //    {
                        bool isSameDate, isRightTime, isLessThanADay;
                        string bidTime = "";
                        isSameDate = ReferenceManager.refMngr.CheckDate(date, endDate, nowDate);
                        isRightTime = ReferenceManager.refMngr.CheckTime(startTime, endTime, nowTime);
                        isLessThanADay = ReferenceManager.refMngr.IsLessThanADay(date, nowDate, nowTime, startTime);
                        BetScript betScript;
                        if (isSameDate && isRightTime)
                        {
                            betScript = Instantiate(onlinebetdataPrefab, betdataPublic).GetComponent<BetScript>();
                        }
                        else if (isSameDate)
                        {
                            betScript = Instantiate(onlinebetdataPrefab, betdataScheduled).GetComponent<BetScript>();
                        }
                        else betScript = null;
                        if (betScript != null)
                        {
                            betScript.id = jsonvale["result_push"][i]["table_id"].ToString();
                            betScript.betType = jsonvale["result_push"][i]["betting type"].ToString();
                            float bettingValue = float.Parse(jsonvale["result_push"][i]["betting value "].ToString());
                            float winningAmount = float.Parse(jsonvale["result_push"][i]["winning amount"].ToString());
                            //betScript.betAmount.text = "Entry Fee:" + bettingValue.ToString();
                            betScript.betAmount.text = bettingValue.ToString();
                            if (isSameDate && isRightTime)
                            {
                                bidTime = "Table opened from: " + startTime + " to " + endTime;
                                //betScript.betTime.GetComponent<Text>().fontStyle = FontStyle.Bold;
                                betScript.GetComponent<Button>().interactable = true;
                                betScript.myToggle.interactable = true;
                                betScript.betTime.transform.parent.gameObject.SetActive(true);
                                //betScript.UpdateClock();
                            }
                            else if (isSameDate)
                            {
                                bidTime = "Table will Open at: " + startTime + " to " + endTime;
                                betScript.GetComponent<Button>().interactable = false;
                                betScript.myToggle.interactable = false;
                                betScript.betTime.gameObject.SetActive(true);
                                //betScript.UpdateClock();
                            }
                            //For countdown
                            //else
                            //{
                            //    if (isLessThanADay)
                            //    {
                            //        betScript.hr = ReferenceManager.refMngr.hour;
                            //        betScript.mns = ReferenceManager.refMngr.minutes;
                            //        betScript.secs = ReferenceManager.refMngr.seconds;
                            //        bidTime = betScript.hr + ":" + betScript.mns + ":" + betScript.secs;
                            //        betScript.UpdateClock();
                            //    }
                            //    else
                            //    {
                            //        bidTime = "Date: " + date + "\n" + "Time: " + startTime;
                            //    }
                            //    //betScript.betTime.GetComponent<Text>().fontStyle = FontStyle.Normal;
                            //    betScript.GetComponent<Button>().interactable = false;
                            //    betScript.myToggle.interactable = false;
                            //    betScript.betTime.gameObject.SetActive(true);
                            //}
                            betScript.openParent = betdataPublic;
                            betScript.betTime.text = bidTime;
                            betScript.winningAmount.text = "Winning Fee:" + winningAmount.ToString();
                            betScript.Callingfunction = twoplayerac;
                        }
                        twoplayerac++;
                        //    }
                        //}
                    }
                }
                else
                {
                    if (playerType == "4")
                    {
                        BetScript betScript = Instantiate(betdataPrefab, betdataPublic).GetComponent<BetScript>();
                        betScript.id = jsonvale["result_push"][i]["table_id"].ToString();
                        betScript.betType = jsonvale["result_push"][i]["betting type"].ToString();

                        float bettingValue = float.Parse(jsonvale["result_push"][i]["betting value "].ToString());
                        float winningAmount = float.Parse(jsonvale["result_push"][i]["winning amount"].ToString());
                        betScript.betAmount.text = "Entry Fee:" + bettingValue.ToString();
                        betScript.winningAmount.text = "Winning Fee:" + winningAmount.ToString();
                        betScript.Callingfunction = twoplayerac;
                        twoplayerac++;
                    }
                }
            }
        }
    }

    bool isprivatetable = false;
    IEnumerator privateBetting()
    {
        using (WWW www = new WWW(betURL))
        {
            yield return www;
            Debug.Log("API Call" + www.text);
            int twoplayerac = 0;
            JsonData jsonvale = JsonMapper.ToObject(www.text);
            for (int i = 0; i < jsonvale["result_push"].Count; i++)
            {
                string playerType = jsonvale["result_push"][i]["betting type"].ToString();
                if (!isprivatetable)
                {
                    if (playerType == "4")
                    {
                        PrivateBetScript betScript = Instantiate(privatebetdataPrefab, privatebetdataPublic).GetComponent<PrivateBetScript>();
                        betScript.id = jsonvale["result_push"][i]["table_id"].ToString();
                        float bettingValue = float.Parse(jsonvale["result_push"][i]["betting value "].ToString());
                        float winningAmount = float.Parse(jsonvale["result_push"][i]["winning amount"].ToString());
                        //betScript.betAmount.text = "Entry Fee:" + bettingValue.ToString();
                        betScript.betAmount.text = bettingValue.ToString();
                        betScript.winningAmount.text = "Winning Fee:" + winningAmount.ToString();
                        betScript.Callingfunction = twoplayerac;
                        twoplayerac++;
                    }
                }
            }
        }
    }
}