using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using UnityEngine.Networking;
using TMPro;

public class GoogleSignInDemo : MonoBehaviour
{
    public Text infoText;
    public string webClientId = "<your client id here>";

    public FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;

    public static GoogleSignInDemo instance;

    [SerializeField] APIManager apimngr;

    [SerializeField] RawImage profilePicture;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI email;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
    }

    private void Start()
    {
        if (instance == null) instance = this;
        else
        {
            if(instance!=this)
            {
                Destroy(gameObject);
                return;
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public bool IsCurrentUserNull()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Debug.LogError("Check user: USer null");
            return true;
        }
        else
        {
            Debug.LogError("Check user:  user not null");
            return false;
        }
    }


    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            Debug.LogError("Welcome: " + task.Result.DisplayName + "!");
            Debug.LogError("Email = " + task.Result.Email);
            Debug.LogError("Google ID Token = " + task.Result.IdToken);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    Uri profilePicUri;

    System.Collections.IEnumerator GetProfilePic()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(profilePicUri))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Texture2D tex = new Texture2D(2, 2);
                tex = DownloadHandlerTexture.GetContent(uwr);
                profilePicture.texture = tex;
                //FindObjectOfType<PickerController>().ChangePlayerTextureFromOutside(tex);
                apimngr.SetPlayerImagee(tex);
            }
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                apimngr.socialEmail = task.Result.Email;
                apimngr.socialName = task.Result.DisplayName;
                Debug.LogError("Email: " +  apimngr.socialEmail);
                Debug.LogError("Display name: " + apimngr.socialName);
                Debug.LogError("Phone number: " + task.Result.PhoneNumber);
                email.text = task.Result.Email;
                playerName.text = task.Result.DisplayName;
                profilePicUri = task.Result.PhotoUrl;
                StartCoroutine(GetProfilePic());
                apimngr.SocialMediaSignInStart();
            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void AddToInformation(string str) { /*infoText.text += "\n" + str; */}
    int count = 0;
    public void SignOutFromGooglePublic()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null && count < 10) 
        {
            Debug.LogError("Google User not null");
            GoogleSignIn.DefaultInstance.SignOut();
            FirebaseAuth.DefaultInstance.SignOut();
            count++;
            CancelInvoke();
            Invoke(nameof(SignOutFromGooglePublic), 0.05f);
        }
        else
        {
            Debug.LogError("Google User null");
            count = 0;
            apimngr.LogOut();
        }
    }

    public void SignOutFromGooglePublic1()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null && count < 10)
        {
            Debug.LogError("Google User not null");
            GoogleSignIn.DefaultInstance.SignOut();
            FirebaseAuth.DefaultInstance.SignOut();
            count++;
            CancelInvoke();
            Invoke(nameof(SignOutFromGooglePublic1), 0.05f);
        }
        else
        {
            Debug.LogError("Google User null");
            count = 0;
        }
    }
}