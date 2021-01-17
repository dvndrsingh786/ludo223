using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;



public class StoreScript : MonoBehaviour
{
    public List<GameObject> coinsAmount;
    public GameObject coinsRequestPanel;

   
    private void OnEnable()
    {
        for (int i = 0; i < coinsAmount.Count; i++)
        {
            if (i < GameManager.Instance.shopCoins.Count)
            {
                coinsAmount[i].SetActive(true);
                coinsAmount[i].GetComponent<BuyItemControl>().coinsTxt.text = GameManager.Instance.shopCoins[i].ToString();
                coinsAmount[i].GetComponent<BuyItemControl>().amountTxt.text = GameManager.Instance.shopCost[i].ToString();
            }
            else
            {
                coinsAmount[i].SetActive(false);
            }
        }
    }
    
    public void OnRequestCoinsClick(int index)
    {
        if (!GameManager.Instance.userID.Contains("Guest"))
        {
            
            
            string url = "https://onlystore.in/api/request_coins.php?my_referral_code=" + GameManager.Instance.userID +
                  "&request_amount=" + GameManager.Instance.shopCost[index]+"&game_name=6";
            
            WWW w = new WWW(url);
            StartCoroutine(RequestCoins(w));
        }
         
        
    }
    IEnumerator RequestCoins(WWW _w)
    {
        yield return _w;
        print("response=" + _w.text);
        if (_w.error == null)
        {

            yield return _w;
      
            Debug.Log(_w.text);
            JsonData jsonvale = JsonMapper.ToObject(_w.text);
            string results = jsonvale["result_push"][0]["message"].ToString();
            string status = jsonvale["result_push"][0]["status"].ToString();
            if (results == "Coins Requested Successfully" || status == "True")
            {
                coinsRequestPanel.SetActive(true);
                GameManager.Instance.initMenuScript.storePanel.SetActive(false);
            }
           
        }
       
    }
}
