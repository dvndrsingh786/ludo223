/*
 * Copyright (C) 2012 GREE, Inc.
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System.Collections;
using UnityEngine;
#if UNITY_2018_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.UI;


public class SampleWebView : MonoBehaviour
{
    //   public string Url;
    // public Text status;
    WebViewObject webViewObject;
    private void Start()
    {

      //  StartCoroutine(OpenView());
    }

    public void OnEnable()
    {
        StartCoroutine(OpenView());
    }

    IEnumerator OpenView()
    {
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init(
            cb: (msg) =>
            {
                // Debug.Log(string.Format("CallFromJS[{0}]", msg));
                // status.text = "callback"+msg;
                //   status.GetComponent<Animation>().Play();
            },
            err: (msg) =>
            {
                // Debug.Log(string.Format("CallOnError[{0}]", msg));
                // status.text ="err"+ msg;
                //   status.GetComponent<Animation>().Play();
            },
            started: (msg) =>
            {
                // status.text = "started" + msg;
                //Debug.Log(string.Format("CallOnStarted[{0}]", msg));
            },
            hooked: (msg) =>
            {
                // status.text ="hooked"+ msg;
                // Debug.Log(string.Format("CallOnHooked[{0}]", msg));
            },
            ld: (msg) =>
            {
                // status.text = "loaded" + msg;
                GameManager.Instance.playfabManager.closeUrl = msg;
                Debug.Log("Checkmsg" + msg);
                if (GameManager.Instance.playfabManager.closeUrl == "https://api1.ludocashwin.com/cashfree/success_payment.php")
                {
                    GameManager.Instance.playfabManager.apiManager.CloseMoneyView();
                }

                if (GameManager.Instance.playfabManager.closeUrl == "https://api1.ludocashwin.com/cashfree/payment_failed.php")
                {
                    GameManager.Instance.playfabManager.apiManager.CloseMoneyView();
                }

                // Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
#if UNITY_EDITOR_OSX || !UNITY_ANDROID
                // NOTE: depending on the situation, you might prefer
                // the 'iframe' approach.
                // cf. https://github.com/gree/unity-webview/issues/189
#if true
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        window.location = 'unity:' + msg;
                      }
                    }
                  }
                ");
#else
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        var iframe = document.createElement('IFRAME');
                        iframe.setAttribute('src', 'unity:' + msg);
                        document.documentElement.appendChild(iframe);
                        iframe.parentNode.removeChild(iframe);
                        iframe = null;
                      }
                    }
                  }
                ");
#endif
#endif
                webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
            },
            //ua: "custom user agent string",
            enableWKWebView: true);
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        webViewObject.bitmapRefreshCycle = 1;
#endif
        // cf. https://github.com/gree/unity-webview/pull/512
        // Added alertDialogEnabled flag to enable/disable alert/confirm/prompt dialogs. by KojiNakamaru · Pull Request #512 · gree/unity-webview
        //webViewObject.SetAlertDialogEnabled(false);

        // cf. https://github.com/gree/unity-webview/pull/550
        // introduced SetURLPattern(..., hookPattern). by KojiNakamaru · Pull Request #550 · gree/unity-webview
        //webViewObject.SetURLPattern("", "^https://.*youtube.com", "^https://.*google.com");

        // cf. https://github.com/gree/unity-webview/pull/570
        // Add BASIC authentication feature (Android and iOS with WKWebView only) by takeh1k0 · Pull Request #570 · gree/unity-webview
        //webViewObject.SetBasicAuthInfo("id", "password");

        webViewObject.SetMargins(0,0,0,0);
        webViewObject.SetVisibility(true);

#if !UNITY_WEBPLAYER && !UNITY_WEBGL
        // if (Url.StartsWith("http")) {
        webViewObject.LoadURL(GameManager.Instance.playfabManager.coinsBuyUrl.Replace(" ", "%20"));
        // }

#else
       
            webViewObject.LoadURL(GameManager.Instance.playfabManager.coinsBuyUrl.Replace(" ", "%20"));
        
        webViewObject.EvaluateJS(
            "parent.$(function() {" +
            "   window.Unity = {" +
            "       call:function(msg) {" +
            "           parent.unityWebView.sendMessage('WebViewObject', msg)" +
            "       }" +
            "   };" +
            "});");
#endif
        yield break;
    }

    /*void OnGUI()
    {
        GUI.enabled = webViewObject.CanGoBack();
        if (GUI.Button(new Rect(10, 10, 80, 80), "<")) {
            webViewObject.GoBack();
        }
        GUI.enabled = true;

        GUI.enabled = webViewObject.CanGoForward();
        if (GUI.Button(new Rect(100, 10, 80, 80), ">")) {
            webViewObject.GoForward();
        }
        GUI.enabled = true;

        GUI.TextField(new Rect(200, 10, 300, 80), "" + webViewObject.Progress());

        if (GUI.Button(new Rect(600, 10, 80, 80), "*")) {
            var g = GameObject.Find("WebViewObject");
            if (g != null) {
                Destroy(g);
            } else {
                StartCoroutine(Start());
            }
        }
        GUI.enabled = true;

        if (GUI.Button(new Rect(700, 10, 80, 80), "c")) {
            Debug.Log(webViewObject.GetCookies(Url));
        }
        GUI.enabled = true;
}
*/
    public void Close()
    {


    }
}
