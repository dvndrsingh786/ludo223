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


public class ThemeScript : MonoBehaviour
{

    [Header("Theme Attribute")]

    public string themeiconUrl;
    public GameObject themeIconPrefab;
    public Transform themeContainer;
    public List<string> screenid = new List<string>();

    [Header("Preview Attribute")]

    public string preveiwUrl;
    public string imgeUrl;
    public Image boardPreview;
    public Transform imgeHolder;
    public GameObject previewPanel;
    public GameObject loadinPanel;
    public GameObject skinPanel;


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
        //StartCoroutine(ThemeConvertor());      
        Invoke(nameof(ThemeConverter), 0.1f);
    }

    void ThemeConverter()
    {
        for (int i = 0; i < ReferenceManager.refMngr.backgroundSprites.Length - 1; i++)
        {
            ThemeIconScript clone = Instantiate(themeIconPrefab, themeContainer).GetComponent<ThemeIconScript>();
            clone.boardImage.sprite = ReferenceManager.refMngr.backgroundSprites[i];
            clone.screenid = (i + 1).ToString();
        }
    }

    IEnumerator ThemeConvertor()
    {
        WWW www = new WWW(themeiconUrl);
        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        for (int i = 0; i < jsonvale["result_push"].Count; i++)
        {
            ThemeIconScript clone = Instantiate(themeIconPrefab,themeContainer).GetComponent<ThemeIconScript>();
            themeiconUrl = jsonvale["result_push"][i]["image_path"].ToString();
            UnityWebRequest webRequest = UnityWebRequest.Get(themeiconUrl);
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

    public void OnTheneButtonClick()
    {
       
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("screen_id", ThemeIconScript.screenId);
        print(GameManager.Uid + "id ");
        print(ThemeIconScript.screenId + "sceen id ");
        WWW w = new WWW(preveiwUrl, form);
        Debug.Log(preveiwUrl);
        StartCoroutine(Preview(w));
    }
    IEnumerator Preview(WWW w)
    {
        yield return w;
        print("Response=" + w.text);
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
            
            if (results == "Successfull" || status == "True")
            {
                Debug.Log("Finally ");
                previewPanel.SetActive(false);
                skinPanel.SetActive(false);
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

}
