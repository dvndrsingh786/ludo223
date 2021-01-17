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

public class AvatarDataScipt : MonoBehaviour
{
    [Header("Player Attribute")]

    public string avataronUrl;
    public GameObject avatarIconPrefab;
    public Transform avatarContainer;
    public GameObject avatarScrollBar;
    public List<string> screenid = new List<string>();

    public string preveiwUrl;
    public string imgeUrl;
    public RawImage boardPreview;
    public GameObject loadinPanel;
    public GameObject skinPanel;
    // Start is called before the first frame update

    void Start()
    {

        StartCoroutine(AvatarConvertor());
    }

    IEnumerator AvatarConvertor()
    {
        WWW www = new WWW(avataronUrl);
        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        for (int i = 0; i < jsonvale["result_push"].Count; i++)
        {
            AvatarData clone = Instantiate(avatarIconPrefab, avatarContainer).GetComponent<AvatarData>();
            string name = jsonvale["result_push"][i]["name"].ToString();
            clone.avtarName.text = name;
            avataronUrl = jsonvale["result_push"][i]["image_path"].ToString();
            UnityWebRequest webRequest = UnityWebRequest.Get(avataronUrl);
            yield return webRequest.SendWebRequest();
            byte[] bytes = webRequest.downloadHandler.data;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            Sprite profileImage = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            clone.boardImage.sprite = profileImage;
            screenid.Add(jsonvale["result_push"][i]["id"].ToString());
            clone.screenid = jsonvale["result_push"][i]["id"].ToString();
        }
    }

}
