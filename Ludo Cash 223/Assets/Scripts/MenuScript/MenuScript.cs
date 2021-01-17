using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour
{
    public string gameUrl;
    public string name;
    [Header("UI Objects")]
    [Space(5)]
    public Transform contentPanel;
    public GameObject gamePrefab, scrollPanel;

    public static Sprite pic;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameList());
    }

    IEnumerator GameList()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        
        // Debug.Log(auth);
        WWW www = new WWW(gameUrl, null, headers);
       
        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        for (int i = 0; i < jsonvale["result"].Count; i++)
        {
           
            AvaiableGameScript avaiableGameScript= Instantiate(gamePrefab, contentPanel).GetComponent<AvaiableGameScript>();
            name = jsonvale["result"][i]["game_name"].ToString();
            avaiableGameScript.gameName.text = name;
            gameUrl = jsonvale["result"][i]["game_image"].ToString();
            UnityWebRequest webRequest = UnityWebRequest.Get(gameUrl);
            yield return webRequest.SendWebRequest();
            byte[] bytes = webRequest.downloadHandler.data;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            Sprite profileImage = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            pic = profileImage;
            avaiableGameScript.gameImage.texture = profileImage.texture;
           
        }
    }
}
