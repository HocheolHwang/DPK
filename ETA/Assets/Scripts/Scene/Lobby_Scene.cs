using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Scene : BaseScene
{
    // 로비 마네킹 관리를 위한 리스트
    MannequinController[] mannequins = new MannequinController[3];

    private void Start()
    {
        // 첫 로그인 여부를 확인
        bool isFirstLogin = PlayerPrefs.GetInt("FirstLogin", 1) == 1;

        if (Managers.Player.GetFirst() && isFirstLogin)
        {
            // 첫 로그인 O: 자격증 Popup UI를 띄움
            Managers.UI.ShowPopupUI<Certificate_Popup_UI>("[Lobby]_Certificate_Popup_UI");

            // 첫 로그인 여부 0으로 저장
            PlayerPrefs.SetInt("FirstLogin", 0);
            PlayerPrefs.Save();
        }
        else
        {
            // 첫 로그인 X: 로비 Popup UI를 띄움
            Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
        }

        Managers.Sound.Play("BackgroundMusic/Lobby");
        Debug.Log(PhotonNetwork.SerializationRate);
        PhotonNetwork.SerializationRate = 10;
        Debug.Log(PhotonNetwork.PrecisionForFloatSynchronization);
        PhotonNetwork.PrecisionForFloatSynchronization = 0.1f;

        Debug.Log(PhotonNetwork.SendRate);
        PhotonNetwork.SendRate = 60;

        SetUpMannequins();
        mannequins[0].EnterPlayer(Managers.Player.GetNickName(), Managers.Player.GetClassCode());

        //TMP
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        
    }

    public override void Clear()
    {
        Debug.Log("Lobby Scene Clear");
    }


    public override void OnJoinedRoom()
    {
        
        
    }

    public void SetUpMannequins()
    {
        MannequinController[] treeMannequin = GameObject.FindObjectsOfType<MannequinController>();

        Debug.Log(treeMannequin[0].index);

        for (int i = 0; i < 3; i++)
            mannequins[treeMannequin[i].index] = treeMannequin[i];
    }

    public void ChangeMannequin()
    {
        for (int i = 0; i < 3; i++)
        {
            mannequins[i].Init();
        }

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties["PlayerIndex"] == null || player.CustomProperties["CurClass"] == null) return;
            Debug.Log(player.NickName);
            Debug.Log((int)player.CustomProperties["PlayerIndex"]);
            Debug.Log((string)player.CustomProperties["CurClass"]);
            mannequins[(int)player.CustomProperties["PlayerIndex"]].EnterPlayer(player.NickName, (string)player.CustomProperties["CurClass"]);
        }
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        foreach (DictionaryEntry entry in changedProps)
        {
            Debug.Log($"{targetPlayer.NickName}'s Property {entry.Key} changed to {entry.Value}");

            if ((string)entry.Key == "CurClass")
            {
                mannequins[(int)targetPlayer.CustomProperties["PlayerIndex"]].ClassUpdate((string)entry.Value);
            }
            if ((string)entry.Key == "PlayerIndex")
            {
                mannequins[(int)entry.Value].SetNickName(targetPlayer.NickName);
            }
        }

        ChangeMannequin();
    }

    public override void OnLeftRoom()
    {
        mannequins[0].EnterPlayer(Managers.Player.GetNickName(), Managers.Player.GetClassCode());
        mannequins[1].Init();
        mannequins[2].Init();
    }

}
