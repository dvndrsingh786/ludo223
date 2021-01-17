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


public class ThemeIconScript : MonoBehaviour
{

    public Image boardImage;
    public string screenid;   
    public static string screenId;
    ThemeScript iconScript;

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
    void Awake()
    {
        iconScript = FindObjectOfType<ThemeScript>();
    }


    public void OnTheneButtonClick()
    {
        screenId = screenid;
        PlayerPrefs.SetInt(ReferenceManager.refMngr.BGThemePP, int.Parse(screenid));
        iconScript.boardPreview.sprite = ReferenceManager.refMngr.GetBackgroundSprite();
        iconScript.previewPanel.SetActive(true);
        Debug.LogError("Theme ID" + PlayerPrefs.GetInt(ReferenceManager.refMngr.BGThemePP));
    }
    #region Old Way to set theme
    /*        WWWForm form = new WWWForm();
            form.AddField("user_id", GameManager.Uid);
            form.AddField("screen_id", screenId);
            print(GameManager.Uid + "id ");
            print(ThemeIconScript.screenId + "sceen id ");
            WWW w = new WWW(iconScript.preveiwUrl, form);
            Debug.Log(iconScript.preveiwUrl);

            StartCoroutine(Preview(w));
}
IEnumerator Preview(WWW w)
{
    yield return w;
    print("ThemeResponse=" + w.text);

    iconScript.loadinPanel.SetActive(true);
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
        iconScript.imgeUrl = GetDataValue(msg, "image_path:");


        if (results == "Successfull" || status == "True")
        {
            Debug.Log("It is working ");
            Debug.Log(iconScript.imgeUrl);
            iconScript.loadinPanel.SetActive(true);
            StartCoroutine(PreviewImge(w));
        }
    }
    else
    {
        Debug.Log("Error");
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
*/
    #endregion

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));
        return value;
    }

    #region Old Way to Set Theme 2
    /*
    IEnumerator PreviewImge( WWW w)
    {
        yield return w;

        Debug.Log("W" + w);
        JsonData jsonvale = JsonMapper.ToObject(w.text);
        for (int i = 0; i < jsonvale["result_push"].Count; i++)
        {
            iconScript.imgeUrl = jsonvale["result_push"][i]["image_path"].ToString();
            UnityWebRequest webRequest = UnityWebRequest.Get(iconScript.imgeUrl);
            Debug.Log(webRequest);
            yield return webRequest.SendWebRequest();
            byte[] bytes = webRequest.downloadHandler.data;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            Sprite profileImage = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            iconScript.boardPreview.sprite = profileImage;
            iconScript.loadinPanel.SetActive(false);
            iconScript.previewPanel.SetActive(true);
        }
    }*/
    #endregion

}
