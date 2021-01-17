using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SavingWindow : MonoBehaviour
{

    public GameObject DisplayTimeText;
    // Use this for initialization

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        string Day = System.DateTime.Now.Date.DayOfWeek.ToString();
        Day = Day.Substring(0, 3);

        string Date = System.DateTime.Now.Date.ToLongDateString();
        Date = Date.Remove(0, Date.IndexOf(","));

        string Time = System.DateTime.Now.ToShortTimeString();
        DisplayTimeText.GetComponent<Text>().text = Day + Date + " " + Time;
    }
}
