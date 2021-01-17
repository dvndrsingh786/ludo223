using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using TMPro;


public class FacebookScriptDav : MonoBehaviour
{
    // public GameObject Panel_Add;
    public TextMeshProUGUI FB_userName;
    public TextMeshProUGUI FB_email;
    public RawImage FB_userDp;

    [SerializeField] APIManager apimngr;

    private void Awake()
    {
        FB.Init(SetInit, onHidenUnity);
       // Panel_Add.SetActive(false);
    }
    void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is Login!");
        }
        else
        {
            Debug.Log("Facebook is not Logged in!");
        }
        DealWithFbMenus(FB.IsLoggedIn);
    }

    void onHidenUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void FBLogin()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        permissions.Add("email");
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }
    // Start is called before the first frame update
    void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
                if (FB.IsLoggedIn)
                {
                    Debug.Log("Facebook is Login!");
                   // Panel_Add.SetActive(true);
                }
            else
            {
                Debug.Log("Facebook is not Logged in!");
            }
            DealWithFbMenus(FB.IsLoggedIn);
        }
    }

    void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API("/me?fields=first_name",HttpMethod.GET,DisplayUsername);
            FB.API("/me?fields=email",HttpMethod.GET,DisplayEmail);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }
        else
        {

        }
    }

    void DisplayEmail(IResult result)
    {
        if (result.Error == null)
        {
            string name = "" + result.ResultDictionary["email"];
            FB_email.text = name;

            Debug.Log("" + name);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            string name = ""+result.ResultDictionary["first_name"];
          FB_userName.text = name;
            
            Debug.Log(""+name);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Debug.Log("Profile Pic");
            //FB_userDp.sprite = Sprite.Create(result.Texture,new Rect(0,0,256,256),new Vector2());
            FB_userDp.texture = result.Texture;
            //FindObjectOfType<PickerController>().ChangePlayerTextureFromOutside(result.Texture);
            apimngr.SetPlayerImagee(result.Texture);
            apimngr.socialEmail = FB_email.text;
            apimngr.socialName = FB_userName.text;
            apimngr.SocialMediaSignInStart();
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    public bool IsFbLoggedIn()
    {
        if (FB.IsLoggedIn)
        {
            return true;
        }
        else return false;
    }

    int count = 0;
    public void SignOutFromFacebookPublic()
    {
        if (FB.IsLoggedIn && count < 10)
        {
            Debug.LogError("FB User not null");
            FB.LogOut();
            count++;
            CancelInvoke();
            Invoke(nameof(SignOutFromFacebookPublic), 0.05f);
        }
        else
        {
            Debug.LogError("FB User null");
            count = 0;
            apimngr.LogOut();
        }
    }

    public void SignOutFromFacebookPublic1()
    {
        if (FB.IsLoggedIn && count < 10)
        {
            Debug.LogError("FB User not null");
            FB.LogOut();
            count++;
            CancelInvoke();
            Invoke(nameof(SignOutFromFacebookPublic1), 0.05f);
        }
        else
        {
            Debug.LogError("FB User null");
            count = 0;
        }
    }

}
