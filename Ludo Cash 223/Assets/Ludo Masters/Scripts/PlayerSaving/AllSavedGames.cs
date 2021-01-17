using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
public class AllSavedGames : MonoBehaviour
{

    public GameObject[] DataObject;
    public Transform _ContentTransform;
    Dictionary<string, string> std;
    // Use this for initialization
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        string jsonData = PlayerPrefs.GetString(StaticStrings.SaveGameString);
        std = new Dictionary<string, string>();
        //std = PlayFab.SimpleJson.DeserializeObject<Dictionary<string, string>>(jsonData);

        IEnumerator savedKeysList = std.Keys.GetEnumerator();
        while (savedKeysList.MoveNext())
        {
            int index = 0;
            string key = savedKeysList.Current.ToString();
            int numberOfPlayers = int.Parse(key.Substring(key.Length - 1, 1));
            if (numberOfPlayers == 4)
            {
                index = 1;
            }

            GameObject obj = Instantiate(DataObject[index]);
            obj.transform.parent = _ContentTransform;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<DataWindow>().Data(key, this);
            Debug.Log("converData   " + key + "   numberOfPlayers   " + numberOfPlayers);
            _ContentTransform.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 200);
        }

        // Debug.Log("converData   " + std[key]);
        // GameData _Gd_new;
        // _Gd_new = JsonUtility.FromJson<GameData>(std[key]);
        // Debug.Log("Time   " + _Gd_new.Time + "   players  " + _Gd_new.diceNumber + "   Date   " + _Gd_new.Date);
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        DataWindow[] _Child = _ContentTransform.GetComponentsInChildren<DataWindow>();
        foreach (DataWindow _DW in _Child)
        {
            DestroyObject(_DW.gameObject);
        }
        _ContentTransform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
    }

    public void RemoveData(string key, GameObject obj)
    {
        std.Remove(key);
      //  string newData = PlayFab.SimpleJson.SerializeObject(std);
      //  PlayerPrefs.SetString(StaticStrings.SaveGameString, newData);
      //  _ContentTransform.gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, 200);
      //  DestroyObject(obj);
    }
}
