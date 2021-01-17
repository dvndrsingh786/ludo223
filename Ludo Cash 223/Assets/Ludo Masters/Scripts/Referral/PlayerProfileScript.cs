using System;
using Kakera;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PlayerProfileScript : MonoBehaviour
{
    Unimgpicker imagePicker=new Unimgpicker();

    public RawImage profileIC;

    int valuecon;

    private int[] sizes = { 1024, 256, 16 };
    RawImage value;

    [Header("String Attribute")]

    public string imagePostURL;

    public InputField nameInputFeild;
    public GameObject loadingPanel;
    public GameObject popupPanel;
    public GameObject kycPanel;
    string status;
    public string val = "1";
    private string results;
    public string Results
    {
        get
        {
            return results;
        }
    }

    void Awake()
    {
        imagePicker = FindObjectOfType<Unimgpicker>();
        setintialize();

    }
    private void Start()
    {
        val = "0";
        SetNoOfPlayers(0);
    }
    void setintialize()
    {
        print(valuecon + "value dot");
        if (valuecon == 0)
        {
            value = profileIC;

        }

        imagePicker.Completed1 += (string path) =>
        {
            print("Profile get");
            StartCoroutine(LoadImage1(path,profileIC));
        };

    }
   /* public void OnPressShowPicker()
    {
        var picker = FindObjectOfType<PickerController>();
        if(picker!=null)
        {
            picker.OnPressShowPicker3(profileIC);
        }
     

    }*/

    private IEnumerator LoadImage1(string path,RawImage pro)
    {
        var url = "file://" + path;
        var unityWebRequestTexture = UnityWebRequestTexture.GetTexture(url);
        yield return unityWebRequestTexture.SendWebRequest();
        print(   "Output" + valuecon);
        var texture = ((DownloadHandlerTexture)unityWebRequestTexture.downloadHandler).texture;
        if (texture == null)
        {
            Debug.LogError("Failed to load texture url:" + url);
        }

        pro.texture = texture;
        //api hit 
    }

    public void OnSend()
    {
        StartCoroutine(UploadAFile(imagePostURL));
    }
    private string CallFunctionByte(RawImage imagepart)
    {

        Texture mainTexture = imagepart.mainTexture;
        Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
        Graphics.Blit(mainTexture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        byte[] bytes = texture2D.EncodeToPNG(); //Can also encode to jpg, just make sure to change the file extensions down below
        string base64encoded = Convert.ToBase64String(bytes);
        Destroy(texture2D);
        return base64encoded;
    }
    static public string Adhar;
    public IEnumerator UploadAFile(string uploadUrl)
    {
        yield return new WaitForEndOfFrame();
        string adhar = CallFunctionByte(profileIC);
        // Create a Web Form, this will be our POST method's data
        var form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("fullname", nameInputFeild.text);
        form.AddField("gender",val);
        form.AddField("profile_pic", adhar.ToString());
        //POST the screenshot to GameSparks
        WWW w = new WWW(uploadUrl, form);
        loadingPanel.SetActive(true);
        yield return w;

        Debug.Log(w.text);
        if (w.error == null)
        {
            Debug.Log(w.error);
            string msg = w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);

            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            Debug.Log(results + "ff");
            if (results == "Profile Updated Successfull" || status == "True")
            {
                loadingPanel.SetActive(false);
                kycPanel.SetActive(false);
                popupPanel.SetActive(true);
            }
        }
        else
        {

        }
    }
    public void SetNoOfPlayers(int index)
    {
        val = index.ToString();
        Debug.Log(val);
        Debug.Log("Transfer Mode" + value);
        switch (index)
        {
            case 0:
                Debug.Log("male");
                break;
            case 1:
                Debug.Log("female");
                break;
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
