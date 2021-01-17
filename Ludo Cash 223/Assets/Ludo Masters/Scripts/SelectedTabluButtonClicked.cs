using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;
public class SelectedTabluButtonClicked : MonoBehaviour
{

    public int tableNumber;
    public int fee;

    private string getBettingApi = "https://onlystore.in/ludomoney/api/betting.php";

    private string getBotDifficulty = "https://onlystore.in/ludomoney/api/gamedifficulty.php";

    private string getBotAvailiblity = "https://onlystore.in/ludomoney/api/botswinning.php";
    // Use this for initialization

    void Start()
    {
        Debug.Log("start");
        //   gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        //   gameObject.GetComponent<Button>().onClick.AddListener(startGame);
    }

    public void startGame()
    {

        GameManager.Instance.GameScene = "GameScene";
        GameManager.Instance.requiredPlayers = tableNumber;

        Debug.Log("Fee: " + fee + "  Coins: " + GameManager.Instance.myPlayerData.GetCoins());
        if (GameManager.Instance.myPlayerData.GetCoins() >= fee)
        {

            if (GameManager.Instance.inviteFriendActivated)
            {
                GameManager.Instance.tableNumber = tableNumber;
                GameManager.Instance.payoutCoins = fee;
                GameManager.Instance.initMenuScript.backToMenuFromTableSelect();
                GameManager.Instance.playfabManager.challengeFriend(GameManager.Instance.challengedFriendID, "" + fee + ";" + tableNumber);

            }
            else if (GameManager.Instance.offlineMode)
            {
                GameManager.Instance.payoutCoins = fee;
                if (!GameManager.Instance.gameSceneStarted)
                {
                    SceneManager.LoadScene(GameManager.Instance.GameScene);
                    GameManager.Instance.gameSceneStarted = true;
                }
            }
            else
            {
                GameManager.Instance.tableNumber = tableNumber;
                GameManager.Instance.payoutCoins = fee;
                GameManager.Instance.facebookManager.startRandomGame();
            }

        }
        else
        {
            GameManager.Instance.dialog.SetActive(true);
        }

    }


}
