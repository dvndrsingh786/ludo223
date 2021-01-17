using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PaymentDataScript : MonoBehaviour
{
    public string gameUrl;

    [Header("Payment Data")]

    public string status;
    public string date;
    public string game;
    public string transactionId;
    public string transactionby;
    public string transactionAmount;
    public Transform contentPanel;
    public GameObject paymentData, scrollPanel;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PaymentList());
    }

    IEnumerator PaymentList()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();

        // Debug.Log(auth);
        WWW www = new WWW(gameUrl+ PlayerPrefs.GetString("token"), null, headers);

        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        for (int i = 0; i < jsonvale["result_push"].Count; i++)
        {

            PaymentScript payment = Instantiate(paymentData, contentPanel).GetComponent<PaymentScript>();
            transactionId = jsonvale["result_push"][i]["transaction_id"].ToString();
            payment.transactionId.text = transactionId;
            transactionby = jsonvale["result_push"][i]["transaction_by"].ToString();
            payment.transactionby.text = transactionby;
            transactionAmount = jsonvale["result_push"][i]["transaction_amount"].ToString();
            payment.transactionAmount.text = transactionAmount;
            status = jsonvale["result_push"][i]["status"].ToString();
            payment.status.text = status;
            date = jsonvale["result_push"][i]["date"].ToString();
            payment.date.text = date;
            PlayerPrefs.SetString("Game",game);
            Debug.Log(PlayerPrefs.GetString("Game"));
        }
    }
}
