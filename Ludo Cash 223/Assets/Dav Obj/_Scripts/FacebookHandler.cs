using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Firebase.Auth;
using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using TMPro;

public class FacebookHandler : MonoBehaviour
{
    public TextMeshProUGUI FB_userName;
    public TextMeshProUGUI FB_email;
    public RawImage FB_userDp;
    [SerializeField] APIManager apimngr;

    // Use this for initialization
    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(setInit);
        }
    }

    void setInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.LogError("fb is logged in");
        }
        else
        {
            Debug.LogError("fb is not logged in");
        }
    }

    public void fbLogin()
    {
        //Enable Loading Screen Here
        List<string> permissions = new List<string>();

        //asks facebook for the users profile
        permissions.Add("public_profile");
        permissions.Add("email");
        permissions.Add("user_friends");
        FB.LogInWithReadPermissions(permissions, authCallBack);
    }

    public void FBLogout()
    {
        FB.LogOut();
        CheckLoginStatus();
    }

    void CheckLoginStatus()
    {
        if (FB.IsLoggedIn)
        {
            Debug.LogError("Can't LogOut");
        }
        else
        {
            Debug.LogError("Logged Out");
        }
    }


    void authCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            if (FB.IsLoggedIn)
            {
                Debug.LogError("Logged in already");
                FB.LogOut();
                fbLogin();
            }
            else
            {
                Debug.LogError("Else");
            }
            //Disable Loading Screen Here
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.LogError("Logged in");
                SetFBCredentials();
                //accessToken(credential);
            }

            else
            {
                Debug.LogError("not logged in");
                //Disable Loading Screen Here
            }
        }
    }

    string facebookuserId;

    public void SetFBCredentials()
    {
        /*
        AccessToken token = AccessToken.CurrentAccessToken;
        Credential credential = FacebookAuthProvider.GetCredential(token.TokenString); // Use these credentials to login on firebase
        */
        facebookuserId = AccessToken.CurrentAccessToken.UserId;
        FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, FbGetPicture);
    }

    Texture2D fbpp;

    void FbGetPicture(IGraphResult result)
    {
        if (result.Texture != null)
        {
            fbpp = result.Texture;
            FB_userDp.texture = fbpp;
            FB.API("/me?fields=id,name,email,picture", HttpMethod.GET, GetFacebookInfo);
        }
    }


    public void GetFacebookInfo(IResult result)
    {
        Debug.LogError(result.RawResult);
        if (result.Error == null)
        {
            Debug.Log(result.ResultDictionary["id"].ToString());
            Debug.Log(result.ResultDictionary["name"].ToString());
            Debug.Log(result.ResultDictionary["email"].ToString());
            FB_userName.text = result.ResultDictionary["name"].ToString();
            FB_email.text = result.ResultDictionary["email"].ToString();
            FB_userDp.texture = fbpp;
            //FindObjectOfType<PickerController>().ChangePlayerTextureFromOutside(result.Texture);
            apimngr.SetPlayerImagee(fbpp);
            apimngr.socialEmail = FB_email.text;
            apimngr.socialName = FB_userName.text;
            apimngr.SocialMediaSignInStart();
            //StartCoroutine(GetSetProfilePic(result.ResultDictionary["name"].ToString(), result.ResultDictionary["email"].ToString()));
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    //For Firebase Login
    /*
    //public void accessToken(Credential firebaseResult)
    //{
    //    FirebaseAuth auth = FirebaseAuth.DefaultInstance;
    //    auth.SignInWithCredentialAsync(firebaseResult).ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCanceled)
    //        {
    //            Debug.LogError("SignInWithCredentialAsync was canceled.");
    //            //Dissable Loading Screen Here
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
    //            //Disable Loading Screen Here
    //            return;
    //        }
    //        FirebaseUser newUser = task.Result;
    //    });
    //}
    */

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

    Uri profilePic;

    IEnumerator GetSetProfilePic(string displyname, string email)
    {

        Texture2D tex;
        tex = fbpp;
        yield return new WaitForEndOfFrame();

        byte[] bArray = tex.EncodeToPNG();
        yield return new WaitForEndOfFrame();

        string code = Convert.ToBase64String(bArray);
        yield return new WaitForEndOfFrame();
        StartCoroutine(SetAvatar(code));
    }

    public IEnumerator SetAvatar(string code)
    {
        //instance.thheCode = code;
        yield return new WaitForEndOfFrame();
        byte[] bArray = Convert.FromBase64String(code);
        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D(800, 600, TextureFormat.RGBA32, false);
        tex.LoadImage(bArray);
        yield return new WaitForEndOfFrame();
        tex.Apply(); //This is profile pic texture
    }
}