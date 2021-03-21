using System;
using System.Collections.Generic;

namespace Data.DataContainers
{
    [Serializable]
    public class UserInfo
    {
        public string userID;
        public string nickname;
        public List<ActiveGames> activeGames = new List<ActiveGames>();
    }

    [Serializable]
    public class ActiveGames
    {
        public string gameID;
        public int currentTurn;
        public List<PlayerList> playerList = new List<PlayerList>();
    }
    
    [Serializable]
    public class PlayerList
    {
        public string userID;
        public bool isAlive;
    }
}