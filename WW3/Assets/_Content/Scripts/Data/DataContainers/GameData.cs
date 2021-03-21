using System;
using System.Collections.Generic;

namespace Data.DataContainers
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
}