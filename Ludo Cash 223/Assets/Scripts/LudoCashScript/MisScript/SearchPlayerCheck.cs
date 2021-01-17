using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SearchPlayerCheck : MonoBehaviour
{
    PlayWithFriendListScript cdata;
    Button btn;
    public Text playername;
    public string uid;
    public string pname;
    public static string pUid;
    public string ref_uid;
    public static string reference_Uid;
    // Start is called before the first frame update
    void Start()
    {
        cdata = FindObjectOfType<PlayWithFriendListScript>();
       
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnCLick);
    }
    void OnCLick()
    {
        Debug.Log("Hello");
        reference_Uid = ref_uid;
        cdata.OnFriendSelected(btn, reference_Uid);
       // gameSc.OnPlay();
    }
}
