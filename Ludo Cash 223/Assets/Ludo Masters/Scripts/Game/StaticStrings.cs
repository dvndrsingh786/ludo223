namespace AssemblyCSharp
{
    public static class StaticStrings
    {
        public static string SaveGameString = "SaveGames";
        public static string AndroidPackageName = "com.game.ludocastle";
        public static string ITunesAppID = "678544535";

        // Notifications
        public static string notificationTitle = "Ludo Masters";
        public static string notificationMessage = "Get your FREE fortune spin!";

        // Game configuration
        public static float WaitTimeUntilStartWithBots = 1.0f; // Time in seconds. If after that time new player doesnt join room game will start with bots

        // Services configration IDS
        public static string PlayFabTitleID = "64592";
        public static string PhotonAppID = "55cdf791-0ae5-49ad-947c-19707cc77dbe";
        public static string PhotonChatID = "e08d66f9-a5d2-4d7d-802e-1437aec25fe2";

        // Facebook share variables
        public static string facebookShareLinkTitle = "I'm playing Ludo Masters!. Available on Android and iOS.";

        // Share private room code
        public static string SharePrivateLinkMessage = "Join me in Ludo Game. My PRIVATE ROOM CODE is:";
        public static string SharePrivateLinkMessage2 = "Download Ludo Game from:";
        public static string ShareScreenShotText = "I finished game in Ludo Masters. It's my score :-) Join me and download Ludo Game:";

        // Initial coins count for new players
        // When logged as Guest
      
        public static int initCoinsCountGuest =20;
        
        //When logged via Facebook
        public static int initCoinsCountFacebook = 20000;
        //When logged as Guest and then link to Facebook
        public static int CoinsForLinkToFacebook = 15000;

        // Unity Ads - reward coins count for watching video
        public static int rewardForVideoAd = 250;

        // Facebook Invite variables
        public static string facebookInviteMessage = "Come play this great game!";
        public static int rewardCoinsForFriendInvite = 250;
        public static int rewardCoinsForShareViaFacebook = 50;

        // String to add coins for testing - To add coins start game, click "Edit" button on your avatar and put that string
        // It will add 1 000 000 coins so you can test tables, buy items etc.
        public static string addCoinsHackString = "Cheat:AddCoins";

        // Hide Coins tab in shop (In-App Purchases)
        public static bool hideCoinsTabInShop = false;
        public static string runOutOfTime = "ran out of time";
        public static string waitingForOpponent = "Waiting for your opponent";

        // Other strings
        public static string youAreBreaking = "You start, good luck";
        public static string opponentIsBreaking = "is starting";
        public static string IWantPlayAgain = "I want to play again!";
        public static string cantPlayRightNow = "Can't play right now";

        // Players names for training mode
        public static string offlineModePlayer1Name = "Player 1";
        public static string offlineModePlayer2Name = "Player 2";

        // Photon configuration
        // Timeout in second when player will be disconnected when game in background
        public static float photonDisconnectTimeout = 2.2f; // In game scene - its better to don't change it. Player that loose focus on app will be immediately disconnected
        public static float photonDisconnectTimeoutLong = 300.0f; // In menu scene etc. 
        public static float photonDisconnectTimeoutOneMinute = 60.0f; // In menu scene etc. 

        // Bids Values
        public static int[] bidValues = new int[] { 100 };//, 2000, 10000, 50000, 250000, 1000000, 2000000, 5000000 };
        public static string[] bidValuesStrings = new string[] { "100" };//, "2000", "10k", "50k", "250k", "1M", "2M", "5M" };

        public static bool isFourPlayerModeEnabled = true;

        // Settings PlayerPrefs keys
        public static string SoundsKey = "EnableSounds";
        public static string VibrationsKey = "EnableVibrations";
        public static string NotificationsKey = "EnableNotifications";
        public static string FriendsRequestesKey = "EnableFriendsRequestes";
        public static string PrivateRoomKey = "EnablePrivateRoomRequestes";
        public static string PrefsPlayerRemovedAds = "UserRemovedAds";

        // Standard chat messages
        public static string[] chatMessages = new string[] {
            "Please don't kill",
            "Play Fast",
            "I will eat you",
            "You are good",
            "Well played",
            "Today is your day",
            "Hehehe",
            "Unlucky",
            "Thanks",
            "Yeah",
            "Remove Blockade",
            "Good Game",
            "Oops",
            "Today is my day",
            "All the best",
            "Hi",
            "Hello",
            "Nice move"
        };

        // Additional chat messages
        // Prices for chat packs
        public static int[] chatPrices = new int[] { 1000, 5000, 10000, 50000, 100000, 250000 };
        public static int[] emojisPrices = new int[] { 1000, 5000, 10000, 50000, 100000 };

        // Chat packs names
        public static string[] chatNames = new string[] { "Motivate", "Emoticons", "Cheers", "Gags", "Laughing", "Talking" };

        // Chat packs strings
        public static string[][] chatMessagesExtended = new string[][] {
            new string[] {
                    "Never give up",
                    "You can do it",
                    "I know you have it in you!",
                    "You play like a pro!",
                    "You can win now!",
                    "You're great!"
                },
                new string[] {
                    ":)",
                    ":(",
                    ":o",
                    ";D",
                    ":P",
                    ":|"
                },
                new string[] {
                    "Keep it going",
                    "Go opponents!",
                    "Fabulastic",
                    "You're awesome",
                    "Best shot ever",
                    "That was amazing",
                },
                new string[] {
                    "OMG",
                    "LOL",
                    "ROFL",
                    "O'RLY?!",
                    "CYA",
                    "YOLO"
                },
                new string[] {
                    "Hahaha!!!",
                    "Ho ho ho!!!",
                    "Mwhahahaa",
                    "Jejeje",
                    "Booooo!",
                    "Muuuuuuuhhh!"
                },
                new string[] {
                    "Yes",
                    "No",
                    "I don't know",
                    "Maybe",
                    "Definitely",
                    "Of course"
                }
        };

    }
}