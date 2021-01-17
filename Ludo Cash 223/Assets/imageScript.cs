using System;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;


public class imageScript : MonoBehaviour
{

    [Header("Kyc Attribute")]

    public string Url;
    public RawImage adharFront;
    public RawImage adharBack;
    public RawImage pancard;

    public string imag1;
    public string image2;
    public string image3;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ImageShow());
    }

    IEnumerator ImageShow()
    {
        WWW www = new WWW(Url);
        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        imag1 = jsonvale["aadhar_first"].ToString();
        UnityWebRequest webRequest = UnityWebRequest.Get(imag1);
        yield return webRequest.SendWebRequest();
        byte[] bytes = webRequest.downloadHandler.data;
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        Sprite profileImage = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        adharFront.texture = profileImage.texture;

        image2 = jsonvale["aadhar_first"].ToString();
        UnityWebRequest webRequest1 = UnityWebRequest.Get(image2);
        yield return webRequest1.SendWebRequest();
        byte[] bytes1 = webRequest1.downloadHandler.data;
        Texture2D tex1 = new Texture2D(2, 2);
        tex1.LoadImage(bytes1);
        Sprite profileImage1 = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex1.width, tex1.height), new Vector2(0.5f, 0.5f), 100.0f);
        adharBack.texture = profileImage1.texture;

        image3 = jsonvale["aadhar_first"].ToString();
        UnityWebRequest webRequest3 = UnityWebRequest.Get(image2);
        yield return webRequest3.SendWebRequest();
        byte[] bytes3 = webRequest3.downloadHandler.data;
        Texture2D tex3 = new Texture2D(2, 2);
        tex3.LoadImage(bytes3);
        Sprite profileImage3 = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f);
        pancard.texture = profileImage3.texture;
    }

   
}
