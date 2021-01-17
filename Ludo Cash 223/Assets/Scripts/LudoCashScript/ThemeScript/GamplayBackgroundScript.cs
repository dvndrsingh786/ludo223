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

public class GamplayBackgroundScript : MonoBehaviour
{
    public string imgeUrl;
    public Image boardPreview;
    public GameObject LoadingPanel;
    public static Sprite bg;
    // Start is called before the first frame update
    void Start()
    {
        OnScreen();
    }
    public void OnScreen()
    {
        //StartCoroutine(ThemeConvertor());
        ThemeConverter();
        if(bg!=null)
        {
            bg = boardPreview.sprite;
        }
    }

    void ThemeConverter()
    {
        boardPreview.sprite = bg = ReferenceManager.refMngr.GetBackgroundSprite();
        //bg = ReferenceManager.refMngr.GetBackgroundSprite();
    }

    IEnumerator ThemeConvertor()
    {
        Debug.LogError("Game Manager Uid" + GameManager.Uid);
        string check= "https://api1.ludocashwin.com/public/api/fetch-screen/user_id="+GameManager.Uid;
        Debug.Log(check+"check ");
        WWW www = new WWW("https://api1.ludocashwin.com/public/api/fetch-screen/user_id="+GameManager.Uid);
        yield return www;
        LoadingPanel.SetActive(true);
        Debug.Log(www.text);
       // boardImage.SetActive(false);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        string msg = jsonvale["result_push"][0]["message"].ToString();
        Debug.Log("Mes"+msg);
        string status= jsonvale["result_push"][0]["status"].ToString();
        Debug.Log("Stat"+status);
        if (msg== "default screen")
        {
            imgeUrl = "https://api1.ludocashwin.com/public/images/default_screen.png";
        }
        else
        {
            imgeUrl = jsonvale["result_push"][0]["image_path"].ToString();

        }

            UnityWebRequest webRequest = UnityWebRequest.Get(imgeUrl);
         
            yield return webRequest.SendWebRequest();
            byte[] bytes = webRequest.downloadHandler.data;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            Sprite profileImage = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            boardPreview.sprite = profileImage;
            bg = profileImage;
            LoadingPanel.SetActive(false);
    }
}
