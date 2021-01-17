using System;
using LitJson;
using System.IO;
using SimpleJSON;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class OfflineOnlineApiScript : MonoBehaviour
{

    [Header("Online Attribute")]

    public string onlineAmountURL;
    public string totalOnlineAmount;
    public Text onlineAmount;
    public Text RedemText;
    public Text upiText;
    public Text BankText;    
    string status;

    [Header("Reach us")]

    public string reachusURL;
    public InputField nameText;
    public InputField messageText;
    public Text errorMsg;


    public GameObject loadingPanel;
    public GameObject popupPanel;
    public GameObject menuPanel;
    private string results;
    public string Results
    {
        get
        {
            return results;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        OnlineCash();
    }

    public void OnlineCash()
    {
        StartCoroutine(OnlineAmount());
    }
    int number;
    IEnumerator OnlineAmount()
    {
        WWW www = new WWW(onlineAmountURL + GameManager.Uid);
        Debug.Log("ONUrl" + onlineAmountURL + GameManager.Uid);
        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        totalOnlineAmount = jsonvale["result_push"][0]["total_amount"].ToString();
        number++;
        Debug.Log(totalOnlineAmount+"     Running Time   "+number);
      if(onlineAmount!=null)
             onlineAmount.text = totalOnlineAmount;
       if (RedemText != null)
            RedemText.text = totalOnlineAmount;
       if (upiText != null)
            upiText.text = totalOnlineAmount;
       if (BankText != null)
            BankText.text = totalOnlineAmount;

       
        GameManager.Instance.coinsCount = float.Parse(totalOnlineAmount);
        AmountTransferScript amsender = FindObjectOfType<AmountTransferScript>();
        if(amsender!=null)
            amsender.SetUPdateProfile(totalOnlineAmount.ToString());
    }
    private void OnEnable()
    {
        OnlineCash();
    }

    public void OnReachUs()
    {
        if (string.IsNullOrEmpty(nameText.text))
        {
            errorMsg.text = "Name Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }

        if (string.IsNullOrEmpty(messageText.text))
        {
            errorMsg.text = "Message Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("name", nameText.text);
        form.AddField("query", messageText.text);
        WWW w = new WWW(reachusURL, form);
        loadingPanel.SetActive(true);
        StartCoroutine(Reachus(w));
    }

    IEnumerator Reachus(WWW w)
    {
        yield return w;

        print("responce=" + w.text);
        if (w.error == null)
        {
            JsonData jsonvale = JsonMapper.ToObject(w.text);
           string status = jsonvale["status"].ToString();
            if (status == "true")
            {
                loadingPanel.SetActive(false);
                menuPanel.SetActive(false);
                popupPanel.SetActive(true);
                nameText.text = "";
                messageText.text = "";
            }
        }
    }
    IEnumerator ErrorClose()
    {
        yield return new WaitForSeconds(2f);
        errorMsg.text = "";
        yield return null;
    }
}
