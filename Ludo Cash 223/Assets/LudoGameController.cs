using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;
using UnityEngine.UI;

public class LudoGameController : PunBehaviour, IMiniGame
{

    public GameObject[] dice;

    public GameObject[] LocalMultiDice;
    public GameObject GameGui;
    public GameGUIController gUIController;
    public GameObject[] Pawns1;
    public GameObject[] Pawns2;
    public GameObject[] Pawns3;
    public GameObject[] Pawns4;

    public GameObject gameBoard;
    public GameObject gameBoardScaler;

    [HideInInspector]
    public int steps = 5;

    public bool nextShotPossible;
    private int SixStepsCount = 0;
    public int finishedPawns = 0;
    public Text playerName;

    public GameObject BluePrefab, RedPrefab, YellowPrefab, GreenPrefab;

    public Image boardImage;
    private int botCounter = 0;

    private List<GameObject> botPawns;

    public void HighlightPawnsToMove(int player, int steps)
    {

        botPawns = new List<GameObject>();

        gUIController.restartTimer();


        GameObject[] pawns = GameManager.Instance.currentPlayer.pawns;

        this.steps = steps;

        if (steps == 6)
        {
            nextShotPossible = true;
            SixStepsCount++;
            if (SixStepsCount == 3)
            {
                nextShotPossible = false;
                if (GameGui != null)
                {
                    //gUIController.SendFinishTurn();
                    if (!GameManager.Instance.isLocalMultiplayer)
                        Invoke("sendFinishTurnWithDelay", 1.0f);

                }

                return;
            }
        }
        else
        {
            SixStepsCount = 0;
            nextShotPossible = false;
        }

        bool movePossible = false;

        int possiblePawns = 0;
        GameObject lastPawn = null;
        for (int i = 0; i < pawns.Length; i++)
        {
            bool possible = pawns[i].GetComponent<LudoPawnController>().CheckIfCanMove(steps);
            if (possible)
            {
                lastPawn = pawns[i];
                movePossible = true;
                possiblePawns++;
                botPawns.Add(pawns[i]);
            }
        }



        if (possiblePawns == 1)
        {
            if (GameManager.Instance.currentPlayer.isBot)
            {
                if (GameManager.Instance.isLocalMultiplayer)
                {
                    if (GameManager.Instance.isPlayingWithComputer)
                        StartCoroutine(movePawn(lastPawn, false));
                    else
                        lastPawn.GetComponent<LudoPawnController>().MakeMove();
                }

                else
                    StartCoroutine(movePawn(lastPawn, false));
            }
            else
            {
                lastPawn.GetComponent<LudoPawnController>().MakeMove();
            }

        }
        else
        {
            if (possiblePawns == 2 && lastPawn.GetComponent<LudoPawnController>().pawnInJoint != null)
            {
                if (GameManager.Instance.currentPlayer.isBot && (GameManager.Instance.isPlayingWithComputer))
                {
                    if (!lastPawn.GetComponent<LudoPawnController>().mainInJoint)
                    {
                        StartCoroutine(movePawn(lastPawn, false));
                        Debug.Log("AAA");
                    }
                    else
                    {
                        StartCoroutine(movePawn(lastPawn.GetComponent<LudoPawnController>().pawnInJoint, false));
                        Debug.Log("BBB");
                    }

                }
                else
                {
                    if (!lastPawn.GetComponent<LudoPawnController>().mainInJoint)
                    {
                        lastPawn.GetComponent<LudoPawnController>().MakeMove();
                    }
                    else
                    {
                        lastPawn.GetComponent<LudoPawnController>().pawnInJoint.GetComponent<LudoPawnController>().MakeMove();
                    }
                    //lastPawn.GetComponent<LudoPawnController>().MakeMove();
                }
            }
            else
            {
                if (possiblePawns > 0 && GameManager.Instance.currentPlayer.isBot && (GameManager.Instance.isPlayingWithComputer))
                {
                    int bestScoreIndex = 0;
                    int bestScore = int.MinValue;
                    // Make bot move
                    for (int i = 0; i < botPawns.Count; i++)
                    {
                        int score = botPawns[i].GetComponent<LudoPawnController>().GetMoveScore(steps);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestScoreIndex = i;
                        }
                    }
                    Debug.Log("Best Score: " + bestScore);

                    StartCoroutine(movePawn(botPawns[bestScoreIndex], true));
                }
                else
                {

                }
            }
        }

        if (!movePossible)
        {
            if (GameGui != null)
            {
                Debug.Log("game controller call finish turn");
                gUIController.PauseTimers();
                if (!GameManager.Instance.isLocalMultiplayer)
                    Invoke("sendFinishTurnWithDelay", 1.0f);
                else if (GameManager.Instance.isPlayingWithComputer)
                {
                    Invoke("sendFinishTurnWithDelay", 0.3f);
                }
                else
                    Invoke("sendFinishTurnWithDelay", 0.05f);
            }
        }
    }
  
    public void sendFinishTurnWithDelay()
    {
        Debug.Log("teststtstss");
        gUIController.SendFinishTurn();
    }

    public void Unhighlight()
    {
        for (int i = 0; i < Pawns1.Length; i++)
        {
            Pawns1[i].GetComponent<LudoPawnController>().Highlight(false);
            // Pawns1[i].GetComponent<LudoPawnController>().MoveBySteps(0);
        }

        for (int i = 0; i < Pawns2.Length; i++)
        {
            Pawns2[i].GetComponent<LudoPawnController>().Highlight(false);
            // Pawns2[i].GetComponent<LudoPawnController>().MoveBySteps(0);
        }

        for (int i = 0; i < Pawns3.Length; i++)
        {
            Pawns3[i].GetComponent<LudoPawnController>().Highlight(false);
            //Pawns3[i].GetComponent<LudoPawnController>().MoveBySteps(0);
        }

        for (int i = 0; i < Pawns4.Length; i++)
        {
            Pawns4[i].GetComponent<LudoPawnController>().Highlight(false);
            //Pawns4[i].GetComponent<LudoPawnController>().MoveBySteps(0);
        }
    }

    void IMiniGame.BotTurn(bool first)
    {
        if (first)
        {
            SixStepsCount = 0;
        }
        Debug.Log("bogCount  " + botCounter + "   delay   " + GameManager.Instance.botDelays.Count);
        // Invoke("RollDiceWithDelay", GameManager.Instance.botDelays[(botCounter + 1) % GameManager.Instance.botDelays.Count]);
        Invoke("RollDiceWithDelay", 0.3f);
        Debug.Log("botcountr   " + botCounter);

        //throw new System.NotImplementedException();
    }


    public IEnumerator movePawn(GameObject pawn, bool delay)
    {
        if (delay)
        {
            yield return new WaitForSeconds(0.1f);
            // botCounter++;
        }
        pawn.GetComponent<LudoPawnController>().MakeMovePC();
    }

    public void RollDiceWithDelay()
    {
        Debug.Log("after    botcountr   " + botCounter +"moves count "+GameManager.Instance.botDiceValues.Count);
        GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().RollDiceBot(GameManager.Instance.botDiceValues[(botCounter % 100)]);
        botCounter++;
    }


    void IMiniGame.CheckShot()
    {
        throw new System.NotImplementedException();
    }

    void IMiniGame.setMyTurn()
    {
        SixStepsCount = 0;
        GameManager.Instance.diceShot = false;
        if (!GameManager.Instance.isLocalMultiplayer)
            dice[0].GetComponent<GameDiceController>().EnableShot();
        else
            LocalMultiDice[0].GetComponent<GameDiceController>().EnableShot();


    }

    void IMiniGame.setOpponentTurn()
    {
        SixStepsCount = 0;
        GameManager.Instance.diceShot = false;
        if (!GameManager.Instance.isLocalMultiplayer)
            dice[0].GetComponent<GameDiceController>().DisableShot();
        else
            LocalMultiDice[0].GetComponent<GameDiceController>().DisableShot();
        Unhighlight();
    }



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GameManager.Instance.miniGame = this;
        PhotonNetwork.OnEventCall += this.OnEvent;
    }

    // Use this for initialization
    void Start()
    {


        // Scale gameboard
       
        
        float scalerWidth = gameBoardScaler.GetComponent<RectTransform>().rect.size.x;
        float boardWidth = gameBoard.GetComponent<RectTransform>().rect.size.x;

        // gameBoard.GetComponent<RectTransform>().localScale = new Vector2(scalerWidth / boardWidth, scalerWidth / boardWidth);


        Debug.Log("  gUIController   ");
        gUIController = GameGui.GetComponent<GameGUIController>();


    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        Debug.Log("Received event Ludo: " + eventcode);

        if (eventcode == (int)EnumGame.DiceRoll)
        {

            gUIController.PauseTimers();
            string[] data = ((string)content).Split(';');
            steps = int.Parse(data[0]);
            int pl = int.Parse(data[1]);

            GameManager.Instance.playerObjects[pl].dice.GetComponent<GameDiceController>().RollDiceStart(steps);
        }
        else if (eventcode == (int)EnumGame.PawnMove)
        {
            string[] data = ((string)content).Split(';');
            int index = int.Parse(data[0]);
            int pl = int.Parse(data[1]);
            steps = int.Parse(data[2]);
            Debug.Log("data   " + data + "  pl  " + pl + "   index  " + index);
            GameManager.Instance.playerObjects[pl].pawns[index].GetComponent<LudoPawnController>().MakeMovePC();
        }
        else if (eventcode == (int)EnumGame.PawnRemove)
        {
            string data = (string)content;
            string[] messages = data.Split(';');
            int index = int.Parse(messages[1]);
            int playerIndex = int.Parse(messages[0]);

            GameManager.Instance.playerObjects[playerIndex].pawns[index].GetComponent<LudoPawnController>().GoToInitPosition(false);
        }

    }
}
