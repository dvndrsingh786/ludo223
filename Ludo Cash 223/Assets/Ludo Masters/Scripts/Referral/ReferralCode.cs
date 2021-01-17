using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferralCode : MonoBehaviour
{

    public static string isUserSeenThisDisplay = "isUserSeenThisDisplay";
    public static string referralCodeString = "RefferalCode";

    public static string isRefferedBySomeOne = "referredBySomeOne";

    public GameObject ReferralScreen;
    public InputField Code;

    public GameObject AlertObj;
    static ReferralCode _Instamce;

    private void Awake()
    {
        if (_Instamce == null)
        {
            _Instamce = this;
            if (PlayerPrefs.GetString(isUserSeenThisDisplay, "false").Equals("false"))
            {
                ReferralScreen.SetActive(true);
            }
            return;
        }
        Destroy(this.gameObject);
    }

    public static ReferralCode Instance
    {
        get
        {
            return _Instamce;
        }

    }

    public void EnterCode()
    {
        if (Code.text.Length > 0)
        {
            PlayerPrefs.SetString(referralCodeString, Code.text);
            ReferralScreen.SetActive(false);
        }
        else
        {
            AlertObj.SetActive(true);
            Invoke("DisableAlert", 2);
        }

    }

    void DisableAlert()
    {
        AlertObj.SetActive(false);
    }

    public void NotInterested()
    {
        PlayerPrefs.SetString(isUserSeenThisDisplay, "true");
        ReferralScreen.SetActive(false);

    }
}
