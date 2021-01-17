using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCoinsFrameValue : MonoBehaviour
{

    private int currentValue = 0;
    public Text text;
    // Use this for initialization
    void Start()
    {
     
       // InvokeRepeating("CheckAndUpdateValue", 0.2f, 0.2f);
    }

   

    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.Instance.coinsCount.ToString();
    }
}
