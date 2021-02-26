using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    [Serializable]
    public class GameData
    {
        public string gameID;
        public string GameName;
        public int worldSeed;
        public List<PlayerInfo> players;
        public int currentPlayerTurn;
    }

    [Serializable]
    public class PlayerInfo
    {
        public string playerID;
        public string nickname;
        public Vector2 position;
        public bool isAlive;
        public int money;
        public float angle;
        public float firepower;
        public string selectedWeapon;
        public List<UnlockedWeapons> unlockedWeapons;
        public float secondDetonationTime;
        public Vector2 secondDetonationPosition;
    }

    [Serializable]
    public class UnlockedWeapons
    {
        public bool cannon;
        public bool carpetBomb;
    }

    [Serializable]
    public class UserInfo
    {
        public string nickname;
        public List<string> activeGames;
    }
}