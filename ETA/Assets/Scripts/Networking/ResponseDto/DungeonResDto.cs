using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 던전 랭크는 리스트로 받아오기 때문에 배열 형식으로 받아와서 저장하기
[System.Serializable]
public class DungeonRankListResDto
{
    public string message;
    public DungeonResDto[] rankingList;
}

[System.Serializable]
public class DungeonResDto
{
    public string partyTitle;
    public string[] playerList;
    public int clearTime; 
    public string createdAt;
}
