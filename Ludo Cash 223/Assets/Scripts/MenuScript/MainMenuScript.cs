using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject signPanel;
    public GameObject loginPanel;
   public GameObject choosePanel;
 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Login"+ PlayerPrefs.GetString("Login"));
       
    }

    public void OnLogoutBtn()
    {
       // PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoginSplash");
    }
    public void OnExitBtn()
    {
        Application.Quit();
    }
}
