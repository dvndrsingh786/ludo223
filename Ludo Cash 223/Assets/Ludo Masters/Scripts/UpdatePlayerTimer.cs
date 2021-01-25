using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatePlayerTimer : MonoBehaviour
{
    private float playerTime;
    public GameObject timerObject;
    private Image timer;
    private bool timeSoundsStarted;
    public AudioSource[] audioSources;
    public GameObject GUIController;
    public bool myTimer;
    public bool paused = false;
    public bool check;
    public string myPlayerName;

    public Transform autoMoveParent;
    public int maxAutoMove = 4;
    public TextMeshProUGUI secondsRemaining;
    public GameObject timerObj;

    public PhotonView myview;

    // Use this for initialization
    List<LudoPawnController> activemyPawn = new List<LudoPawnController>();

    // Use this for initialization
    void Start()
    {
        myPlayerName = "";
        maxAutoMove = 4;
        //Debug.LogError("Update Player Timer: " + gameObject.name);
        timer = gameObject.GetComponent<Image>();
        LudoPawnController[] com = FindObjectsOfType<LudoPawnController>();
        foreach (var item in com)
        {
            if (item.isMinePawn)
                activemyPawn.Add(item);
        }
    }

    [PunRPC]
    public void SetTurnCountDisabledFunction(int count)
    {
        //turnCount = count;
        //if (turnCount != 0 || true)
        //{
        //    playerchnaceLeft.text = "Auto Move: " + turnCount.ToString();
        //}
        //check = !check;
        //Debug.LogError("Setting turn count: " + turnCount);
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if (timer == null) timer = gameObject.GetComponent<Image>();
        if (!myview) myview = GetComponent<PhotonView>();
        if (Gamedice != null)
            Gamedice.timer = this;
        timerObj.SetActive(true);
        playerTime = GameManager.Instance.playerTime;
        secondsRemaining.text = "20s";
    }



    public void Pause()
    {
        paused = true;
        audioSources[0].Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!paused)
            if (!IsInvoking(nameof(updateClock)) && gameObject.activeInHierarchy)
            {
                playerTime = GameManager.Instance.playerTime;
                secondsRemaining.text = "20s";
                Invoke(nameof(updateClock), 0.01f);
            }
    }

    public void restartTimer()
    {
        paused = false;
        timer.fillAmount = 1.0f;
        playerTime = GameManager.Instance.playerTime;
        secondsRemaining.text = "20s";
    }


    void OnDisable()
    {
        ismyTurn = false;
        misschance = false;
        if (timer != null)
        {
            timer.fillAmount = 1.0f;
            paused = false;
            audioSources[0].Stop();
            misschance = false;
            playerTime = GameManager.Instance.playerTime;
            secondsRemaining.text = "20s";
            timerObj.SetActive(false);
        }
    }
    public GameDiceController Gamedice;
    public Text playerchnaceLeft;
    public int turnCount = 0;
    public bool misschance = false;
    private void updateClock()
    {
        //Debug.LogError("Updating Clock");
        float minus;
        playerTime--;
        secondsRemaining.text = ((int)playerTime).ToString() + "s";
        //playerTime = GameManager.Instance.playerTime;
        //if (GameManager.Instance.offlineMode)
        //    playerTime = GameManager.Instance.playerTime + GameManager.Instance.cueTime;
        //minus = 1.0f / playerTime * Time.deltaTime;

        //timer.fillAmount -= minus;

        //if (timer.fillAmount < 0.05f)
        if (playerTime < 1)
        {
            //Debug.LogError("Inside");
            audioSources[0].Play();
            // timeSoundsStarted = true;

            if (!misschance)
            {
                misschance = true;
                turnCount++;
                Debug.Log("TurnCount" + turnCount);
                if(turnCount != maxAutoMove)
                autoMoveParent.GetChild(turnCount - 1).GetComponent<Image>().color = Color.red;
                //SynchrozeTurnCount();
                //  FindObjectOfType<GameGUIController>().playerCount.text = "Auto turn Chance" + turnCount;
                playerchnaceLeft.text = "Auto Move: " + turnCount.ToString();
                if (turnCount == maxAutoMove)
                {
                    FindObjectOfType<GameGUIController>().LeaveGame(false);
                }
                else
                {
                    //  Gamedice.RollDice();
                    if (!ismyTurn)
                    {
                        ismyTurn = true;
                    }
                }
                //   Gamedice.RollDice();
            }
        }

        //if (timer.fillAmount <= 0.0f)
        if (playerTime < 1)
        {
            Pause();
            StartCoroutine(autoMove());
        }
        else
        {
            if(gameObject.activeInHierarchy)
            Invoke(nameof(updateClock), 1);
        }

    }
    List<LudoPawnController> InBoard = new List<LudoPawnController>();
    bool ismyTurn = false;

    bool canWalkTurn = true;

    public void SynchrozeTurnCount()
    {
        myview.RPC("SetTurnCount", PhotonTargets.AllBuffered, turnCount);
    }

    public void CanWalkTrueAgain()
    {
        canWalkTurn = true;
    }

    public void PlayerAutoMove()
    {
        print("NameDefine" + Gamedice.steps);
        if (!canWalkTurn) return;
        CancelInvoke();
        if (Gamedice.isMyDice)
        {
            Debug.LogError("MY New If");
            canWalkTurn = false;
            Invoke(nameof(CanWalkTrueAgain), 10);
            InBoard.Clear();
            foreach (LudoPawnController item in activemyPawn)
            {
                if (item.isOnBoard)
                    InBoard.Add(item);
            }
            if (InBoard.Count >= 3)
            {
                //by default opening the 
                int ran = Random.Range(0, InBoard.Count);
                InBoard[ran].MakeMove();
            }
            else
            {
                if (Gamedice.steps == 6)
                {
                    Debug.Log("DiceValue_A" + Gamedice.steps);
                    int ran = Random.Range(0, activemyPawn.Count);
                    activemyPawn[ran].MakeMove();
                    ismyTurn = false;
                    misschance = false;
                }
                else
                {
                    int ran = Random.Range(0, InBoard.Count);
                    if (InBoard.Count > 0)
                    {
                        InBoard[ran].MakeMove();
                        ismyTurn = false;
                        misschance = false;
                    }
                }
            }

        }
        else
        {
            //Debug.LogError("MY New Else");
            GUIController.GetComponent<GameGUIController>().SendingFinishSlowly(this);
        }
    }

    //public void PlayerAutoMove1()
    //{
    //    Invoke(nameof(PlayerAutoMove2), 1);
    //}

    public void PlayerAutoMove1()
    {
        Debug.LogError("Auto Move 1");
        InBoard.Clear();
        foreach (LudoPawnController item in Gamedice.myPawns)
        {
            if (item.isOnBoard)
                InBoard.Add(item);
        }
        if (InBoard.Count >= 1)
        {
            Debug.LogError("Count issss: " + InBoard.Count);
            //by default opening the 
            int ran = Random.Range(0, InBoard.Count);
            InBoard[ran].MakeMoveManually(Gamedice.steps);
        }
        else
        {
            if (Gamedice.steps == 6)
            {
                Debug.Log("DiceValue_A" + Gamedice.steps);
                int ran = Random.Range(0, Gamedice.myPawns.Count);
                Gamedice.myPawns[ran].MakeMoveManually(Gamedice.steps);
                ismyTurn = false;
                misschance = false;
            }
            else
            {
                int ran = Random.Range(0, InBoard.Count);
                if (InBoard.Count > 0)
                {
                    InBoard[ran].MakeMoveManually(Gamedice.steps);
                    ismyTurn = false;
                    misschance = false;
                }
            }
        }
        GUIController.GetComponent<GameGUIController>().TheEnd();
    }

    public void SetTurn(int turns, string idd)
    {
        if (idd == myPlayerName || true)
        {
            turnCount = turns;
            if (turns > 0)
            {
                Debug.LogWarning("Got turns: " + turns);
                playerchnaceLeft.text = "Auto Move: " + turnCount.ToString();
            }
        }
    }

    IEnumerator autoMove()
    {
        StopAllCoroutines();
        Debug.LogError("Auto move");
        TempGameManager.tempGM.view.RPC("SetAliveState", PhotonTargets.All, false);
        Gamedice.RollDice();
        //StartCoroutine(StartFinishTurnSlowly());
        //FindObjectOfType<GameGUIController>().GetCurrentPlayerIndex();
        //if (TempGameManager.tempGM.GetIndex(FindObjectOfType<GameGUIController>().GetCurrentPlayerIndex()))
        //{
        ////    Debug.LogError("take Down");
        //    if (!Gamedice.isMyDice)
        //    {
        //        GUIController.GetComponent<GameGUIController>().SendFinishTurnManually();
        //    }
        //}
        Invoke("PlayerAutoMove", 1f);
        yield return new WaitForSeconds(0.3f);

        yield return new WaitForSeconds(0.6f);
        //Invoke("PlayerAutoMove", 0.15f);
        // turnCount++;
        //Debug.Log("TurnCount" + turnCount);
        //if (turnCount == 2)
        //{
        //    FindObjectOfType<GameGUIController>().LeaveGame(false);
        //}
        audioSources[0].Stop();
        GameManager.Instance.stopTimer = true;
        Debug.LogError("Just before checking offline mode");
        if (!GameManager.Instance.offlineMode)
        {
            Debug.LogError("not offline mode");
            if (myTimer)
            {
                Debug.LogError("My timer true");
                Debug.Log("Timer call finish turn");
                GUIController.GetComponent<GameGUIController>().SendFinishTurn();
            }
            else
            {
                Debug.LogError("My timer false");
                Debug.LogError("Sedning finish turn manually");
                GUIController.GetComponent<GameGUIController>().SendFinishTurnManually();
            }
            //PhotonNetwork.RaiseEvent(9, null, true, null);
        }
        else
        {
            Debug.LogError("Offline mode true");
            Debug.LogError("Offline mode true");
            if (GameManager.Instance.isLocalMultiplayer)
            {
                Debug.LogError("Local multiplayer true");
                GUIController.GetComponent<GameGUIController>().SendFinishTurn();
            }
            else
            {
                Debug.LogError("Local multiplayer false");
                GameManager.Instance.wasFault = true;
                GameManager.Instance.cueController.setTurnOffline(true);
            }
        }
        //showMessage("You " + StaticStrings.runOutOfTime);

        /*if (!GameManager.Instance.offlineMode)
        {
            GameManager.Instance.cueController.setOpponentTurn();
        }*/
    }
}
