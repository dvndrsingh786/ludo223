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

public class PlayWithFriendListScript : MonoBehaviour
{

    [Header("URL")]
    [Space(5)]
    public string friendSearchUrl;
   
    [Header("UI Objects")]
    [Space(5)]
    public InputField searchFriendField;
    public Text nameText;
    public Transform contentPanel;
    public GameObject searchfriendPrefab, scrollPanel;
    
    List<Button> btns = new List<Button>();
    public List<string> names = new List<string>();
    public List<string> Nameid = new List<string>();
    public List<string> referenceId = new List<string>();

    void Start()
    {
        OnGetFriend();
    }
    public void OnGetFriend()
    {
        StartCoroutine(GetFriendList());
    }
    IEnumerator GetFriendList()
    {

        WWW www = new WWW(friendSearchUrl);
        yield return www;
        Debug.Log(www.text);
       JsonData jsonvale = JsonMapper.ToObject(www.text);
       
        for (int i = 0; i < jsonvale["result_push"].Count; i++)
        {  
              SearchPlayerCheck clone = Instantiate(searchfriendPrefab, contentPanel).GetComponent< SearchPlayerCheck>();
                clone.transform.SetParent(contentPanel, false);
            names.Add(jsonvale["result_push"][i]["username"].ToString());
            clone.pname = jsonvale["result_push"][i]["username"].ToString();
            clone.playername.text = clone.pname;
            Nameid.Add(jsonvale["result_push"][i]["id"].ToString());
            clone.uid= jsonvale["result_push"][i]["id"].ToString();
            referenceId.Add(jsonvale["result_push"][i]["ref_code"].ToString());
            clone.ref_uid = jsonvale["result_push"][i]["ref_code"].ToString();
        }
    }
    public void ApplySearchByName()
    {

        if (searchFriendField.text.Length > 0)
        {
            scrollPanel.SetActive(true);
            if (btns.Count != scrollPanel.GetComponentsInChildren<Button>().Length)
            {
              //  btns.Clear();
                btns.AddRange(scrollPanel.GetComponentsInChildren<Button>());
            }
            bool isFound = false;
                
            foreach (Button btn in btns)
            {
                string[] names = btn.GetComponentInChildren<Text>().text.Split(')');
                string playerName = names[0].Trim().ToLower();
                if (playerName.StartsWith(searchFriendField.text.ToLower()))
                {
                    btn.gameObject.SetActive(true);
                    isFound = true;
                }     
                else
                    btn.gameObject.SetActive(false);
            }
            if(!isFound)
            {
                foreach (Button btn in btns)
                {
                    btn.gameObject.SetActive(true);
                }
            }
        }
        else
        { 
        }
    }
    private void Update()
    {
        if (searchFriendField.isFocused)
        {
            ApplySearchByName();
        }
    }
    [Obsolete]
    public void OnFriendSelected(Button btn,string id)
    {
        if (searchFriendField.gameObject.active)
        {
            string[] names = btn.GetComponentInChildren<Text>().text.Split(')');
            nameText.text = names[0].Remove(0, 1);
            Debug.Log(nameText.text.ToString());
            searchFriendField.text = names[0];
            string countryName = searchFriendField.text;
            Debug.Log(countryName);
            scrollPanel.SetActive(false);
            Debug.Log("abc");
            OnPlayerProfileData();
        }
    }
    public void OnPlayerProfileData()
    {
        StartCoroutine(GetPlayerData());
    }
    IEnumerator GetPlayerData()
    {
        string url = "https://api1.ludocashwin.com/public/api/client_details/my_referral_code=" + SearchPlayerCheck.reference_Uid;
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        GameManager.searchUserId = jsonvale["result_push"][0]["uid"].ToString();
        Debug.Log("UID" + GameManager.searchUserId);
    }
}
