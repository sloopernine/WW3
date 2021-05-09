using System;
using System.Collections.Generic;

namespace Data.DataContainers
{
    [Serializable]
    public class UserInfo
    {
        public string userID;
        public string nickname;
        public List<ActiveGame> activeGames = new List<ActiveGame>();
    }

    [Serializable]
    public class ActiveGame
    {
        public string gameID;
        public int currentTurn;
        public bool firstStart;
        public List<PlayerList> playerList = new List<PlayerList>();
    }
    
    [Serializable]
    public class PlayerList
    {
        public string userID;
        public bool isAlive;
    }
}