using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DailyReward : MonoBehaviour
{

    public GameObject[] RewardObjs;

    public GameObject ScalingAnim;

    public GameObject TargetCanvas;
    public GameObject RewardButton;

    public Sprite[] objSprite;

    int[] coins = new int[7] { 50, 500, 600, 700, 800, 900, 10000 };

    // Use this for initialization
    void Start()
    {

    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        int rewardItem = PlayerPrefs.GetInt("ClaimRewards", 0);

        int index = 0;
        foreach (GameObject obj in RewardObjs)
        {
            if (index == rewardItem)
            {
                obj.GetComponent<Image>().sprite = objSprite[1];
            }
            obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Day " + (index + 1);
            obj.transform.GetChild(1).gameObject.GetComponent<Text>().text = coins[index].ToString();
            if (index < rewardItem)
            {
                obj.transform.GetChild(3).gameObject.GetComponent<Text>().enabled = true;
            }
            index++;
        }
    }

    public void ClaimRewardClicked()
    {
        Debug.Log("Close Reward Clicked");
        int rewardItem = PlayerPrefs.GetInt("ClaimRewards", 0);
        PlayerPrefs.SetInt("Rewards_" + rewardItem, System.DateTime.UtcNow.DayOfYear);
        RewardObjs[rewardItem].transform.GetChild(3).gameObject.GetComponent<Text>().enabled = true;
        rewardItem++;
        PlayerPrefs.SetInt("ClaimRewards", rewardItem);
        RewardButton.GetComponent<Button>().interactable = false;
        Invoke("CloseScene", 1.0f);

    }

    void CloseScene()
    {
        ScalingAnim.GetComponent<ScalerAnimationHandler>().DeScale(TargetCanvas);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
