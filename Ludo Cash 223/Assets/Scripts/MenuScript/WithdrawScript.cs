using System;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class WithdrawScript : MonoBehaviour
{
    [Header("SignUp Object")]

    public InputField _name;
    public InputField _amount;
    public InputField _mobileNumber;
    public Text errorMsg;
    public GameObject withdrawalPanel;
    public GameObject notificationPanel;
    public GameObject loginPanel;
    
    public string choose;
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
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void OnWithDrawBtnClick()
    {
        
         
            if (string.IsNullOrEmpty(_name.text))
            {
                errorMsg.text = "Name Can't be Blank";
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
                return;
            }

            if (string.IsNullOrEmpty(_amount.text))
            {
                errorMsg.text = "Amount Can't be Blank";
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
                return;
            }
            if (string.IsNullOrEmpty(_mobileNumber.text))
            {
                errorMsg.text = "Mobile Can't be Blank";
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
                return;
            }
            
       

            WWWForm form = new WWWForm();
            form.AddField("my_referral_code", PlayerPrefs.GetString("token"));
            form.AddField("name", _name.text);
            form.AddField("mobile", _mobileNumber.text);
            form.AddField("coins", _amount.text);
            url = "http://onlystore.in/api/withdraw.php?my_referral_code=" + GameManager.Instance.userID + "&name=" + _name.text + "&mobile=" + _mobileNumber.text
                  + "&amount=" + _amount.text+"&gameid=6";

            Debug.Log(url);
            WWW w = new WWW(url);
            StartCoroutine(Withdraw(w));
        
    }
    IEnumerator Withdraw(WWW _w)
    {
        yield return _w;
        print("responce=" + _w.text);
        if (_w.error == null)
        {

            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            if (results == "withdrawal amount requested" || status == "True")
            {
                withdrawalPanel.SetActive(false);
                notificationPanel.SetActive(true);
                loginPanel.SetActive(false);
                _name.text = "";
                _amount.text = "";
                _mobileNumber.text = "";
            }
            else
            {
                errorMsg.text = results;
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());

        }
    }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));

        return value;
    }
    IEnumerator ErrorClose()
    {
        yield return new WaitForSeconds(2f);
        errorMsg.text = "";
        yield return null;
    }
}
