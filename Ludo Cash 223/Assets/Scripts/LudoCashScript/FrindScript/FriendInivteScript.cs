using System;
using LitJson;
using System.IO;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class FriendInivteScript : MonoBehaviour
{
    [Header("Friend Notification")]

    public string notificationUrl;


    [Header("GameObjet Attribute")]

    public GameObject joinedPrefab;
    public GameObject unjoinedPrefab;
    public GameObject joinedScroll;
    public GameObject unjoinedScroll;
    public Transform joinedContentPanel;
    public Transform unjoinedContentPanel;
    public GameObject loadingPanel;
    public GameObject friendPanel;
    public GameObject frindPopupPanel;

    string status;

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


    public void OnNotificationFriendPanel() {
        for (int i = 0; i < joinedContentPanel.childCount; i++)
        {
            Destroy(joinedContentPanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < unjoinedContentPanel.childCount; i++)
        {
            Destroy(unjoinedContentPanel.GetChild(i).gameObject);
        }
        friendPanel.SetActive(true);
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        WWW w = new WWW(notificationUrl, form);
        StartCoroutine(FriendNotifi(w));
        loadingPanel.SetActive(true);
    }

    IEnumerator FriendNotifi(WWW w)
    {
        yield return w;
        Debug.Log(w.text);
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
            JsonData jsonvale = JsonMapper.ToObject(w.text);
            if (results == "Referred History" || status == "True")
            {
                loadingPanel.SetActive(false);
                for (int i = 0; i < jsonvale["joined"].Count; i++) {
                    FriendJoinScript friendJoinScript = Instantiate(joinedPrefab, joinedContentPanel).GetComponent<FriendJoinScript>();
                    string name = jsonvale["joined"][i]["fullname"].ToString();
                    friendJoinScript.playername.text = name;
                    string mobile= jsonvale["joined"][i]["mobile"].ToString();
                    friendJoinScript.playerMobile.text = mobile;
                  /*  string imageUrl= jsonvale["joined"][i]["profile_pic"].ToString();
                    UnityWebRequest webRequest = UnityWebRequest.Get(imageUrl);
                    Debug.Log(webRequest);
                    yield return webRequest.SendWebRequest();
                    byte[] bytes = webRequest.downloadHandler.data;
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(bytes);
                    Sprite profileImage = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    friendJoinScript.playerImage.texture= profileImage.texture;*/
                }
              /*  for (int j = 0; j < jsonvale["unjoined"].Count; j++)
                {
                    UnjoinedFrindScript friendJoinScript = Instantiate(unjoinedPrefab, unjoinedContentPanel).GetComponent<UnjoinedFrindScript>();
                    string name = jsonvale["unjoined"][j]["mobile"].ToString();
                    friendJoinScript.playername.text = name;
                }*/
            }
            if (results == "Data not found" || status == "False")
            {
                Debug.Log("No Data ");
                loadingPanel.SetActive(false);
                friendPanel.SetActive(false);
                frindPopupPanel.SetActive(true);
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
}
