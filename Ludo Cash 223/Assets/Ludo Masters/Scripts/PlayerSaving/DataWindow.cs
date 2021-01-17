
using UnityEngine;
using UnityEngine.UI;
public class DataWindow : MonoBehaviour
{
    public GameObject UpperObj, InfoObj;
    string key;
    public int noOfPlayer;
    AllSavedGames _CallingObj;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Data(string key, AllSavedGames _CallingObj)
    {
        string excludeLastStr = key.Substring(0, (key.Length - 1));
        UpperObj.GetComponent<Text>().text = excludeLastStr;
        InfoObj.GetComponent<Text>().text = "Date :- " + excludeLastStr + " Game Type - " + noOfPlayer + " Players";
        this.key = key;
        this._CallingObj = _CallingObj;
    }

    public void RemoveKey()
    {
        this._CallingObj.RemoveData(this.key, this.gameObject);

    }

    public void PlaySavedGame()
    {
        PlayerPrefs.SetString("PlaySavedGame", this.key);
        GameManager.Instance.requiredPlayers = noOfPlayer;
        if (noOfPlayer == 2)
            GameManager.Instance.type = MyGameType.TwoPlayer;
        else
            GameManager.Instance.type = MyGameType.FourPlayer;

        GameManager.Instance.playfabManager.PlayOfflineWithReal();
    }
}
