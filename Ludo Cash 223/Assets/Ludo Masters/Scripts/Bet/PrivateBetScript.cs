using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrivateBetScript : MonoBehaviour
{

    public Text betAmount;
    public Text winningAmount;
    public string id;
    public string playerNum;
    public Image ThisButton;
    public Toggle myToggle;

    private void Start()
    {
        ThisButton = GetComponent<Image>();
    }
    public int Callingfunction;

    public void TogglePrivateBet(Toggle theToggle)
    {
        if (theToggle.IsInteractable()) OnPrivateBet();
    }

    public void OnPrivateBet()
    {
        if (ThisButton.color != Color.green)
        {
            ReferenceManager.refMngr.isBidSelected = true;
            DisableAllButton();
            if (!myToggle.isOn) myToggle.isOn = true;
            ThisButton.color = Color.green;
            FindObjectOfType<GameConfigrationController>().ChangeBettingAmount(Callingfunction);
            ReferenceManager.refMngr.ShowOfflineInvestment(GameManager.Instance.currentBetAmount, GameManager.Instance.currentWinningAmount);
        }
        else
        {
            ReferenceManager.refMngr.isBidSelected = false;
            ThisButton.color = Color.white;
            if (myToggle.isOn) myToggle.isOn = false;
            ReferenceManager.refMngr.ShowOfflineInvestment(0, 0);
        }
    }
    public void DisableAllButton()
    {
        var findingButton = FindObjectsOfType<PrivateBetScript>();
        foreach (var item in findingButton)
        {
            item.ThisButton.color = Color.white;
            if (item != this) item.myToggle.isOn = false;
        }
    }
}
