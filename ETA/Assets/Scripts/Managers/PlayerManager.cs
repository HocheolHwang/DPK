using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    private static PlayerManager instance;
    private string id;
    private string nickname;
    private string accessToken;
    private int gold;
    private string playerId;
    private string curClass;
    private int index;
    private bool partyLeader;
    private bool first;

    public PlayerManager() { }

    public static PlayerManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerManager();
        }

        return instance;
    }

    public void init()
    {
        partyLeader = false;
        index = 0;
    }

    public void SetId(string id)
    {
        this.id = id;
    }
    public void SetNickName(string nickname)
    {
        this.nickname = nickname;
    }
    public void SetToken(string accessToken)
    {
        this.accessToken = accessToken;
    }
    public void SetGold(int gold)
    {
        this.gold = gold;
    }
    
    public void SetPlayerId(string playerId)
    {
        this.playerId = playerId;
    }
    public void SetClassCode(string curClass)
    {
        this.curClass = curClass;
    }
    
    public void SetIndex(int index)
    {
        this.index = index;
    }
    
    public void SetPartyLeader(bool partyLeader)
    {
        this.partyLeader = partyLeader;
    }
    public void SetFirst(bool first)
    {
        this.first = first;
    }

    public string GetId()
    {
        return id;
    }
    public string GetNickName()
    {
        return nickname;
    }
    public string GetToken()
    {
        return accessToken;
    }
    public int GetGold()
    {
        return gold;
    }

    public string GetPlayerId()
    {
        return playerId;
    }
    public string GetClassCode()
    {
        return curClass;
    }
    public int GetIndex()
    {
        return index;
    }
    public bool GetPartyLeader()
    {
        return partyLeader;
    }
    public bool GetFirst()
    {
        return first;
    }
}
