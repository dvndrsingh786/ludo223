using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDiceSelectionScript : MonoBehaviour
{

    public GameObject[] dicePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnDiceSelection(int num) {
        dicePanel[num].SetActive(false);
        PlayerPrefs.SetInt("Diceselect", num);
    }
}
