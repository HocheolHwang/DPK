using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    private void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();

        foreach(var player in PhotonNetwork.PlayerList)
        {
            if(player.CustomProperties.TryGetValue("viewID", out object vid))
            {
                int viewId = (int)vid;
                if(photonView.ViewID == viewId)
                {
                    if(player.CustomProperties.TryGetValue("PlayerLevel", out object level))
                    {
                        int playerLevel = (int)level;
                        MaxHp += 20 * playerLevel;
                        Hp += 20 * playerLevel;
                        AttackDamage += 5 * playerLevel;
                    }
                }
            }
            
        }
    }
}
