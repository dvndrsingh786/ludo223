using UnityEngine;
using System.Collections.Generic;



[System.Serializable]
class GameData
{
    public string Date, Time, Day;
    public int numberofPlayers;

    public int currentPlayer;
    public bool isDiceRolled;
    public int diceNumber;

    public List<PlayerData> _PD_List;

    public GameData(int numberOfPlayers, int currentPlayer, bool isDiceRolled, int diceNumber)
    {

        this.numberofPlayers = numberOfPlayers;
        _PD_List = new List<PlayerData>();
        Date = System.DateTime.Now.Date.ToLongDateString();
        Date = Date.Remove(0, Date.IndexOf(","));
        Day = System.DateTime.Now.Date.DayOfWeek.ToString();
        Day = Day.Substring(0, 3);
        Time = System.DateTime.Now.ToShortTimeString();
        this.currentPlayer = currentPlayer;
        this.isDiceRolled = (isDiceRolled);
        this.diceNumber = diceNumber;

    }

    public void AddPoistion(GameObject[] Pawn)
    {
        int playerindex = Pawn[0].GetComponent<LudoPawnController>().playerIndex;
        PlayerData _PD = new PlayerData(4, playerindex);
        for (int index = 0; index < 4; index++)
        {
            _PD.dicePos[index] = Pawn[index].GetComponent<LudoPawnController>().currentPosition;
        }
        _PD_List.Add(_PD);
    }


}

[System.Serializable]
class PlayerData
{
    public int[] dicePos;
    public int playerIndex;
    public PlayerData(int numberOfDices, int playerIndex)
    {
        dicePos = new int[numberOfDices];
        this.playerIndex = playerIndex;
    }
}
