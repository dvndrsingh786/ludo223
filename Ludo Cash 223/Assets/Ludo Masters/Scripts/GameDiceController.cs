using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
 using Photon;
public class GameDiceController : PunBehaviour
{

    public Sprite[] diceValueSprites; 
    public GameObject arrowObject;
    public GameObject diceValueObject;
    public GameObject diceAnim;
    public  int tempvalue=0;
    // Use this for initialization
    public bool isMyDice = false;
    public GameObject LudoController;
    public LudoGameController controller;
    public int player = 1;
    private Button button;

    public GameObject notInteractable;

    public GameObject highlightBoard;
    


    public int steps = 0;

    List<int> PriorityCheck;
    List<int> UpPriority = new List<int> { 0, 1, 2, 3, 4, 5, 11, 12, 13, 19, 20, 21, 22, 23, 24 };
    List<int> DownPriority = new List<int> { 26, 27, 28, 29, 30, 31, 37, 38, 39, 45, 46, 47, 48, 49, 50 };
    int diceIndex = 0;
    int nextSixAppearnce = 0;
    string Manualanimation =
       "RollDiceAnimation";

    public List<LudoPawnController> myPawns;

    public UpdatePlayerTimer timer;

    void Start()
    {
        nextSixAppearnce = Random.Range(3, 8);
        if (isMyDice)
        {
            SetSprite();
        }
    }

    private void SetSprite()
    {
        tempvalue = PlayerPrefs.GetInt("Diceselect");
        Debug.Log("DiceValue"+tempvalue);
        if (tempvalue == 0)
        {

            for (int i = 0; i < diceValueSprites.Length; i++)
            {
                diceValueSprites[i] = DiceSelectionScript.instance.Dice1[i];
                diceValueObject.GetComponent<Image>().sprite = DiceSelectionScript.instance.D1;
                Manualanimation = "RollDiceAnimation";
            }
        }
        else if (tempvalue == 1)
        {
            for (int i = 0; i < diceValueSprites.Length; i++)
            {
                diceValueSprites[i] = DiceSelectionScript.instance.Dice2[i];
                diceValueObject.GetComponent<Image>().sprite = DiceSelectionScript.instance.D2;
                Manualanimation = "D2";
            }

        }
        else if (tempvalue == 2)
        {
            for (int i = 0; i < diceValueSprites.Length; i++)
            {
                diceValueSprites[i] = DiceSelectionScript.instance.Dice3[i];
                diceValueObject.GetComponent<Image>().sprite = DiceSelectionScript.instance.D3;
                Manualanimation = "D3";
            }

        }
        else if (tempvalue == 3)
        {
            for (int i = 0; i < diceValueSprites.Length; i++)
            {
                diceValueSprites[i] = DiceSelectionScript.instance.Dice4[i];
                diceValueObject.GetComponent<Image>().sprite = DiceSelectionScript.instance.D4;
                Manualanimation = "D4";
            }

        }
        else if (tempvalue == 4)
        {
            for (int i = 0; i < diceValueSprites.Length; i++)
            {
                diceValueSprites[i] = DiceSelectionScript.instance.Dice5[i];
                diceValueObject.GetComponent<Image>().sprite = DiceSelectionScript.instance.D5;
                Manualanimation = "D5";
            }

        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        PriorityCheck = new List<int> { 0, 1, 2, 3, 4, 5, 19, 20, 21, 22, 23, 24, 26, 27, 28, 29, 30, 31, 45, 46, 47, 48, 49, 50 };
        button = GetComponent<Button>();
        controller = LudoController.GetComponent<LudoGameController>();

        button.interactable = false;

        if (GameManager.Instance.isLocalMultiplayer)
        {
            if (!GameManager.Instance.isPlayingWithComputer)
                isMyDice = true;
        }

    }

    public void SetDiceValue()
    {
        Debug.Log("Set dice value called");
       diceValueObject.GetComponent<Image>().sprite = diceValueSprites[steps - 1];
        diceValueObject.SetActive(true);
        diceAnim.SetActive(false);

        controller.gUIController.restartTimer();

        if (isMyDice)
            controller.HighlightPawnsToMove(player, steps);
        else if (GameManager.Instance.currentPlayer.isBot)
        {
            controller.HighlightPawnsToMove(player, steps);
        }

    }

    public void EnableOpponentLocal()
    {
        if (button == null)
        {
            Debug.Log("button Null  " + gameObject.name);
            button = GetComponent<Button>();
        }
        button.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableShot()
    {
        if (GameManager.Instance.currentPlayer.isBot)
        {
            if (!GameManager.Instance.isLocalMultiplayer)
            {
                Debug.Log("Bot Turn  ");
                GameManager.Instance.miniGame.BotTurn(false);
            }

            else
            {
                if (GameManager.Instance.isPlayingWithComputer)
                {
                    GameManager.Instance.miniGame.BotTurn(false);
                }
                else
                {
                    Debug.Log("Player Offffffff");
                    button.interactable = true;
                    arrowObject.SetActive(true);
                    highlightBoard.SetActive(true);
                }


            }

        }
        else
        {
            if (PlayerPrefs.GetInt(StaticStrings.SoundsKey, 0) == 0)
            {
                Debug.Log("Vibrate");
#if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
            
#endif
            }
            else
            {
              
                Debug.Log("Vibrations OFF");
            }

            Debug.Log("Player ONNNN");
            if (controller != null)
                controller.gUIController.myTurnSource.Play();
            notInteractable.SetActive(false);
            if (button != null)
                button.interactable = true;
            arrowObject.SetActive(true);
            highlightBoard.SetActive(true);
        }
    }

    public void DisableShot()
    {
        Debug.Log("DisableShot");
        notInteractable.SetActive(true);
        if (button != null)
            button.interactable = false;
        arrowObject.SetActive(false);
        highlightBoard.SetActive(false);
    }

    public void EnableDiceShadow()
    {
        Debug.Log("EnableDiceShadow");
        arrowObject.SetActive(false);
        highlightBoard.SetActive(false);
        notInteractable.SetActive(true);

        if (GameManager.Instance.isPlayingWithComputer)
        {
            if (GameManager.Instance.requiredPlayers == 2)
            {
                button.gameObject.SetActive(false);
                notInteractable.gameObject.SetActive(false);
            }
        }

        AdjustPriority();
    }

    public bool isDiceRolled
    {
        get
        {
            return !button.interactable;
        }
    }

    public void DisableDiceShadow()
    {
        Debug.Log("DisableDiceShadow");
        arrowObject.SetActive(true);
        highlightBoard.SetActive(true);
        notInteractable.SetActive(false);

        if (GameManager.Instance.isPlayingWithComputer)
        {
            if (GameManager.Instance.requiredPlayers == 2)
            {
                button.gameObject.SetActive(true);
                button.interactable = true;
                notInteractable.gameObject.SetActive(false);
            }
        }


    }


    void AdjustPriority()
    {
        List<LudoPawnController> sortingUpPawn = new List<LudoPawnController>();
        List<LudoPawnController> sortingDownPawn = new List<LudoPawnController>();
        foreach (PlayerObject obj in controller.gUIController.playerObjects)
        {
            GameObject[] pawns = obj.pawns;
            foreach (GameObject objs in pawns)
            {
                if (objs.GetComponent<LudoPawnController>().CurrentPosition != -1)
                {
                    int SiblingIndex = objs.GetComponent<LudoPawnController>().SiblingIndex;

                    if (SiblingIndex != -1 && UpPriority.Any(x => x == SiblingIndex))
                    {
                        sortingUpPawn.Add(objs.GetComponent<LudoPawnController>());
                    }
                    if (SiblingIndex != -1 && DownPriority.Any(x => x == SiblingIndex))
                    {
                        sortingDownPawn.Add(objs.GetComponent<LudoPawnController>());
                    }
                }

                // obj.GetComponent<LudoPawnController>().MoveBySteps(0);
            }
        }

        sortingUpPawn = sortingUpPawn.OrderBy(x => x.SiblingIndex).ToList();
        sortingDownPawn = sortingDownPawn.OrderBy(x => x.SiblingIndex).ToList();
        foreach (LudoPawnController obj in sortingUpPawn)
        {
            obj.transform.SetAsLastSibling();
            Debug.Log("sortingUpPawn pawn  " + obj.CurrentPosition);
        }
        foreach (LudoPawnController obj in sortingDownPawn)
        {
            obj.transform.SetAsFirstSibling();
            Debug.Log(" sortingDownPawn  pawn  " + obj.CurrentPosition);
        }
    }

    int aa = 0;
    int bb = 0;

    public void RollDice()
    {
        if (isMyDice)
        {
            if (PhotonNetwork.inRoom)
            {
                TempGameManager.tempGM.view.RPC("SetAliveState", PhotonTargets.AllBuffered, true);
                TempGameManager.tempGM.view.RPC("SetCurrentPlayerIndex", PhotonTargets.AllBuffered, FindObjectOfType<GameGUIController>().GetCurrentPlayerIndex());
                //timer.SynchrozeTurnCount();
            }
            steps = Random.Range(1, 7);
            if (steps == 6)
            {
                ResetPredictOutcome();
            }
            else if (diceIndex == nextSixAppearnce)
            {
                ResetPredictOutcome();
            }

            controller.nextShotPossible = false;
            controller.gUIController.PauseTimers();
            button.interactable = false;
            Debug.Log("Roll Dice");
            arrowObject.SetActive(false);
            if (steps == 6)
            {
                aa++;
                if (aa == 2)
                {
                    steps = Random.Range(1, 6);
                    aa = 0;
                }
            }
            else
            {
                aa = 0;
            }
            Debug.Log("aaa   " + aa);

            if (steps == 6)
            {
                Debug.Log("Popup Call");
            }
            // if (aa % 2 == 0) steps = 6;
            // else steps = 2;
            // aa++;
            // steps = Random.Range(1, 7);

            RollDiceStart(steps);
            string data = steps + ";" + controller.gUIController.GetCurrentPlayerIndex();
            PhotonNetwork.RaiseEvent((int)EnumGame.DiceRoll, data, true, null);
            diceIndex++;
            Debug.Log("Value: " + steps);
        }
    }

    public void RollDiceMAnually()
    {
        //Debug.LogError("Rolling dice manually");
        steps = Random.Range(1, 6);
        if (steps == 6)
        {
            ResetPredictOutcome();
        }

        //controller.nextShotPossible = false;
        controller.gUIController.PauseTimers();
        //button.interactable = false;
        Debug.Log("Roll Dice");
        arrowObject.SetActive(false);
        if (steps == 6)
        {
            aa++;
            if (aa == 2)
            {
                steps = Random.Range(1, 6);
                aa = 0;
            }
        }
        else
        {
            aa = 0;
        }

        //RollDiceStart(steps);
        GameManager.Instance.playerObjects[controller.gUIController.GetCurrentPlayerIndex()].dice.GetComponent<GameDiceController>().RollDiceStart(steps);
        string data = steps + ";" + controller.gUIController.GetCurrentPlayerIndex();
        PhotonNetwork.RaiseEvent((int)EnumGame.DiceRoll, data, true, null);
    }

    void ResetPredictOutcome()
    {
        nextSixAppearnce = Random.Range(4, 8);
        steps = 6;
        diceIndex = 0;
    }

    public void RollDiceBot(int value)
    {

        controller.nextShotPossible = false;
        controller.gUIController.PauseTimers();

        Debug.Log("Roll Dice bot    " + value);

        // if (bb % 2 == 0) steps = 6;
        // else steps = 2;
        // bb++;
        // if (value == 0)
        //     steps = Random.Range(1, 7);
        // else
        steps = value;
        RollDiceStart(steps);


    }

    public void RollDiceStart(int steps)
    {
        GetComponent<AudioSource>().Play();
        this.steps = steps;
        diceValueObject.SetActive(false);
        diceAnim.SetActive(true);
        diceAnim.GetComponent<Animator>().Play(Manualanimation);
        Invoke("AnimationOVer", 0.3f);
    }
  private void AnimationOVer()
    {
        CancelInvoke();
        SetDiceValue();
    }

    public void LoadPrevDice(int playerIndex, int steps)
    {
        Debug.Log("setssss   " + steps);
        this.steps = steps;
        button.interactable = false;

        arrowObject.SetActive(false);
        diceValueObject.GetComponent<Image>().sprite = diceValueSprites[steps - 1];
        diceValueObject.SetActive(true);
        controller.HighlightPawnsToMove(playerIndex, this.steps);
    }
}
