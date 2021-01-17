using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class VideoPlayerScript : MonoBehaviour
{
    public RawImage vidoScene;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(playVideo());
        StartCoroutine(VideoPlayer());
    }
    IEnumerator playVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        vidoScene.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
    }
    IEnumerator VideoPlayer()
    {
        yield return new WaitForSeconds(10.5f);
        SceneManager.LoadScene("LoginSplash");
             
    }
}
