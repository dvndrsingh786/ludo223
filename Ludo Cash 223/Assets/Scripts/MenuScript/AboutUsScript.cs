using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
public class AboutUsScript : MonoBehaviour
{
    public string gameUrl;
    public string name;
    public Text discription;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AboutUs());
    }

    IEnumerator AboutUs()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();

        // Debug.Log(auth);
        WWW www = new WWW(gameUrl, null, headers);

        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        for (int i = 0; i < jsonvale["result"].Count; i++)
        {
            name = jsonvale["result"][i]["about_us"].ToString();
            discription.text = name;
        }
    }
}
