using System;
using Kakera;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.IO.Compression;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PickerController : MonoBehaviour
{

    Unimgpicker imagePicker;

    public RawImage imageRenderer;
    public RawImage imageRenderer1;
    public RawImage imageRenderer2;
    public RawImage imageRenderer3;
    int valuecon;

    private int[] sizes = { 1024, 256, 16 };
    RawImage value;

    [Header("String Attribute")]

    public string imagePostURL;
    public GameObject loadingPanel;
    public GameObject popupPanel;
    public GameObject kycPanel;
    string status;

    [Header("Player Profile Attribute")]

    public string playerprofileURL;
    public InputField playername;
    public GameObject playerProfile;
    public GameObject notificationPanel;
    string val = "0";

    bool CAnSetImageAtStart = true;


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
    void setintialize()
    {
        print(valuecon + "value dot");
        if (valuecon == 0)
        {
            value = imageRenderer;

        }
        else if (valuecon == 1)
        {
            value = imageRenderer1;
        }
        else if (valuecon == 2)
        {
            value = imageRenderer2;
        }
        else
        {
            value = imageRenderer3;
        }
        imagePicker.Completed += (string path) =>
        {
            StartCoroutine(LoadImage(path, value));
        };

    }

    public void OnPressShowPicker()
    {
        valuecon = 0;
        setintialize();
        imagePicker.Show("Select Image", "unimgpicker");

    }
    public void OnPressShowPicker1()
    {
        valuecon = 1;
        setintialize();
        imagePicker.Show("Select Image1", "unimgpicker");

    }
    public void OnPressShowPicker2()
    {
        valuecon = 2;
        setintialize();
        imagePicker.Show("Select Image2", "unimgpicker");

    }
    public void OnPressShowPicker3()
    {
        //  imageRenderer3 = pic;
        valuecon = 3;
        setintialize();
        imagePicker.Show("Select Image3", "unimgpicker");

    }
    private IEnumerator LoadImage(string path, RawImage output)
    {
        var url = "file://" + path;
        var unityWebRequestTexture = UnityWebRequestTexture.GetTexture(url);
        yield return unityWebRequestTexture.SendWebRequest();
        print(output.name + "Output" + valuecon);
        var texture = ((DownloadHandlerTexture)unityWebRequestTexture.downloadHandler).texture;
        if (texture == null)
        {
            Debug.LogError("Failed to load texture url:" + url);
        }

        output.texture = texture;
        //api hit 
    }

    public void OnSend()
    {
        StartCoroutine(UploadAFile(imagePostURL));
    }

    private string CallFunctionByte(Texture imagepart)
    {
        Texture mainTexture = imagepart;
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
        string adhar = CallFunctionByte(imageRenderer.mainTexture);
        string adharsecond = CallFunctionByte(imageRenderer2.mainTexture);
        string pancard = CallFunctionByte(imageRenderer1.mainTexture);

        // Create a Web Form, this will be our POST method's data
        var form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("aadhar_first", adhar.ToString());
        form.AddField("aadhar_second", adharsecond.ToString());
        form.AddField("pan_card", pancard.ToString());
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
            if (results == "Uploaded Successfully" || status == "True")
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

    Texture playerTexture;

    public void ChangePlayerTextureFromOutside(Texture playerTex)
    {
        Debug.LogError("Setting texture in PC");
        playerTexture = playerTex;
        imageRenderer3.texture = playerTexture;
        Debug.LogError("Setting texture finished in PC");
        StartCoroutine(PlayerPPublic(playerprofileURL));
    }

    IEnumerator PlayerPPublic(string url)
    {
        Debug.LogError("Uploading pic");
        yield return new WaitForEndOfFrame();
        string profile = CallFunctionByte(playerTexture);
        var form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        //form.AddField("fullname", playername.text);
        //form.AddField("gender", val);
        form.AddField("profile_pic", profile.ToString());
        Debug.Log("BB" + profile.ToString());
        //POST the screenshot to GameSparks
        WWW w = new WWW(url, form);
        //loadingPanel.SetActive(true);
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
                //loadingPanel.SetActive(false);
                //playerProfile.SetActive(false);
                //notificationPanel.SetActive(true);
                Debug.LogError("Uploading pic finsihed");
            }
        }
        else
        {
            Debug.LogError(" Uploading pic Error: " + w.error);
        }
    }

    /// <summary>
    /// Player profile..!!!
    /// </summary>
    public void OnPlayerProfile()
    {
        StartCoroutine(PlayerP(playerprofileURL));
    }

    IEnumerator PlayerP(string url)
    {
        Debug.Log(val);
        yield return new WaitForEndOfFrame();
        string profile = CallFunctionByte(imageRenderer3.mainTexture);
        print(profile + "string profile");
        var form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("fullname", playername.text);
        form.AddField("gender", val);
        form.AddField("profile_pic", profile.ToString());
        Debug.Log("BB" + profile.ToString());
        //POST the screenshot to GameSparks
        WWW w = new WWW(url, form);
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
                UIFlowHandler.uihandler.THETEXTURE = imageRenderer3.mainTexture;
                GameManager.profileImge = UIFlowHandler.uihandler.TextureToSprite((Texture2D)imageRenderer3.mainTexture);
                UIFlowHandler.uihandler.SetPlayerImage();
                loadingPanel.SetActive(false);
                playerProfile.SetActive(false);
                notificationPanel.SetActive(true);
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


 
