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

public class ProfilePicScript : MonoBehaviour
{
    public string imgeUrl;
    public RawImage boardPreview;
    public RawImage boardPreview1;
    public GameObject LoadingPanel;
    // Start is called before the first frame update
    void Start()
    {
        OnScreen();
    }
    public void OnScreen()
    {
        StartCoroutine(ThemeConvertor());
    }
    IEnumerator ThemeConvertor()
    {
        if (GameManager.Uid != null) {
            string check = "https://api1.ludocashwin.com/public/api/fetch-avatar/user_id=" + GameManager.Uid;
            WWW www = new WWW(check);
            yield return www;

            Debug.Log(www.text);
            JsonData jsonvale = JsonMapper.ToObject(www.text);

            imgeUrl = jsonvale["result_push"][0]["image_path"].ToString();
            UnityWebRequest webRequest = UnityWebRequest.Get(imgeUrl);
            yield return webRequest.SendWebRequest();
            byte[] bytes = webRequest.downloadHandler.data;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            Sprite profileImage = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            boardPreview.texture = profileImage.texture;
            boardPreview1.texture = profileImage.texture;
        }
        
    }
}
