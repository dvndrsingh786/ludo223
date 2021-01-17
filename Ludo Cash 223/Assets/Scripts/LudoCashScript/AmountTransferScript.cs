using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountTransferScript : MonoBehaviour
{

    [Header("Amount Attribute")]

    public string amounttransferURL;
    public InputField transferAmount;
    public InputField searchfeild;
    public Text errorText, amountText;
    public GameObject loadingPanel;
    public GameObject amountpopupPanel;
    public GameObject walletPanel;

    public Text playerAmount;
    string status;
    private string results;
    public string Results
    {
        
        get
        {
            return results;
        }
    }
    OfflineOnlineApiScript offlineOnlineApiScript;
    void Start()
    {
        offlineOnlineApiScript = FindObjectOfType<OfflineOnlineApiScript>();
    }

    public void AmountTranfer()
    {
        if (string.IsNullOrEmpty(searchfeild.text))
        {
            errorText.text = "Search Field Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(transferAmount.text))
        {
            errorText.text = "Amount Field Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
       
        Debug.Log(amounttransferURL);
        WWWForm form = new WWWForm();
        form.AddField("transfer_to", GameManager.searchUserId);
        form.AddField("amount", transferAmount.text);
        form.AddField("user_id", GameManager.Uid);
        Debug.Log("transfer_to"+ GameManager.searchUserId);
        Debug.Log("amount"+ transferAmount.text);
        Debug.Log("user_id"+ GameManager.Uid);
        WWW w = new WWW(amounttransferURL, form);
        loadingPanel.SetActive(true);
        StartCoroutine(AccountWalletTransfer(w));
    }

    IEnumerator AccountWalletTransfer(WWW w)
    {
        yield return w;
        print("amount=" + w.text);
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

            print("Results occur" + results);
            if (results == "Amount Transfer Successfully" || status == "True")
            {
                GameManager.Instance.playfabManager.GetAccountCode();
                loadingPanel.SetActive(false);
                amountText.text = results;
                amountpopupPanel.SetActive(true);
                walletPanel.SetActive(false);
                FindObjectOfType<OfflineOnlineApiScript>().OnlineCash();


                playerAmount.text = GameManager.Instance.coinsCount.ToString();
               transferAmount.text = "";
                searchfeild.text = "";
                Debug.Log("Amount Transfer..!!!");
            }
            if (results == "Please Check Your Balance" || status == "True")
            {
                amountText.text = results;
                amountpopupPanel.SetActive(true);
                transferAmount.text = "";
                searchfeild.text = "";
            }
        }
    }
    
    IEnumerator ErrorClose()
    {
        yield return new WaitForSeconds(2f);
        errorText.text = "";
        yield return null;
    }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));

        return value;
    }

    public void SetUPdateProfile(string value)
    {
       
            playerAmount.text =value.ToString();
    }
}
