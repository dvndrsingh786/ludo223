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
using UnityEngine.SceneManagement;

public class AvatarData : MonoBehaviour
{

    public Image boardImage;
    public Text avtarName;
    public string screenid;
    public static string screenId;

    AvatarDataScipt dataScipt;
    // Start is called before the first frame update
    void Start()
    {
        dataScipt = FindObjectOfType<AvatarDataScipt>();
    }

    public void OnTheneButtonClick()
    {
        screenId = screenid;
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("avatar_id", screenId);
        print(GameManager.Uid + "id ");
        print(ThemeIconScript.screenId + "sceen id ");
        WWW w = new WWW(dataScipt.preveiwUrl, form);
        Debug.Log(dataScipt.preveiwUrl);

        StartCoroutine(Preview(w));
    }
    IEnumerator Preview(WWW w)
    {
        yield return w;
        print("ThemeResponse=" + w.text);

        dataScipt.loadinPanel.SetActive(true);
        if (w.error == null)
        {

            string msg = w.text;

            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            string results = GetDataValue(msg, "message:");
            string status = GetDataValue(msg, "status:");
            dataScipt.imgeUrl = GetDataValue(msg, "image_path:");


            if (results == "Successfull" || status == "True")
            {
                Debug.Log("It is working ");
                Debug.Log(dataScipt.imgeUrl);
                dataScipt.loadinPanel.SetActive(true);
               
                StartCoroutine(PreviewImge(w));
            }
        }
        else
        {
            Debug.Log("Error");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }


    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));
        return value;
    }

    IEnumerator PreviewImge(WWW w)
    {
        yield return w;

        Debug.Log("W" + w);
        JsonData jsonvale = JsonMapper.ToObject(w.text);
        for (int i = 0; i < jsonvale["result_push"].Count; i++)
        {
            dataScipt.imgeUrl = jsonvale["result_push"][i]["image_path"].ToString();
            UnityWebRequest webRequest = UnityWebRequest.Get(dataScipt.imgeUrl);
            Debug.Log(webRequest);
            yield return webRequest.SendWebRequest();
            byte[] bytes = webRequest.downloadHandler.data;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            Sprite profileImage = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            dataScipt.boardPreview.texture = profileImage.texture;
            dataScipt.loadinPanel.SetActive(false);
            dataScipt.skinPanel.SetActive(false);
        }
    }
}
