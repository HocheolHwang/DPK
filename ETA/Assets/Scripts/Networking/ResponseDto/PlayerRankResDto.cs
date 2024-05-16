using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRankResDto 
{
    public string message;
    public PlayerRank[] rankingList;
    public int personalRanking;
}

[System.Serializable]
public class PlayerRank
{
    public string className;
    public string nickname;
    public int playerLevel;
}