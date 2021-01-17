using System;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class SignupScript : MonoBehaviour
{

    [Header("SignUp Object")]

    public InputField _name;
    public InputField _emailId;
    public InputField _password;
    public InputField _confirmPassword;
    public InputField _mobileNumber;
    public InputField _city;
    public InputField _refferalCode;
 
    public Text errorMsg;
    public GameObject shopPanel;
    public GameObject SignupPanel;
    public GameObject loginPanel;

    public string refferalCode;
    public static string code;

    [Header("PlayerGetData")]

    public string playerName;
    public string coins;
    public static string CoinId;
    public Text player;
    public Text coin;
    string playerUrl;
    [Obsolete]
    WWW w;
    string status;
    string url;
    StoreUpdateScript updateScript;
    UserStaticScript staticScript;
    GameConfigrationController configrationController;
    StoreScript storeScript;
    public const string MatchEmailPattern =
      @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
      + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
      + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
   

    void Start()
    {
        staticScript = (UserStaticScript)UnityEngine.Object.FindObjectOfType(typeof(UserStaticScript));
        configrationController = (GameConfigrationController)UnityEngine.Object.FindObjectOfType(typeof(GameConfigrationController));
        updateScript = (StoreUpdateScript)UnityEngine.Object.FindObjectOfType(typeof(StoreUpdateScript));
        storeScript = (StoreScript)UnityEngine.Object.FindObjectOfType(typeof(StoreScript));
    }
    private string results;
    public string Results
    {
        get
        {
            return results;
        }
    }
    public void OnRegisterBtnClick()
    {

        url = "http://onlystore.in/ludomoney/api/register_user.php?fullname=" + _name.text+ "&email="+_emailId.text+ "&mobile=" + _mobileNumber.text + 
              "&password=" + _password.text + "&cpassword=" + _confirmPassword.text + "&pincode=" + _city.text +
              "&ref=" + _refferalCode.text;
            
       
        if (string.IsNullOrEmpty(_name.text))
        {
            errorMsg.text = "Name Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
      
        if (!validateEmail(_emailId.text))
        {
            if (string.IsNullOrEmpty(_emailId.text))
            {
                errorMsg.text = "Email Can't be Blank";
            }
        
            else{ errorMsg.text = "Invalid Email id";
            }
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(_password.text))
        {
            errorMsg.text = "Password Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (_password.text.Length < 6 || _password.text.Length > 12)
        {
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            errorMsg.text = "password must contain minimum 6 character and maximum 12 characters.";
            return;
        }
        if (string.IsNullOrEmpty(_confirmPassword.text))
        {
            errorMsg.text = "Confirm Password Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (_password.text != _confirmPassword.text)
        {
            errorMsg.text = "Password and Confirm Password Must be same";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (_confirmPassword.text.Length < 6 || _confirmPassword.text.Length > 12)
        {
            errorMsg.text = "ConfirmPassword must contain minimum 6 character and maximum 12 characters.";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (!Regex.IsMatch(_password.text, "(?=.*[A-Z])") || !Regex.IsMatch(_password.text, "(?=.*[a-z])") || !Regex.IsMatch(_password.text, "(?=.*[~!@#$%^&*()_-])"))
        {
            errorMsg.text = "password must contain 1 uppercase character,1 lowercase character,1 special character";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (!Regex.IsMatch(_confirmPassword.text, "(?=.*[A-Z])") || !Regex.IsMatch(_confirmPassword.text, "(?=.*[a-z])") || !Regex.IsMatch(_confirmPassword.text, "(?=.*[~!@#$%^&*()_-])"))
        {
            errorMsg.text = "ConfirmPassword must contain 1 uppercase character,1 lowercase character,1 special character";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(_city.text))
        {
            errorMsg.text = "City Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(_mobileNumber.text))
        {
            errorMsg.text = "Mobile Number Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(_refferalCode.text))
        {
            errorMsg.text = "Refferal code Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("fullname", _name.text);
        form.AddField("email", _emailId.text);
        form.AddField("mobile", _mobileNumber.text);
        form.AddField("password", _password.text);
        form.AddField("cpassword", _confirmPassword.text);
        form.AddField("pincode", _city.text);
        form.AddField("ref", _refferalCode.text);
        WWW w = new WWW(url, form);
        StartCoroutine(Signup_time(w));
       
    } 
    IEnumerator Signup_time(WWW _w)
    {
        yield return _w;
       print("responce=" + _w.text);
        if (_w.error == null)
        {
           
            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            refferalCode = GetDataValue(msg, "userid:");
            PlayerPrefs.SetString("token", refferalCode);
            if (results == "User Register Successfully" || status == "True")
            {
                SignupPanel.SetActive(false);
                loginPanel.SetActive(true);
                Debug.Log("Signup Complete");
                PlayerPrefs.SetString("SignUp", "1");
                OnPlayerProfileData();
                updateScript.choose = "Signup";
                staticScript.currentscreen = "SignUp";
                configrationController.currentScreen = "Signup";
                GameFinishWindowController.current = "Signup";
              
            }
            else
            {
               errorMsg.text = results;
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
           
        }
    }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));

        return value;
    }
    public static bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }
    public static bool IsPhoneNumber(string number)
    {
        return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
    }
   
    IEnumerator ErrorClose()
    {
        yield return new WaitForSeconds(2f);
        errorMsg.text = "";
        yield return null;
    }

    public void OnPlayerProfileData()
    {
        StartCoroutine(Data());
    }
    IEnumerator Data()
    {
        yield return new WaitForSeconds(10.8f);
        Dictionary<string, string> headers = new Dictionary<string, string>();
        // Debug.Log(auth);
        WWW www = new WWW("http://onlystore.in/ludomoney/api/user_info.php?userid=" + PlayerPrefs.GetString("token"), null, headers);

        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        for (int i = 0; i < jsonvale["result_push"].Count; i++)
        {
            playerName = jsonvale["result_push"][i]["fullname"].ToString();
            player.text = playerName;
            if (!string.IsNullOrEmpty(coins))
            {
                coins = jsonvale["result_push"][i]["coins"].ToString();
                coin.text = coins;
            }
        }
    }
}
