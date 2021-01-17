using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SplashScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Splash()); 
    }

    IEnumerator Splash()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(1);
    }
}
