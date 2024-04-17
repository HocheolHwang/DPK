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

    PlayerManager() { }

    public static PlayerManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerManager();
        }

        return instance;
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

    public string getId()
    {
        return id;
    }
    public string getNickName()
    {
        return nickname;
    }
    public string getToken()
    {
        return accessToken;
    }
    public int getGold()
    {
        return gold;
    }
}
