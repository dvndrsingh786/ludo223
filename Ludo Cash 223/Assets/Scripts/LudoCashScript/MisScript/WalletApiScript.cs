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

public class WalletApiScript : MonoBehaviour
{
    [Header("Wallet Attribute")]

    public InputField amount;
    public Text errorMsg;

    public GameObject coinsPanel;
    public GameObject walletPanel;
    public GameObject loadingPanel;
    public GameObject webVeiw;
    string status;
    int point;
    OfflineOnlineApiScript offlineOnlineApiScript;
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

        offlineOnlineApiScript = FindObjectOfType<OfflineOnlineApiScript>();
        if (PlayerPrefs.HasKey("Logintoken"))
        {
            GameManager.Instance.userID = PlayerPrefs.GetString("Logintoken");          
            GameManager.Instance.playfabManager.apiManager.OnPlayerProfileData();
            Debug.Log("Id" + GameManager.Uid);
          //  GameManager.Instance.playfabManager.coinsBuyUrl = "https://ludocashwin.com/razorpay/pay.php?user_id=" + GameManager.Uid + "&amount=" + amount.text;
        }
       
    }

    public void OnWalletButtonClick()
    {
        string walletURL = "https://api1.ludocashwin.com/public/api/add-wallet";
        if (string.IsNullOrEmpty(amount.text))
        {
            errorMsg.text = "Please Enter the Amount.!!";
            StopCoroutine(Error());
            StartCoroutine(Error());
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id",GameManager.Uid);
        form.AddField("amount",amount.text);
        WWW w = new WWW(walletURL, form);
        Debug.Log("URL" + walletURL);
        StartCoroutine(WalletAPI(w));
    }

    IEnumerator WalletAPI(WWW w)
    {
        yield return w;
        print("responce=" + w.text);
        if (w.error == null)
        {

            string msg = w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);

            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
           
            if (results == "Online Amount Added Successfully" || status == "True")
            {
                Debug.Log("Coins Added..!!");
                coinsPanel.SetActive(true);
                amount.text = "";
                walletPanel.SetActive(false);
                loadingPanel.SetActive(true);
                StartCoroutine(CoinsUpdate());
            }
            
        }
        else {
            Debug.Log("error");
        }
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));

        return value;
    }

    IEnumerator Error()
    {
        yield return new WaitForSeconds(0.6f);
        errorMsg.text = "";
        yield return null;
    }

    
    IEnumerator CoinsUpdate()
    {
        Debug.Log("Cha");
        yield return new WaitForSeconds(0.5f);
        coinsPanel.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        loadingPanel.SetActive(false);
        FindObjectOfType<OfflineOnlineApiScript>().OnlineCash();

    }

    public void OnRequestCoinsClick()
    {
       /* GameManager.Instance.playfabManager.apiManager.OnPlayerProfileData();
        GameManager.Instance.playfabManager.apiManager.LoadingPage.SetActive(false);
        GameManager.Instance.playfabManager.apiManager.splashCanvas.SetActive(false);*/
        if (string.IsNullOrEmpty(amount.text))
        {
            errorMsg.text = "Please Enter the Amount.!!";
            StopCoroutine(Error());
            StartCoroutine(Error());
            return;
        }
        Debug.Log("Id"+ GameManager.Uid);
       
        GameManager.Instance.playfabManager.coinsBuyUrl = "https://api1.ludocashwin.com/cashfree/pay.php?name=" + GameManager.playerName+ "&email="+GameManager.emailId + "&number="+GameManager.mobileNumber + "&amount=" + amount.text;
        Debug.Log("URL" + GameManager.Instance.playfabManager.coinsBuyUrl);
        GameManager.Instance.playfabManager.apiManager.OpenWebview();
    }
    
}
