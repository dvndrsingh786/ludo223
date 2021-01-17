using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class StartScriptController : MonoBehaviour
{

    public GameObject splashCanvas;
    public GameObject LoginCanvas;
    public FacebookManager fbManager;
    public GameObject menuCanvas;
    public GameObject[] go;

    // Use this for initialization
    void Start()
    {
        //fbManager.GuestLogin();
    }

    public void HideAllElements()
    {
        menuCanvas.SetActive(true);
    }
}
