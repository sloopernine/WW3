using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public string gameID;
        public string creationDate;
        public string gameName;
        public string creator;
        public int worldSeed;
        public List<PlayerInfo> players = new List<PlayerInfo>();
        public int currentTurn;
        public bool firstTurn;
    }

    [Serializable]
    public class PlayerInfo
    {
        public string playerID;
        public string nickname;
        public Vector2 position;
        public bool isAlive;
        public float angle;
        public float lastAngle;
        public float firepower;
        
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
            firepower = 0;
        }
    }
    
    [Serializable]
    public class UserInfo
    {
        public string userID;
        public string nickname;
        public List<string> activeGames;
    }
}