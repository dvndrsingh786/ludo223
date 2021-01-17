using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BetScript : MonoBehaviour
{

    public Text betAmount;
    public Text winningAmount;
    public Text betTime;
    public string id;
    public string betType;
    public string playerNum;
    public int Callingfunction;
    public Image ThisButton;
    public Toggle myToggle;
    //public bool isClockTimer = false;
    public int hr, mns, secs;

    public string startTime, endTime, startDate, endDate;

    public Transform openParent;

    private void Start()
    {
        ThisButton = GetComponent<Image>();
    }

    public void ToggleButtonPower(Toggle theToggle)
    {
        if (theToggle.IsInteractable()) ButtonPOwer();
    }

    public void ButtonPOwer()
    {
        if (ThisButton.color != Color.green)
        {
            ReferenceManager.refMngr.isOnlineBidSelected = true;
            Debug.LogError("Check");
            DisableAllButton();
            ThisButton.color = Color.green;
            if (!myToggle.isOn) myToggle.isOn = true;
            FindObjectOfType<GameConfigrationController>().ChangeBettingAmount(Callingfunction);
            ReferenceManager.refMngr.ShowOnlineInvestment(GameManager.Instance.currentBetAmount, GameManager.Instance.currentWinningAmount);
            Debug.Log("Amount" + Callingfunction);
        }
        else
        {
            ReferenceManager.refMngr.isOnlineBidSelected = false;
            ThisButton.color = Color.white;
            if (myToggle.isOn) myToggle.isOn = false;
            ReferenceManager.refMngr.ShowOnlineInvestment(0, 0);
        }
    }
 
    public void DisableAllButton()
    {
        Debug.LogError("Disabling" + gameObject.name);
        var findingButton = FindObjectsOfType<BetScript>();
        foreach (var item in findingButton)
        {
            item.ThisButton.color = Color.white;
            if(item != this) item.myToggle.isOn = false;
        }
    }

    public void CheckTableStatus()
    {

    }

    //public void UpdateClock()
    //{
    //    betTime.text = hr.ToString() + ":" + mns.ToString() + ":" + secs.ToString();
    //    secs--;
    //    if (secs < 0)
    //    {
    //        mns--;
    //        secs = 59;
    //        if (mns < 0)
    //        {
    //            hr--;
    //            mns = 59;
    //        }
    //    }
    //    if (hr != 0 || mns != 0 || secs != 0)
    //    {
    //        Invoke(nameof(UpdateClock), 1);
    //    }
    //    else
    //    {
    //        SetAsOpenTable();
    //    }
    //}

    //void SetAsOpenTable()
    //{
    //    transform.parent = openParent;
    //    betTime.gameObject.SetActive(false);
    //    GetComponent<Button>().interactable = true;
    //    myToggle.interactable = true;
    //}

}
