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

public class RefferalCodeScript : MonoBehaviour
{

    [Header("Refferal Attribute")]

    public string refferalCodeURL;
    public InputField _referalCode;


    public Text popupText;
    public GameObject popupPanel;
    public GameObject loadingPanel;
    public GameObject friendSharePanel;

    public Text errortext;
    string status;

    string refCodePP = "FriendRefCodePP";

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

    public GameObject inviteFriendPanel;

    public void OnInviteFriendEnabled()
    {
        if (PlayerPrefs.GetString(refCodePP) == "")
        {
            _referalCode.text = PlayerPrefs.GetString(refCodePP);
            _referalCode.interactable = true;
            inviteFriendPanel.SetActive(true);
        }
        else
        {
            _referalCode.text = PlayerPrefs.GetString(refCodePP);
            _referalCode.interactable = false;
            inviteFriendPanel.SetActive(true);
        }
    }

    public void OnClaimPrize()
    {
        if (string.IsNullOrEmpty(_referalCode.text))
        {
            errortext.text = "RefferalCode Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("refer_code", _referalCode.text);
        Debug.Log("Code" + _referalCode.text);
        WWW w = new WWW(refferalCodeURL, form);
        StartCoroutine(ClaimPrize(w));
    }

    IEnumerator ClaimPrize(WWW w)
    {
        yield return w;
        print("responce=" + w.text);
        loadingPanel.SetActive(true);
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
            Debug.Log(results);
            Debug.Log(status);
            if (status == "True")
            {
                Debug.Log("this is perfect..!!!");
                GameManager.Instance.playfabManager.GetReferCode();
                loadingPanel.SetActive(false);
                popupPanel.SetActive(true);
                PlayerPrefs.SetString(refCodePP, _referalCode.text);
                _referalCode.interactable = false;
                StartCoroutine(popUpPanel());
                FindObjectOfType<OfflineOnlineApiScript>().OnlineCash();
                //_referalCode.text = "";
                friendSharePanel.SetActive(false);
            }
            if (status == "False")
            {
                //_referalCode.text = "";
                loadingPanel.SetActive(false);
                popupText.text = results;
                popupPanel.SetActive(true);
                StartCoroutine(popUpPanel());
            }
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
        errortext.text = "";
        yield return null;
    }
    IEnumerator popUpPanel()
    {
        yield return new WaitForSeconds(0.5f);
        popupPanel.SetActive(false);
    }
}
