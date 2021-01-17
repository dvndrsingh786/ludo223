using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEngine.Networking;


namespace Wizcorp.Web
{

    public class WebView : MonoBehaviour
    {
        public InputField amount;
        public Text errorMsg;

        public GameObject coinsPanel;
        public GameObject walletPanel;
        public GameObject loadingPanel;
        public GameObject WebPanel;
        public string value = "1";
        int point;
        OfflineOnlineApiScript offlineOnlineApiScript;

        string URL = "https://ludocashwin.com/razorpay/pay.php?user_id=";
        //public string URL = "https://onlystore.in/ludomoney/Paytm_Web_Sample_Kit_PHP-master/pgRedirect.php?uid="+"USR101"+"&coins="+amount.text+"&amount"=amount.text";

        //public Text Context;
        #region shared
        public void CallBack(string message)
        {
            errorMsg.text = message;
            Debug.Log("Context "+message);
        }
        #endregion
        long count = 0;

        WWW w;
        string status;
        private string results;
        public string Results
        {
            get
            {
                return results;
            }
        }

#if UNITY_ANDROID
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                WWW w = new WWW("https://ludocashwin.com/razorpay/verify.php");

                StartCoroutine(Store(w));
            }
        }
        public void CallWebView()
        {
            if (!string.IsNullOrEmpty(amount.text))
            {

                URL = URL + GameManager.Uid + "&amount=" + amount.text;
                Debug.Log("Call WebView" + URL);
                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("OpenWebView", URL);
                Debug.Log(URL);
                WWWForm form = new WWWForm();
                form.AddField("user_id", GameManager.Uid);
                form.AddField("amount", amount.text);
                WWW w = new WWW(URL, form);
                StartCoroutine(Store(w));

            }
           
        }
       
        IEnumerator Store(WWW _w)
        {
            yield return _w;
            Debug.Log("https://ludocashwin.com/razorpay/verify.php");


            if (_w.error == null)
            {
                JsonData jsonvale = JsonMapper.ToObject(w.text);
                for (int i = 0; i < jsonvale.Count; i++)
                {
                   print("Json value" +jsonvale[i].ToString());
                   
                }
                string msg = _w.text;
                Debug.Log(msg);
                msg = msg.Replace("{", "");
                msg = msg.Replace("}", "");
                msg = msg.Replace(@"""", string.Empty);
                results = GetDataValue(msg, "message:");
                status = GetDataValue(msg, "status:");
                Debug.Log("Rews"+results);
                Debug.Log("stat"+status);

                /*  if (results == "Your payment was successful")
                  {
                      Debug.Log("Coins added Complete");
                      if (Application.platform == RuntimePlatform.Android)
                      {
                          AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                          AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                          currentActivity.Call("SetupCallBack", this.gameObject.name, "CallBack", "Calling back from Android");
                      }
                      coinsPanel.SetActive(true);
                      amount.text = "";
                      walletPanel.SetActive(false);
                      loadingPanel.SetActive(true);
                      StartCoroutine(CoinsUpdate());
                  }
                  else if (results == "Coins Requested Successfully" )
                  {

                      Debug.Log("error");
                  }
              }
              else
              {
                  Debug.Log("error");
              }*/
            }

        }

        IEnumerator CoinsUpdate()
        {
            Debug.Log("Cha");
            yield return new WaitForSeconds(0.5f);
            coinsPanel.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            loadingPanel.SetActive(false);
            offlineOnlineApiScript.OnlineCash();

        }
        string GetDataValue(string data, string index)
        {
            string value = data.Substring(data.IndexOf(index) + index.Length);
            if (value.Contains(","))
                value = value.Remove(value.IndexOf(","));

            return value;
        }
        void Start()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("SetupCallBack", this.gameObject.name, "CallBack", "Calling back from Android");
                coinsPanel.SetActive(true);
            }
            offlineOnlineApiScript = FindObjectOfType<OfflineOnlineApiScript>();
        }
        #endif

#if UNITY_IOS
	[DllImport("__Internal")]
	private static extern void _nativeLog();
	[DllImport("__Internal")]
	private static extern void _openURL(string url);
	[DllImport("__Internal")]
	private static extern void _setupCallBack(string gameObject, string methodName);

	// Connect with button onClick event
	public void CallWebView()
	{
		_openURL(URL);
	}

	void Start()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_setupCallBack(this.gameObject.name, "CallBack");

			_nativeLog();
		}
	}

#endif
    }
}