using System;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
//using Facebook.Unity;
using AssemblyCSharp;
//using Firebase.Extensions;
using System.Collections;
using PlayFab.ClientModels;
using System.Globalization;
using Random = System.Random;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon.Chat;
using System.Text.RegularExpressions;

public class PlayFabManager : Photon.PunBehaviour, IChatClientListener {
    
    private Sprite[] avatarSprites;
    public int minimumWithdrawlAmount;
    public string PlayFabId;
    public string authToken;
    public bool multiGame = true;
    public bool roomOwner = false;
    private FacebookManager fbManager;
    public GameObject fbButton;
    private FacebookFriendsMenu facebookFriendsMenu;
    public ChatClient chatClient;
    private bool alreadyGotFriends = false;
    public GameObject menuCanvas;

    public GameObject MatchPlayersCanvas;
    public GameObject splashCanvas;
    public GameObject Loading;
    public bool opponentReady = false;
    public bool imReady = false;
    public GameObject playerAvatar;
    public GameObject playerName;
    public GameObject backButtonMatchPlayers;

    public APIManager apiManager;
    public PaymentHistroyScript histroyScript;
    public bool isInLobby = false;
    public bool isInMaster = false;
    public StaticGameVariablesController staticGameVariables;

    public string coinsBuyUrl, closeUrl;

    public PickerController pickerController;
  /*Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;
    private string topic = "TestTopic";*/
    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        GameManager.Instance.type = MyGameType.TwoPlayer;
        PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
        PhotonNetwork.PhotonServerSettings.PreferredRegion = CloudRegionCode.asia;
        // PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.BestRegion;
        // PhotonNetwork.PhotonServerSettings.AppID = StaticStrings.PhotonAppID;
#if UNITY_IOS
        PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Tcp;
#else
        PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Udp;
#endif
        Debug.Log("PORT: " + PhotonNetwork.PhotonServerSettings.ServerPort);

        PlayFabSettings.TitleId = StaticStrings.PlayFabTitleID;

        PhotonNetwork.OnEventCall += this.OnEvent;

        DontDestroyOnLoad(transform.gameObject);
    }

    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    public void destroy()
    {
        if (this.gameObject != null)
            DestroyImmediate(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("Playfab start");
        PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong; ;
        GameManager.Instance.playfabManager = this;
        fbManager = GameObject.Find("FacebookManager").GetComponent<FacebookManager>();
        facebookFriendsMenu = GameManager.Instance.facebookFriendsMenu;

        avatarSprites = staticGameVariables.avatars;
        pickerController = FindObjectOfType<PickerController>();
       /* Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });*/
        
    }
    private void OnEnable()
    {
        //InitializeFirebase();
    }


    // Setup message event handlers.
   /* void InitializeFirebase()
    {

        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.SubscribeAsync(topic).ContinueWithOnMainThread(task => {
            // LogTaskCompletion(task, "SubscribeAsync");
        });
        Debug.Log("Firebase Messaging Initialized");

        // This will display the prompt to request permission to receive
        // notifications if the prompt has not already been displayed before. (If
        // the user already responded to the prompt, thier decision is cached by
        // the OS and can be changed in the OS settings).

        isFirebaseInitialized = true;
    }
    Firebase.Messaging.TokenReceivedEventArgs token;
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
       Debug.Log("Received a new message from: " + e.Message.From);
    }*/
    void Update()
    {
        if (chatClient != null) { chatClient.Service(); }
    }

    // handle events:
    private void OnEvent(byte eventcode, object content, int senderid)
    {

        Debug.Log("Received event: " + (int)eventcode);

        if (eventcode == (int)EnumPhoton.BeginPrivateGame)
        {
            //StartGame();
            LoadGameScene();
        }
        else if (eventcode == (int)EnumPhoton.StartWithBots && senderid != PhotonNetwork.player.ID)
        {
            LoadBots();
        }
        else if (eventcode == (int)EnumPhoton.StartGame)
        {
            LoadGameScene();
        }
    }

    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if (GameManager.Instance.controlAvatars != null && GameManager.Instance.type == MyGameType.Private)
        {
            PhotonNetwork.LeaveRoom();
            GameManager.Instance.controlAvatars.ShowJoinFailed("Room closed");
        }
        else
        {
            if (newMasterClient.NickName == PhotonNetwork.player.NickName)
            {
                Debug.Log("Im new master client");
                WaitForNewPlayer();
            }
        }

    }

    public void StartGame()
    {
        // while (!opponentReady || !imReady /*|| (!GameManager.Instance.roomOwner && !GameManager.Instance.receivedInitPositions)*/)
        // {
        //     yield return 0;
        // }
        PhotonNetwork.room.IsOpen = false;
        Debug.Log("calling start function");
        CancelInvoke("StartGameWithBots");
        Invoke("startGameScene", 3.0f);

        //startGameScene();
    }

    private IEnumerator waitAndStartGame()
    {
        // while (!opponentReady || !imReady /*|| (!GameManager.Instance.roomOwner && !GameManager.Instance.receivedInitPositions)*/)
        // {
        //     yield return 0;
        // }
        while (GameManager.Instance.readyPlayers < GameManager.Instance.requiredPlayers - 1 || !imReady /*|| (!GameManager.Instance.roomOwner && !GameManager.Instance.receivedInitPositions)*/ )
        {
            yield return 0;
        }
        startGameScene();
        GameManager.Instance.readyPlayers = 0;
        opponentReady = false;
        imReady = false;
    }

    public void startGameScene()
    {
        if (GameManager.Instance.currentPlayersCount >= GameManager.Instance.requiredPlayers || GameManager.Instance.type == MyGameType.Private)
        {
            Debug.Log("bata de bhai kyu rula rha h...!!");
            LoadGameScene();

            if (GameManager.Instance.type == MyGameType.Private)
            {
                PhotonNetwork.RaiseEvent((int)EnumPhoton.BeginPrivateGame, null, true, null);
            }
            else
            {
                PhotonNetwork.RaiseEvent((int)EnumPhoton.StartGame, null, true, null);
            }

        }
        else
        {
            if (PhotonNetwork.isMasterClient)
                WaitForNewPlayer();
        }
    }

    public void LoadGameScene()
    {
        //ReferenceManager.refMngr.SetBets(GameManager.Instance.currentBetAmount,GameManager.Instance.currentWinningAmount);
        //for (int i = 0; i < GameManager.Instance.opponentsNames.Count; i++)
        //{
        //    Debug.LogError("Loading Game Scene OP Names: " + GameManager.Instance.opponentsNames[i]);
        //}

        //for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
        //{
        //    Debug.LogError("Loading Game Scene OP ID: " + GameManager.Instance.opponentsIDs[i]);
        //}
        apiManager.tablevalue = ReferenceManager.refMngr.GetTableValue(GameManager.Instance.currentBetAmount, GameManager.Instance.currentWinningAmount);
        GameManager.Instance.GameScene = "GameScene";
        Debug.Log("proinghhvk"+ GameManager.Instance.gameSceneStarted);
        if (!GameManager.Instance.gameSceneStarted)
        {
            SceneManager.LoadScene(GameManager.Instance.GameScene);
            GameManager.Instance.gameSceneStarted = true;
            if (GameManager.Instance.type == MyGameType.Private)
            {
                ReferenceManager.refMngr.ChangeWinningAmountManually(GameManager.Instance.currentBetAmount, GameManager.Instance.currentPlayersCount);
            }
            if (GameManager.Instance.isLocalMultiplayer == false)
            {
                ReferenceManager.refMngr.ChangeWinningAmountManually(GameManager.Instance.currentBetAmount, GameManager.Instance.currentPlayersCount);
                //   GameManager.Instance.coinsCount -= GameManager.Instance.currentBetAmount;
                apiManager.DeductCoins(GameManager.Instance.currentBetAmount);
            }
            if (GameManager.Instance.type == MyGameType.Private) {
                if (GameManager.Instance.isLocalMultiplayer == false)
                {
                    //apiManager.DeductpCoins(GameManager.Instance.currentBetAmount);
                    Debug.Log("Private table");
                }
            }
        }

    }

    public void WaitForNewPlayer()
    {
        StartCoroutine(waitForPlayerCo());
    }

    IEnumerator waitForPlayerCo()
    {
        yield return new WaitForSeconds(30f);
        //yield return new WaitForSeconds(1f);
        if (PhotonNetwork.isMasterClient && GameManager.Instance.type != MyGameType.Private)
        {
            Debug.Log("START INVOKE");
            Debug.Log(GameManager.Instance.isMultiplayerBot);

            if (GameManager.Instance.isMultiplayerBot)
            {
                // GameManager.Instance.coinsCount -= GameManager.Instance.currentBetAmount;
                // apiManager.DeductCoins(GameManager.Instance.currentBetAmount);
                CancelInvoke("StartGameWithBots");
                Invoke("StartGameWithBots", StaticStrings.WaitTimeUntilStartWithBots);

            }
        }
    }

    public void StartGameWithBots()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Master Client");
            PhotonNetwork.RaiseEvent((int)EnumPhoton.StartWithBots, null, true, null);
            LoadBots();
        }
        else
        {
            Debug.Log("Not Master client");
        }
    }

    public void LoadBots()
    {
        // PhotonNetwork.room.open = false;fdsgds
        // Add Bots here
        //for (int i = 0; i < GameManager.Instance.requiredPlayers - 1; i++)
        for (int i = GameManager.Instance.currentPlayersCount - 1; i < GameManager.Instance.requiredPlayers- 1; i++)
        {
            //Debug.Log("Step2");
         //   StartCoroutine(AddBot(i));
            if (GameManager.Instance.opponentsIDs[i] == null)
            {
                Debug.Log("Step2");
                StartCoroutine(AddBot(i));
            }
            else 
            {
                GameManager.Instance.opponentsIDs[i] = null;
                StartCoroutine(AddBot(i));
                print("Gamemanager is not null " + GameManager.Instance.opponentsIDs[i]);
            }
        }

    }

    public void PlayofflineMode()
    {
      
        GameManager.Instance.isLocalMultiplayer = true;
        string BotMoves = generateBotMoves();
        extractBotMoves(BotMoves);
        if (GameManager.Instance.type == MyGameType.TwoPlayer)
        {
            GameManager.Instance.requiredPlayers = 2;
        }
        else
            GameManager.Instance.requiredPlayers = 4;

        Debug.Log("Required Player    " + GameManager.Instance.requiredPlayers);
        for (int i = 0; i < GameManager.Instance.requiredPlayers - 1; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] == null)
            {
                // StartCoroutine(AddBot(i));
                GameManager.Instance.opponentsAvatars[i] = avatarSprites[UnityEngine.Random.Range(0, avatarSprites.Length - 1)];
                GameManager.Instance.opponentsIDs[i] = "_BOT" + i;
                GameManager.Instance.opponentsNames[i] = "Computer " + (i + 1);
            }
        }
        LoadGameScene();
    }

    public void PlayOfflineWithReal()
    {
        GameManager.Instance.isPlayingWithComputer = false;
        GameManager.Instance.isLocalMultiplayer = true;
        if (GameManager.Instance.type == MyGameType.TwoPlayer)
            GameManager.Instance.requiredPlayers = 2;
        else
            GameManager.Instance.requiredPlayers = 4;

        Debug.Log("Required Player    " + GameManager.Instance.requiredPlayers + "   type  " + GameManager.Instance.type);
        for (int i = 0; i < GameManager.Instance.requiredPlayers - 1; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] == null)
            {
                // StartCoroutine(AddBot(i));
                GameManager.Instance.opponentsAvatars[i] = avatarSprites[UnityEngine.Random.Range(0, avatarSprites.Length - 1)];
                GameManager.Instance.opponentsIDs[i] = "_BOT" + i;
                GameManager.Instance.opponentsNames[i] = "Player " + (i + 1);
            }
        }
        LoadGameScene();
    }

    public IEnumerator AddBot(int i)
    {
        yield return new WaitForSeconds(i + UnityEngine.Random.Range(0.0f, 0.9f));
        Debug.Log("Step3");
        GameManager.Instance.opponentsAvatars[i] = avatarSprites[UnityEngine.Random.Range(0, avatarSprites.Length - 1)];
        GameManager.Instance.opponentsIDs[i] = "_BOT" + i;
        GameManager.Instance.opponentsNames[i] = staticGameVariables.Player_name[UnityEngine.Random.Range(0, 5)]; //"Guest" + UnityEngine.Random.Range (100000, 999999);
        Debug.Log("Name: " + GameManager.Instance.opponentsNames[i]);
        Debug.Log(GameManager.Instance.isLocalMultiplayer);
       if (!GameManager.Instance.isLocalMultiplayer)
        GameManager.Instance.controlAvatars.PlayerJoined(i, "_BOT" + i);
    }


    public void setInitNewAccountData(bool fb)
    {
        Dictionary<string, string> data = MyPlayerData.InitialUserData(fb);
        GameManager.Instance.myPlayerData.UpdateUserData(data);
    }

    public void updateBoughtChats(int index)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(MyPlayerData.ChatsKey, GameManager.Instance.myPlayerData.GetChats() + ";'" + index + "'");

        GameManager.Instance.myPlayerData.UpdateUserData(data);

    }

    public void UpdateBoughtEmojis(int index)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(MyPlayerData.EmojiKey, GameManager.Instance.myPlayerData.GetEmoji() + ";'" + index + "'");

        GameManager.Instance.myPlayerData.UpdateUserData(data);
    }

    public void addCoinsRequest(int count)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(MyPlayerData.CoinsKey, "" + (GameManager.Instance.myPlayerData.GetCoins() + count));
        GameManager.Instance.myPlayerData.UpdateUserData(data);
    }

    public void getPlayerDataRequest()
    {
        Debug.Log("Get player data request!!");


        if (SceneManager.GetActiveScene().name == "MenuScene")
            splashCanvas.SetActive(false);
       
        //if (!PlayerPrefs.HasKey("Logintoken"))
        //    apiManager.choosePanel.SetActive(true);

        /* GetUserDataRequest getdatarequest = new GetUserDataRequest () {
             PlayFabId = GameManager.Instance.playfabManager.PlayFabId,
         };

         PlayFabClientAPI.GetUserData (getdatarequest, (result) => 
         {

             Dictionary<string, UserDataRecord> data = result.Data;

             GameManager.Instance.myPlayerData = new MyPlayerData (data, true);

             Debug.Log ("Get player data request finish!!");
             StartCoroutine (loadSceneMenu ());
         }, (error) => {
             Debug.Log ("Data updated error " + error.ErrorMessage);
         }, null);*/

    }

    private IEnumerator loadSceneMenu()
    {
        yield return new WaitForSeconds(0.1f);

        if (isInMaster && isInLobby)
        {
            Debug.Log("Firebase Connected");
            // SceneManager.LoadScene ("MenuScene");
        }
        else
        {
            // StartCoroutine (loadSceneMenu ());
        }

    }

    public void LinkFacebookAccount()
    {

    }

    public void LoginWithFacebook()
    {

    }

    public void CheckIfFirstTitleLogin(string id, bool fb)
    {
        GetUserDataRequest getdatarequest = new GetUserDataRequest()
        {
            PlayFabId = id,

        };

        PlayFabClientAPI.GetUserData(getdatarequest, (result) => {
            Dictionary<string, UserDataRecord> data = result.Data;

            if (!data.ContainsKey(MyPlayerData.TitleFirstLoginKey))
            {
                Debug.Log("First login for this title. Set initial data");
                setInitNewAccountData(fb);
            }

        }, (error) => {
            Debug.Log("Data updated error " + error.ErrorMessage);
        }, null);
    }

    private string androidUnique()
    {
        AndroidJavaClass androidUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityPlayerActivity = androidUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject unityPlayerResolver = unityPlayerActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass androidSettingsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
        return androidSettingsSecure.CallStatic<string>("getString", unityPlayerResolver, "android_id");
    }


    public void Login()
    {
        splashCanvas.SetActive(true);
        string customId = "";
        if (PlayerPrefs.HasKey("unique_identifier"))
        {
            customId = PlayerPrefs.GetString("unique_identifier");
        }
        else
        {
            customId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("unique_identifier", customId);
        }

        Debug.Log("UNIQUE IDENTIFIER: " + customId);

        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = customId //SystemInfo.deviceUniqueIdentifier
        };

        PlayFabClientAPI.LoginWithCustomID(request, (result) => {
            PlayFabId = result.PlayFabId;
            Debug.Log("Got PlayFabID: " + PlayFabId);
            /*
                            Dictionary<string, string> data = new Dictionary<string, string> ();

                            if (result.NewlyCreated) {
                                Debug.Log ("(new account)");
                                setInitNewAccountData (false);

                                string name = result.PlayFabId;
                                name = "Guest";
                                for (int i = 0; i < 6; i++) {
                                    name += UnityEngine.Random.Range (0, 9);
                                }
                                name = GameManager.Instance.nameMy;
                                data.Add (MyPlayerData.AvatarIndexKey, GameManager.Instance.avatarIndex);
                                data.Add ("PlayerName", name);

                            } else {
                                CheckIfFirstTitleLogin (PlayFabId, false);
                                Debug.Log ("(existing account)");
                            }
                            data.Add ("LoggedType", "Guest");*/

            // data.Add (MyPlayerData.AvatarIndexKey, GameManager.Instance.avatarIndex);
            // data.Add ("PlayerName", name);
            // GameManager.Instance.myPlayerData.UpdateUserData (data);
            //PlayerPrefs.SetString ("LoggedType", "Guest");
            //PlayerPrefs.Save ();

            //fbManager.showLoadingCanvas();
            GetPhotonToken();

        },
            (error) => {
                Debug.Log("Error logging in player with custom ID:");
                Debug.Log(error.ErrorMessage);
                GameManager.Instance.connectionLost.showDialog();
            });
    }

    public void GetPlayfabFriends()
    {
        if (alreadyGotFriends)
        {
            Debug.Log("show firneds FFFF");
            if (PlayerPrefs.GetString("LoggedType").Equals("Facebook"))
            {
                fbManager.getFacebookInvitableFriends();
            }
            else
            {

                facebookFriendsMenu.showFriends(null, null, null);
            }
        }
        else
        {
            Debug.Log("IND");
            GetFriendsListRequest request = new GetFriendsListRequest();
            request.IncludeFacebookFriends = true;
            PlayFabClientAPI.GetFriendsList(request, (result) => {

                Debug.Log("Friends list Playfab: " + result.Friends.Count);
                var friends = result.Friends;

                List<string> playfabFriends = new List<string>();
                List<string> playfabFriendsName = new List<string>();
                List<string> playfabFriendsFacebookId = new List<string>();

                chatClient.RemoveFriends(GameManager.Instance.friendsIDForStatus.ToArray());

                List<string> friendsToStatus = new List<string>();

                int index = 0;
                foreach (var friend in friends)
                {

                    playfabFriends.Add(friend.FriendPlayFabId);

                    Debug.Log("Title: " + friend.TitleDisplayName);
                    GetUserDataRequest getdatarequest = new GetUserDataRequest()
                    {
                        PlayFabId = friend.TitleDisplayName,
                    };

                    int ind2 = index;

                    PlayFabClientAPI.GetUserData(getdatarequest, (result2) => {

                        Dictionary<string, UserDataRecord> data2 = result2.Data;
                        playfabFriendsName[ind2] = data2["PlayerName"].Value;
                        Debug.Log("Added " + data2["PlayerName"].Value);
                        GameManager.Instance.facebookFriendsMenu.updateName(ind2, data2["PlayerName"].Value, friend.TitleDisplayName);

                    }, (error) => {

                        Debug.Log("Data updated error " + error.ErrorMessage);
                    }, null);

                    playfabFriendsName.Add("");

                    friendsToStatus.Add(friend.FriendPlayFabId);

                    index++;
                }

                GameManager.Instance.friendsIDForStatus = friendsToStatus;

                chatClient.AddFriends(friendsToStatus.ToArray());

                GameManager.Instance.facebookFriendsMenu.addPlayFabFriends(playfabFriends, playfabFriendsName, playfabFriendsFacebookId);

                if (PlayerPrefs.GetString("LoggedType").Equals("Facebook"))
                {
                    fbManager.getFacebookInvitableFriends();
                }
                else
                {
                    GameManager.Instance.facebookFriendsMenu.showFriends(null, null, null);
                }
            }, OnPlayFabError);
        }

    }

    void OnPlayFabError(PlayFabError error)
    {
        Debug.Log("Playfab Error: " + error.ErrorMessage);
    }

    // #######################  PHOTON  ##########################

    void GetPhotonToken()
    {

        GetPhotonAuthenticationTokenRequest request = new GetPhotonAuthenticationTokenRequest();
        request.PhotonApplicationId = StaticStrings.PhotonAppID.Trim();

        PlayFabClientAPI.GetPhotonAuthenticationToken(request, OnPhotonAuthenticationSuccess, OnPlayFabError);
    }

    void OnPhotonAuthenticationSuccess(GetPhotonAuthenticationTokenResult result)
    {
        string photonToken = result.PhotonCustomAuthenticationToken;
        Debug.Log(string.Format("Yay, logged in session token: {0}", photonToken));
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.AddAuthParameter("username", this.PlayFabId);
        PhotonNetwork.AuthValues.AddAuthParameter("Token", result.PhotonCustomAuthenticationToken);
        PhotonNetwork.AuthValues.UserId = this.PlayFabId;
        PhotonNetwork.ConnectUsingSettings("1.0");
        Debug.LogError("Connecting Using Settings");
        PhotonNetwork.playerName = this.PlayFabId;
        PhotonNetwork.player.UserId = GameManager.Instance.nameMy;

        authToken = result.PhotonCustomAuthenticationToken;
        getPlayerDataRequest();
        connectToChat();

    }

    public void connectToChat()
    {
        chatClient = new ChatClient(this);
        GameManager.Instance.chatClient = chatClient;
        ExitGames.Client.Photon.Chat.AuthenticationValues authValues = new ExitGames.Client.Photon.Chat.AuthenticationValues();
        authValues.UserId = this.PlayFabId;
        authValues.AuthType = ExitGames.Client.Photon.Chat.CustomAuthenticationType.Custom;
        authValues.AddAuthParameter("username", this.PlayFabId);
        authValues.AddAuthParameter("Token", authToken);
        chatClient.Connect(StaticStrings.PhotonChatID, "1.0", authValues);
    }

    public void OnConnected()
    {
        Debug.Log("Photon Chat connected!!!");
        chatClient.Subscribe(new string[] { "invitationsChannel" });
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameManager.Instance.opponentDisconnected = true;

        GameManager.Instance.invitationID = "";

        if (GameManager.Instance.controlAvatars != null)
        {
            Debug.Log("PLAYER DISCONNECTED " + player.NickName);
            if (PhotonNetwork.room.PlayerCount > 1)
            {
                GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = true;
            }
            else
            {
                GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = false;
            }

            int index = GameManager.Instance.opponentsIDs.IndexOf(player.NickName);
            PhotonNetwork.room.IsOpen = true;
            GameManager.Instance.controlAvatars.PlayerDisconnected(index);
        }
    }

    public void showMenu()
    {
        menuCanvas.gameObject.SetActive(true);

        playerName.GetComponent<Text>().text = GameManager.Instance.nameMy;

        if (GameManager.Instance.avatarMy != null)
            playerAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

        splashCanvas.SetActive(false);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed to CHAT - set online status!");
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnUnsubscribed(string[] par)
    {

    }

    public void OnUserUnsubscribed(string par, string par2)
    {

    }

    public void OnUserSubscribed(string par, string par2)
    {

    }

    public void challengeFriend(string id, string message)
    {
        //if (GameManager.Instance.invitationID.Length == 0 || !GameManager.Instance.invitationID.Equals(id))
        //{
        chatClient.SendPrivateMessage(id, "INVITE_TO_PLAY_PRIVATE;" + /*id + this.PlayFabId + ";" +*/ GameManager.Instance.nameMy + ";" + message);
        GameManager.Instance.invitationID = id;
        Debug.Log("Send invitation to: " + id);
        // }
    }

    string roomname;
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!sender.Equals(this.PlayFabId))
        {
            if (message.ToString().Contains("INVITE_TO_PLAY_PRIVATE"))
            {
                GameManager.Instance.invitationID = sender;

                string[] messageSplit = message.ToString().Split(';');
                string whoInvite = messageSplit[1];
                string payout = messageSplit[2];
                string roomID = messageSplit[3];
                GameManager.Instance.payoutCoins = int.Parse(payout);
                GameManager.Instance.invitationDialog.GetComponent<PhotonChatListener>().showInvitationDialog(0, whoInvite, payout, roomID, 0);
            }
        }

        if ((GameManager.Instance.invitationID.Length == 0 || !GameManager.Instance.invitationID.Equals(sender)))
        {

        }
        else
        {
            GameManager.Instance.invitationID = "";
        }
    }

    public void join()
    {
        GameManager.Instance.JoinedByID = true;
        PhotonNetwork.JoinRoom(roomname);
    }

    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnChatStateChange(ChatState state)
    {

    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from photon");
        //  if (!PhotonNetwork.room.CustomProperties.ContainsKey ("bt"))
        // {
        //    switchUser ();
        // }

    }

    public void DisconnecteFromPhoton()
    {
        PhotonNetwork.Disconnect();
    }

    public void switchUser()
    {
        
        GameManager.Instance.playfabManager.destroy();
        GameManager.Instance.facebookManager.destroy();
        GameManager.Instance.connectionLost.destroy();
        //GameManager.Instance.adsScript.destroy();
        // GameManager.Instance.avatarMy = null;
        GameManager.Instance.logged = false;
        GameManager.Instance.resetAllData();
        SceneManager.LoadScene("LoginSplash");
    }

    public void OnDisconnected()
    {
        //        Debug.Log ("Chat disconnected - Reconnect");
        //    connectToChat ();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {

    }

    public void OnUserUnsubscribed(string[] channels)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }

    public override void OnConnectedToMaster()
    {
        isInMaster = true;
        Debug.Log("Connected to master");

        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        isInLobby = true;
    }

    public void JoinRoomAndStartGame()
    {
        if (GameManager.Instance.currentBetAmount <= GameManager.Instance.coinsCount)
        {
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "m", GameManager.Instance.mode.ToString () + GameManager.Instance.type.ToString () + GameManager.Instance.payoutCoins.ToString () }
            };
            Debug.Log("Join Room: " + GameManager.Instance.mode.ToString() + GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString());
            Debug.Log(expectedCustomRoomProperties["m"]);
            // PhotonNetwork.player.NickName = GameManager.Instance.nameMy;

            StartCoroutine(TryToJoinRandomRoom(expectedCustomRoomProperties));
        }
        else
        {
            GameManager.Instance.dialog.SetActive(true);
        }

        //PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public IEnumerator TryToJoinRandomRoom(ExitGames.Client.Photon.Hashtable roomOptions)
    {
        Debug.LogError("WTF");
        while (true)
        {
            if (isInLobby && isInMaster)
            {
                PhotonNetwork.JoinRandomRoom(null, 0);
                break;
            }
            else
            {
                yield return new WaitForSeconds(10f);
            }
        }
    }

    public void OnPhotonRandomJoinFailed()
    {
        Debug.LogError("Random join failed");
        RoomOptions roomOptions = new RoomOptions();
        GameManager.Instance.isMultiplayerBot = true;
        if (GameManager.Instance.isMultiplayerBot)
        {

            roomOptions.CustomRoomPropertiesForLobby = new String[] { "m", "v" };
            string BotMoves = "";

            BotMoves = generateBotMoves();

            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "m", GameManager.Instance.mode.ToString () + GameManager.Instance.type.ToString () + GameManager.Instance.payoutCoins.ToString () }, { "bt", BotMoves }, { "fp", UnityEngine.Random.Range (0, GameManager.Instance.requiredPlayers) }
            };

            Debug.Log("Create Room: " + GameManager.Instance.mode.ToString() + GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString());
            roomOptions.MaxPlayers = (byte)GameManager.Instance.requiredPlayers;
            StartCoroutine(TryToCreateGameAfterFailedToJoinRandom(roomOptions));
        }
        else
        {

            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "m", GameManager.Instance.mode.ToString () + GameManager.Instance.type.ToString () + GameManager.Instance.payoutCoins.ToString () }
            };
            if (GameManager.Instance.type == MyGameType.TwoPlayer)
            {
                roomOptions.MaxPlayers = 2;

            }
            else
            {
                roomOptions.MaxPlayers = 4;
            }

            GameManager.Instance.requiredPlayers = roomOptions.MaxPlayers;
            Debug.Log(roomOptions.CustomRoomProperties["m"]);
            Debug.Log("Create Room: " + GameManager.Instance.mode.ToString() + GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString());
            StartCoroutine(TryToCreateGameAfterFailedToJoinRandom(roomOptions));
        }
        //roomOptions.IsVisible = true;
    }

    public string generateBotMoves()
    {
        GameManager.Instance.isPlayingWithComputer = true;
        // Generate BOT moves
        string BotMoves = "";
        int BotCount = 100;
        int count = 0;
        // Generate dice values
        for (int i = 0; i < BotCount; i++)
        {
            int step = 0;
            GameManager.Instance.isHard = true;
            if (!GameManager.Instance.isHard)
            {
                step = UnityEngine.Random.Range(1, 7);
            }
            else
            {
                float x = UnityEngine.Random.Range(0.0f, 10.0f);
                if (x <= 7.5f)
                {
                    step = UnityEngine.Random.Range(4, 7);
                }
                else
                {
                    step = UnityEngine.Random.Range(1, 4);
                }
            }

            if (step == 6)
            {
                if (count == 2)
                {
                    step = UnityEngine.Random.Range(1, 6);
                    count = 0;
                }
                count++;
            }
            else
            {
                count = 0;
            }
            BotMoves += (step).ToString();
            if (i < BotCount - 1)
            {
                BotMoves += ",";
            }
        }

        BotMoves += ";";

        // Generate delays
        float minValue = GameManager.Instance.playerTime / 10;
        if (minValue < 1.5f) minValue = 1.5f;
        for (int i = 0; i < BotCount; i++)
        {
            BotMoves += (UnityEngine.Random.Range(minValue, GameManager.Instance.playerTime / 8)).ToString();
            if (i < BotCount - 1)
            {
                BotMoves += ",";
            }
        }
        Debug.Log("BotMoves   " + BotMoves);
        return BotMoves;
    }

    public void extractBotMoves(string data)
    {
        GameManager.Instance.botDiceValues = new List<int>();
        GameManager.Instance.botDelays = new List<float>();
        string[] d1 = data.Split(';');

        string[] diceValues = d1[0].Split(',');
        for (int i = 0; i < diceValues.Length; i++)
        {
            GameManager.Instance.botDiceValues.Add(int.Parse(diceValues[i]));
        }

        Debug.Log("botDiceValues   " + GameManager.Instance.botDiceValues[0] + " 1  " + GameManager.Instance.botDiceValues[1]);

        string[] delays = d1[1].Split(',');
        for (int i = 0; i < delays.Length; i++)
        {
            GameManager.Instance.botDelays.Add(float.Parse(delays[i]));
        }
    }

    public void OnLeftLobby()
    {
        isInLobby = false;
        isInMaster = false;
    }

    public IEnumerator TryToCreateGameAfterFailedToJoinRandom(RoomOptions roomOptions)
    {
        Debug.LogError("FFF");
        while (true)
        {
            Debug.LogError("1432F");
            if (isInLobby && isInMaster)
            {
                Debug.LogError("22F");
                PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);

                break;
            }
            else
            {
                Debug.LogError("111");
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        //Debug.LogError("Joined room");
        if (ReferenceManager.refMngr.loadingPanel.activeInHierarchy) ReferenceManager.refMngr.CloseLoadingPanel();
        if (PhotonNetwork.room.CustomProperties.ContainsKey("bt"))
        {
            Debug.Log("room proper   " + PhotonNetwork.room.CustomProperties["bt"].ToString());
            extractBotMoves(PhotonNetwork.room.CustomProperties["bt"].ToString());
        }

        if (PhotonNetwork.room.CustomProperties.ContainsKey("pc"))
        {

            GameManager.Instance.payoutCoins = float.Parse(PhotonNetwork.room.CustomProperties["pc"].ToString());
            Debug.Log(GameManager.Instance.payoutCoins);
        }
        if (PhotonNetwork.room.CustomProperties.ContainsKey("fp"))
        {
            GameManager.Instance.firstPlayerInGame = int.Parse(PhotonNetwork.room.CustomProperties["fp"].ToString());
        }
        else
        {
            GameManager.Instance.firstPlayerInGame = 0;
        }

        GameManager.Instance.avatarOpponent = null;

        Debug.Log("Players in room " + PhotonNetwork.room.PlayerCount);

        GameManager.Instance.currentPlayersCount = 1;

        GameManager.Instance.controlAvatars.setCancelButton();
        if (PhotonNetwork.room.PlayerCount == 1)
        {
            GameManager.Instance.roomOwner = true;
            WaitForNewPlayer();
        }
        else if (PhotonNetwork.room.PlayerCount >= GameManager.Instance.requiredPlayers)
        {
            PhotonNetwork.room.IsOpen = false;
        }

        if (!roomOwner)
        {
            GameManager.Instance.backButtonMatchPlayers.SetActive(false);

            for (int i = 0; i < PhotonNetwork.otherPlayers.Length; i++)
            {

                int ii = i;
                int index = GetFirstFreeSlot();
                GameManager.Instance.opponentsIDs[index] = PhotonNetwork.otherPlayers[ii].NickName;
                GameManager.Instance.opponentsNames[index] = PhotonNetwork.otherPlayers[ii].CustomProperties["name"].ToString();
                GameManager.Instance.opponentsAvatars[index] = GameManager.Instance.playfabManager.staticGameVariables.avatars[int.Parse(PhotonNetwork.otherPlayers[ii].CustomProperties["avatarId"].ToString())];
                PhotonNetwork.otherPlayers[ii].CustomProperties["name"].ToString();
                Debug.Log(" Name " + PhotonNetwork.otherPlayers[ii].CustomProperties["name"].ToString());
                string otherID = PhotonNetwork.otherPlayers[ii].NickName;
                getOpponentData(index + 1, otherID);

            }
        }
    }

    public void CreatePrivateRoom(string roomName)
    {
        Debug.LogError("Creating room");
        if (GameManager.Instance.currentBetAmount <= GameManager.Instance.coinsCount)
        {
            Debug.LogError("Coins are less man");
            GameManager.Instance.JoinedByID = false;
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            Debug.Log(GameManager.Instance.payoutCoins);
            roomOptions.CustomRoomPropertiesForLobby = new String[] { "pc","betAmount", "privateRoom" };
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "pc", GameManager.Instance.payoutCoins }, {"betAmount",GameManager.Instance.currentBetAmount},{"privateRoom", true}
            };
            Debug.Log("Private room name: " + roomName);
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
        else
        {
            GameManager.Instance.dialog.SetActive(true);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        if (ReferenceManager.refMngr.loadingPanel.activeInHierarchy) ReferenceManager.refMngr.CloseLoadingPanel();
        roomOwner = true;
        GameManager.Instance.roomOwner = true;
        GameManager.Instance.currentPlayersCount = 1;
        GameManager.Instance.controlAvatars.updateRoomID(PhotonNetwork.room.Name);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom called");

        //if (PhotonNetwork.room.CustomProperties != null)
        //{
        //    if (PhotonNetwork.room.CustomProperties.ContainsKey("bt"))
        //    {
        //        roomOwner = false;
        //        GameManager.Instance.roomOwner = false;
        //        GameManager.Instance.resetAllData();
        //    }
        //}

        if (GameManager.Instance.doesContainBotMoves)
        {
            roomOwner = false;
            GameManager.Instance.roomOwner = false;
            GameManager.Instance.resetAllData();
            Debug.LogError("LEft Room Working");
        }
    }

    public int GetFirstFreeSlot()
    {
        int index = 0;
        for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] == null )
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Failed to create room");
        // CreatePrivateRoom();
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Failed to join room");

        if (GameManager.Instance.type == MyGameType.Private)
        {
            if (GameManager.Instance.controlAvatars != null)
            {
                GameManager.Instance.controlAvatars.ShowJoinFailed(codeAndMsg[1].ToString());
            }
        }
        else
        {
            GameManager.Instance.facebookManager.startRandomGame();
        }
    }

    private void GetPlayerDataRequest(string playerID)
    {

    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("New player joined " + newPlayer.NickName);
        Debug.Log("Players Count: " + GameManager.Instance.currentPlayersCount);
        Debug.Log("PLayer id" + newPlayer.ID + " nick name" + newPlayer.NickName);

        if (PhotonNetwork.room.PlayerCount >= GameManager.Instance.requiredPlayers)
        {
            PhotonNetwork.room.IsOpen = false;
        }

        if (PhotonNetwork.room.PlayerCount > 1)
        {
            GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = true;
        }
        else
        {
            GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = true;
        }

        int index = GetFirstFreeSlot();

        GameManager.Instance.opponentsIDs[index] = newPlayer.NickName;
        GameManager.Instance.opponentsNames[index] = newPlayer.CustomProperties["name"].ToString();
        GameManager.Instance.opponentsAvatars[index] = GameManager.Instance.playfabManager.staticGameVariables.avatars[int.Parse(newPlayer.CustomProperties["avatarId"].ToString())];
        //  GameManager.Instance.opponentsNames[index] = newPlayer.CustomProperties[];
        getOpponentData(newPlayer.ID, newPlayer.NickName);
    }

    private void getOpponentData(int index, string id)
    {
        Debug.Log("GET OPPONENT DATA: ");
        //  GameManager.Instance.opponentsAvatars[index] =  GameManager.Instance.playfabManager.staticGameVariables.avatars[index];
        GameManager.Instance.controlAvatars.PlayerJoined(index, id);
    }

    public void GetReferCode()
    {
        string url = "https://app.ludocashwin.com/public/api/test-notification";
        WWWForm form = new WWWForm();
        //form.AddField("device_token",);
        form.AddField("title","Reffer Code");
        form.AddField("message",GameManager.Instance.nameMy+"Send you the reffer code.");
        WWW w = new WWW(url, form);
      //  Debug.Log("Device"+ token.Token);
        Debug.Log("Title"+ "me");
        Debug.Log("Me"+ GameManager.Instance.nameMy + "Send you the reffer code.");
        StartCoroutine(NotificationTest(w));
    }
    IEnumerator NotificationTest(WWW w) {
        yield return w;
        Debug.Log(w.text);
    }

    public void GetAccountCode()
    {
        string url = "https://app.ludocashwin.com/public/api/test-notification";
        WWWForm form = new WWWForm();
      //  form.AddField("device_token",token.Token);
        form.AddField("title", "Reffer Code");
        form.AddField("message", GameManager.Instance.nameMy + "Send you Amount.");
        WWW w = new WWW(url, form);
       // Debug.Log("Device" + token.Token);
        Debug.Log("Title" + "me");
        Debug.Log("Me" + GameManager.Instance.nameMy + "Send you the Amount.");
        StartCoroutine(Account(w));
    }
    IEnumerator Account(WWW w)
    {
        yield return w;
        Debug.Log(w.text);
    }
    public void OnLogout()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoginSplash");
    }
}