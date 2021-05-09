using System;
using System.Collections.Generic;
using UnityEngine;


namespace Data.Enums
{
    [Serializable]
    public class GameData
    {
        public string gameID;
        public string creationDate;
        public string gameName;
        public string creatorUserID;
        public bool isPublic;
        public int worldSeed; //TODO Remove?
        public List<PlayerInfo> players = new List<PlayerInfo>();
        public int currentPlayerTurn;
        public int currentTurn;
        public bool firstTurn;
    }
    
    [Serializable]
    public class PlayerInfo
    {
        public string playerID;
        public string nickname;
        public Vector2 position; //TODO Remove?
        public bool isAlive;
        public float angle;
        public float firepower;
        public bool leftGame;

        public PlayerInfo()
        {
            
        }
        
        public PlayerInfo(string playerID, string nickname)
        {
            this.playerID = playerID;
            this.nickname = nickname;
            position = Vector2.zero;
            isAlive = true;
            angle = 0;
            firepower = 10;
            leftGame = false;
        }
    }
}