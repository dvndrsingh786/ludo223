using System;
using LitJson;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ReedemCoinsScript : MonoBehaviour
{
    [Header("URL String Attribute")]

    public string payTmUrl;
    public string bankDetailUrl;
    public string upiUrl;
    public string withdrawUrl;
    public string UpdateMobileNumberUrl;

    [Header("UI Attribute")]

    public InputField paytmFeild;
    public InputField banknameFeild;
    public InputField ifscFeild;
    public InputField accountFeild;
    public InputField upiField;

    public Text pytmError;
    public Text bnkError;
    public Text upiError;

    [Header ("GameObject Attribute")]

    public GameObject pytmsubmitBtn;
    public GameObject UpiSubmitBtn;
    public GameObject BankSubmitbtn;

    public GameObject pytmwithdrawlBtn;
    public GameObject UpiwithdrawlBtn;
    public GameObject bankwithdrawBtn;

    public GameObject upiMinimumObject;
    public GameObject pytmMinimumObject;
    public GameObject bankMinimumObject;

    public GameObject upiwithdrawPanel;
    public GameObject notificationPanel;
    public GameObject paytmwithdrawPanel;
    public GameObject bankDetailWithdrawPanel;


    [Header("UI Attribute")]

    public GameObject loadingPanel;
    public GameObject successPanel;
    string status;

    [Header("WithDrawal Attribute")]

    public InputField upiWithdraw;
    public InputField pytmamountWithdraw;
    public InputField banknameamountWithdraw;

    [Header("OTP Attribute")]


    public string OtpURL;
    public string setOtpURL;
    public InputField otpVerify;
    public InputField bankVerify;
    public InputField UpiVerify;
    public Text otpText;
    public GameObject pytmOtpPopupPanel;
    public GameObject bankOtpPopupPanel;
    public GameObject UPIOtpPopupPanel;
    public GameObject otpNotificatonPanel;
   
    private string results;
    public string Results
    {
        get
        {
            return results;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Pytm") == "1") {
            pytmsubmitBtn.SetActive(false);
            pytmwithdrawlBtn.SetActive(true);
        }
        if (PlayerPrefs.GetString("UPI") == "1")
        {
            UpiSubmitBtn.SetActive(false);
            UpiwithdrawlBtn.SetActive(true);
        }
        if (PlayerPrefs.GetString("Bank") == "1")
        {
            BankSubmitbtn.SetActive(false);
            bankwithdrawBtn.SetActive(true);
        }
    }

    public void OnPytmBtnClick()
    {
        paytmFeild.interactable = false;
        pytmsubmitBtn.SetActive(false);
        pytmwithdrawlBtn.SetActive(true);
        if (string.IsNullOrEmpty(paytmFeild.text))
        {
            pytmError.text = "Paytm Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("paytm", paytmFeild.text);
        WWWForm form1 = new WWWForm();
        form1.AddField("user_id", GameManager.Uid);
        form1.AddField("mobile", paytmFeild.text);
        WWW w = new WWW(payTmUrl, form);
        WWW w1 = new WWW(UpdateMobileNumberUrl, form1);
        loadingPanel.SetActive(true);
        StartCoroutine(pytmCheck(w1));
       
    }
    public void OnUPIBtnClick()
    {
        if (string.IsNullOrEmpty(upiField.text))
        {
            upiError.text = "UPI Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        upiField.interactable = false;
        UpiSubmitBtn.SetActive(false);
        UpiwithdrawlBtn.SetActive(true);
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("upi", upiField.text);
        WWW w = new WWW(upiUrl, form);
        loadingPanel.SetActive(true);
        StartCoroutine(upiCheck(w));
    }
    public void OnBankDetailBtnClick()
    {
        banknameFeild.interactable = false;
        accountFeild.interactable = false;
        ifscFeild.interactable = false;
        BankSubmitbtn.SetActive(false);
        bankwithdrawBtn.SetActive(true);

        if (string.IsNullOrEmpty(banknameFeild.text))
        {
            bnkError.text = "Bank Name Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(accountFeild.text))
        {
            bnkError.text = "Bank Acount Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(ifscFeild.text))
        {
            bnkError.text = "IFSC Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("bank_name", banknameFeild.text);
        form.AddField("ifsc_code", ifscFeild.text);
        form.AddField("account", accountFeild.text);
        WWW w = new WWW(bankDetailUrl, form);
        loadingPanel.SetActive(true);
        StartCoroutine(bankDetailCheck(w));
    }
    IEnumerator pytmCheck(WWW w)
    {
        yield return w;
        
        print("responce=" + w.text);
        if (w.error == null)
        {
            string msg = w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            Debug.Log(results);
            Debug.Log(status);
            if (results == "Updated Successfully" || status == "True")
            {
                loadingPanel.SetActive(false);
                pytmsubmitBtn.SetActive(false);
                pytmwithdrawlBtn.SetActive(true);
                successPanel.SetActive(true);
                PlayerPrefs.SetString("Pytm","1");
            }
            else
            {
                upiError.text = results;
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
        }
    }
    IEnumerator upiCheck(WWW w)
    {
        yield return w;
        
        print("responce=" + w.text);
        if (w.error == null)
        {
            string msg = w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            Debug.Log(results);
            Debug.Log(status);
            if (results == "Updated Successfully" || status == "True")
            {
                loadingPanel.SetActive(false);
                UpiSubmitBtn.SetActive(false);
                UpiwithdrawlBtn.SetActive(true);
                successPanel.SetActive(true);
                PlayerPrefs.SetString("UPI", "1");
            }
            else
            {
                upiError.text = results;
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
        }
    }
    IEnumerator bankDetailCheck(WWW w)
    {
        yield return w;
        

        print("responce=" + w.text);
        if (w.error == null)
        {
            string msg = w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            Debug.Log(results);
            Debug.Log(status);
            if (results == "Updated Successfully" || status == "True")
            {
                loadingPanel.SetActive(false);
                BankSubmitbtn.SetActive(false);
                bankwithdrawBtn.SetActive(true);
                successPanel.SetActive(true);
                PlayerPrefs.SetString("Bank", "1");
            }
            else
            {
                bnkError.text = results;
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
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
  
    public void OnpytnEdit()
    {
        paytmFeild.interactable = true;
        pytmsubmitBtn.SetActive(true);
        pytmwithdrawlBtn.SetActive(false);
    }
    public void OnUpiEdit()
    {
        upiField.interactable = true;
        UpiSubmitBtn.SetActive(true);
        UpiwithdrawlBtn.SetActive(false);
    }
    public void OnbankEdit(int check)
    {
        switch (check)
        {

            case 0:
                banknameFeild.interactable = true;
                break;
            case 1:
                ifscFeild.interactable = true;
                break;
            case 2:
                accountFeild.interactable = true;
                break;
        }
        BankSubmitbtn.SetActive(true);
        bankwithdrawBtn.SetActive(false);
    }

     public void OnPytmAmountValueChanged()
     {
        Debug.Log(pytmamountWithdraw.text + string.IsNullOrWhiteSpace(pytmamountWithdraw.text));
        if (!string.IsNullOrEmpty(pytmamountWithdraw.text))
        {
            int amount = int.Parse(pytmamountWithdraw.text.ToString());
            Debug.Log(amount);
            Debug.Log(GameManager.Instance.coinsCount);

            if (GameManager.Instance.winAmount <= 0)
            {
                pytmwithdrawlBtn.GetComponent<Button>().interactable = false;
                //MinimumObject.SetActive(true);
                //MinimumText.text = "#Can only redeem earned coins";
                // ServiceObject.SetActive(false);
            }
            else if (amount < GameManager.Instance.playfabManager.minimumWithdrawlAmount)
            {
                pytmwithdrawlBtn.GetComponent<Button>().interactable = false;
              
                GameManager.Instance.playfabManager.apiManager.MinimumObject.SetActive(true);
                GameManager.Instance.playfabManager.apiManager.MinimumText.text = "#Minimum Withdrawl Amount is " + GameManager.Instance.playfabManager.minimumWithdrawlAmount;
                // ServiceObject.SetActive(false);
            }
            else if (amount > GameManager.Instance.winAmount && amount >= GameManager.Instance.playfabManager.minimumWithdrawlAmount)
            {
                pytmwithdrawlBtn.GetComponent<Button>().interactable = false;
    
                pytmMinimumObject.SetActive(true);
                pytmError.text = "#Amount should be less or equal to earned coins";
                // ServiceObject.SetActive(false);
            }
            else
            {
               pytmwithdrawlBtn.GetComponent<Button>().interactable =true;
               pytmMinimumObject.SetActive(false);
               pytmError.text = "";
                /* serviceTax = ((int)(amount * 0.05f));
                 amountToWithdraw = (amount - (int)(amount * 0.05f));
                 serviceChargeTxt.text = "5%";
                 redeemAmountTxt.text = amountToWithdraw.ToString();*/
            }
        }
        else
        {
            pytmwithdrawlBtn.GetComponent<Button>().interactable = true;
            pytmMinimumObject.SetActive(false);
            // ServiceObject.SetActive(false);
        }
     }

    public void OnUPiAmountValueChanged()
    {
        Debug.Log(upiWithdraw.text + string.IsNullOrWhiteSpace(upiWithdraw.text));
        if (!string.IsNullOrEmpty(pytmamountWithdraw.text))
        {
            int amount = int.Parse(upiWithdraw.text.ToString());
            Debug.Log(amount);
            Debug.Log(GameManager.Instance.coinsCount);

            if (GameManager.Instance.winAmount <= 0)
            {
                UpiwithdrawlBtn.GetComponent<Button>().interactable = false;
                upiMinimumObject.SetActive(true);
                upiError.text = "#Can only redeem earned coins";
                // ServiceObject.SetActive(false);
            }
            else if (amount < GameManager.Instance.playfabManager.minimumWithdrawlAmount)
            {
                UpiwithdrawlBtn.GetComponent<Button>().interactable = false;

                upiMinimumObject.SetActive(true);
                upiError.text = "#Minimum Withdrawl Amount is " + GameManager.Instance.playfabManager.minimumWithdrawlAmount;
                // ServiceObject.SetActive(false);
            }
            else if (amount > GameManager.Instance.winAmount && amount >= GameManager.Instance.playfabManager.minimumWithdrawlAmount)
            {
                UpiwithdrawlBtn.GetComponent<Button>().interactable = false;
                upiMinimumObject.SetActive(true);
                upiError.text = "#Amount should be less or equal to earned coins";
                // ServiceObject.SetActive(false);
            }
            else
            {
                UpiwithdrawlBtn.GetComponent<Button>().interactable = true;
                upiMinimumObject.SetActive(false);
                 upiError.text = "";
                /* serviceTax = ((int)(amount * 0.05f));
                 amountToWithdraw = (amount - (int)(amount * 0.05f));
                 serviceChargeTxt.text = "5%";
                 redeemAmountTxt.text = amountToWithdraw.ToString();*/
            }
        }
        else
        {
            UpiwithdrawlBtn.GetComponent<Button>().interactable = true;
            upiMinimumObject.SetActive(false);
            // ServiceObject.SetActive(false);
        }
    }

    public void OnBankAmountValueChanged()
    {
        Debug.Log(banknameamountWithdraw.text + string.IsNullOrWhiteSpace(banknameamountWithdraw.text));
        if (!string.IsNullOrEmpty(banknameamountWithdraw.text))
        {
            int amount = int.Parse(banknameamountWithdraw.text.ToString());
            Debug.Log(amount);
            Debug.Log(GameManager.Instance.coinsCount);

            if (GameManager.Instance.winAmount <= 0)
            {
                bankwithdrawBtn.GetComponent<Button>().interactable = false;
                bankMinimumObject.SetActive(true);
                bnkError.text = "#Can only redeem earned coins";
                // ServiceObject.SetActive(false);
            }
            else if (amount < GameManager.Instance.playfabManager.minimumWithdrawlAmount)
            {
                bankwithdrawBtn.GetComponent<Button>().interactable = false;

                bankMinimumObject.SetActive(true);
                bnkError.text = "#Minimum Withdrawl Amount is " + GameManager.Instance.playfabManager.minimumWithdrawlAmount;
                // ServiceObject.SetActive(false);
            }
            else if (amount > GameManager.Instance.winAmount && amount >= GameManager.Instance.playfabManager.minimumWithdrawlAmount)
            {
                bankwithdrawBtn.GetComponent<Button>().interactable = false;

                bankMinimumObject.SetActive(true);
               bnkError.text = "#Amount should be less or equal to earned coins";
                // ServiceObject.SetActive(false);
            }
            else
            {
                bankwithdrawBtn.GetComponent<Button>().interactable = true;
                bankMinimumObject.SetActive(false);
                bnkError.text = "";
                /* serviceTax = ((int)(amount * 0.05f));
                 amountToWithdraw = (amount - (int)(amount * 0.05f));
                 serviceChargeTxt.text = "5%";
                 redeemAmountTxt.text = amountToWithdraw.ToString();*/
            }
        }
        else
        {
            bankwithdrawBtn.GetComponent<Button>().interactable = true;
            bankMinimumObject.SetActive(false);
            // ServiceObject.SetActive(false);
        }
    }

    public void OnpytmWithDrawBtnClick()
    {
        print(GameManager.otp.ToString() +" difference"+ otpVerify.text);
        if (GameManager.otp.ToString() == otpVerify.text)
        {
            WWWForm form = new WWWForm();
            form.AddField("user_id", GameManager.Uid);
            form.AddField("amount", pytmamountWithdraw.text);
            WWW w = new WWW(withdrawUrl, form);
            StartCoroutine(pytmWithdraw(w));
        }
        else if (GameManager.otp.ToString() != otpVerify.text)
        {
            otpText.text = "Invalid Otp";
            otpNotificatonPanel.SetActive(true);
        }
    }

    public void OnUpiWithDrawBtnClick()
    {
        print(GameManager.otp.ToString() + " difference" + UpiVerify.text);
        if (GameManager.otp.ToString() == UpiVerify.text)
        {

            WWWForm form = new WWWForm();
            form.AddField("user_id", GameManager.Uid);
            form.AddField("amount", upiWithdraw.text);
            WWW w = new WWW(withdrawUrl, form);
            StartCoroutine(UpiWithdraw(w));
        }
        else if (GameManager.otp.ToString() != UpiVerify.text)
        {
            otpText.text = "Invalid Otp";
            otpNotificatonPanel.SetActive(true);
        }
    }

    public void OnBankWithDrawBtnClick()
    {
        print(GameManager.otp.ToString() + " difference" + bankVerify.text);
        if (GameManager.otp.ToString() == bankVerify.text)
        {

            WWWForm form = new WWWForm();
            form.AddField("user_id", GameManager.Uid);
            form.AddField("amount", banknameamountWithdraw.text);
            WWW w = new WWW(withdrawUrl, form);
            StartCoroutine(bankWithdraw(w));
        }
        else if (GameManager.otp.ToString() != bankVerify.text)
        {
            otpText.text = "Invalid Otp";
            otpNotificatonPanel.SetActive(true);
        }
    }

    IEnumerator pytmWithdraw(WWW _w)
    {
        yield return _w;
        print("responce=" + _w.text);
        if (_w.error == null)
        {
            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            Debug.Log(results);
            Debug.Log(status);
            if (results == "Transaction Successfully" || status == "True")
            {
                paytmwithdrawPanel.SetActive(false);
                notificationPanel.SetActive(true);
                // loginPanel.SetActive(false);
                pytmamountWithdraw.text = "";
            }
            else
            {
                pytmError.text = results;
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
        }
    }

    IEnumerator UpiWithdraw(WWW _w)
    {
        yield return _w;
        print("responce=" + _w.text);
        if (_w.error == null)
        {
            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            Debug.Log(results);
            Debug.Log(status);
            if (results == "Transaction Successfully" || status == "True")
            {
                upiwithdrawPanel.SetActive(false);
                notificationPanel.SetActive(true);
                // loginPanel.SetActive(false);
                pytmamountWithdraw.text = "";
            }
            else
            {
                upiError.text = results;
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
        }
    }

    IEnumerator bankWithdraw(WWW _w)
    {
        yield return _w;
        print("responce=" + _w.text);
        if (_w.error == null)
        {
            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            Debug.Log(results);
            Debug.Log(status);
            if (results == "Transaction Successfully" || status == "True")
            {
                bankDetailWithdrawPanel.SetActive(false);
                notificationPanel.SetActive(true);
                // loginPanel.SetActive(false);
                pytmamountWithdraw.text = "";
            }
            else
            {
                bnkError.text = results;
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
        }
    }

    /// <summary>
    /// Otp Send Area
    /// </summary>
    /// <returns></returns>

    public void OnOTPSendPaytm()
    {
        if (!GameManager.Instance.userID.Contains("Guest"))
        {


            if (string.IsNullOrEmpty(pytmamountWithdraw.text))
            {
                pytmError.text = "Amount Can't be Blank";
                print("withdraw value");
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
                return;
            }
            else
            {
                int amount = int.Parse(pytmamountWithdraw.text.ToString());
                Debug.Log(amount);
                Debug.Log(GameManager.Instance.coinsCount);
                if (amount > GameManager.Instance.coinsCount)
                {
                    pytmError.text = "Amount Can't be greater than Coins";
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                    return;
                }
            }
           
        }
        
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        WWW w = new WWW(OtpURL, form);
        pytmOtpPopupPanel.SetActive(true);
        paytmwithdrawPanel.SetActive(false);
        StartCoroutine(OtpSend(w));

     
    }

    public void OnOTPSendPBank()
    {
        if (!GameManager.Instance.userID.Contains("Guest"))
        {


            if (string.IsNullOrEmpty(banknameamountWithdraw.text))
            {
                bnkError.text = "Bank Withdraw Can't be Blank";
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
                return;
            }
            else
            {
                int amount = int.Parse(banknameamountWithdraw.text.ToString());
                Debug.Log(amount);
                Debug.Log(GameManager.Instance.coinsCount);
                if (amount > GameManager.Instance.coinsCount)
                {
                    bnkError.text = "Bank Withdraw Can't be greater than Coins";
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                    return;
                }
            }
        }

        bankOtpPopupPanel.SetActive(true);
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        WWW w = new WWW(OtpURL, form);
        bankOtpPopupPanel.SetActive(true);
        bankDetailWithdrawPanel.SetActive(false);
        StartCoroutine(OtpSend(w));

    }
    public void OnOTPUPIBank()
    {
        if (!GameManager.Instance.userID.Contains("Guest"))
        {


            if (string.IsNullOrEmpty(upiWithdraw.text))
            {
                upiError.text = "UPI Can't be Blank";
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
                return;
            }
            else
            {
                int amount = int.Parse(upiWithdraw.text.ToString());
                Debug.Log(amount);
                Debug.Log(GameManager.Instance.coinsCount);
                if (amount > GameManager.Instance.coinsCount)
                {
                    upiError.text = "UPI Can't be greater than Coins";
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                    return;
                }
            }

        }
        
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        WWW w = new WWW(OtpURL, form);

        UPIOtpPopupPanel.SetActive(true);
        upiwithdrawPanel.SetActive(false);
        StartCoroutine(OtpSend(w));
    }

    IEnumerator OtpSend(WWW w)
    {
        yield return w;
        print("responce=" + w.text);
        loadingPanel.SetActive(true);
        if (w.error == null)
        {
            string msg = w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            GameManager.otp = GetDataValue(msg, "otp:");
            Debug.Log(GameManager.otp);
            
        }
        Debug.Log(results);
        Debug.Log(status);
        if (status == "true")
        {
            loadingPanel.SetActive(false);
            otpText.text = results;
            otpNotificatonPanel.SetActive(true);
            StartCoroutine(Notify());
        }
    }
    IEnumerator Notify()
    {
        yield return new WaitForSeconds(0.7f);
        otpNotificatonPanel.SetActive(false);
    }
    IEnumerator ErrorClose()
    {
        yield return new WaitForSeconds(2f);
        pytmError.text = "";
        bnkError.text = "";
        upiError.text = "";
    }
}
