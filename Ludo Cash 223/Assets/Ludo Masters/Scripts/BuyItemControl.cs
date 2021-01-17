using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyItemControl : MonoBehaviour {

    public int index = 1;
   
    public Text coinsTxt, amountTxt;


    public void buyItem() {
       // GameManager.Instance.IAPControl.OnPurchaseClicked(index);
    }
}
