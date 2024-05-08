using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Scene : BaseScene
{
    // 로비 마네킹 관리를 위한 리스트
    MannequinController[] mannequins = new MannequinController[3];
    public bool isSoloPlay;

    public int currentDungeonNumber;

    private void Start()
    {
        // 첫 로그인 여부를 확인
        bool isFirstLogin = PlayerPrefs.GetInt("FirstLogin", 1) == 1;
        currentDungeonNumber = 1; // 깊은 숲 설정
        //Managers.Photon.DungeonIndex = 1;

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

        
        SetUpMannequins();

        // 다시 들어 왔을떄는?
        if (PhotonNetwork.InRoom)
            ChangeMannequin();
        else
        {
            // 캐릭터 바꿀때 마다 이거 또 해줘야할듯
            mannequins[0].EnterPlayer(Managers.Player.GetNickName(), Managers.Player.GetClassCode());
            mannequins[1].Init();
            mannequins[2].Init();
        }


        // 파티 유지할 경우, GameSystem 초기화 시킴
        GameSystem gameSystem = FindObjectOfType<GameSystem>();
        if (gameSystem != null)
        {
            gameSystem.Clear();
        }

        // TMP 임시 코드
        if (PhotonNetwork.InRoom == false)
        {
            //PhotonNetwork.JoinRandomOrCreateRoom();
        }
        if (Managers.Photon.IsConnecting && PhotonNetwork.InRoom ) Managers.Photon.OpenRoom();

        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.LocalPlayer.CustomProperties;
        properties["currentScene"] = Define.Scene.Lobby;
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

    }

    public override void Clear()
    {
        Debug.Log("Lobby Scene Clear");
    }


    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            Managers.Player.SetPartyLeader(true);
            GameObject gameSystem = PhotonNetwork.Instantiate("Prefabs/GameSystem", new Vector3(), new Quaternion());
            gameSystem.name = "GameSystem";
            DontDestroyOnLoad(gameSystem);
        }
        else
        {
            if(PhotonNetwork.CurrentRoom.CustomProperties["dungeonIndex"] != null)
            {
                currentDungeonNumber = (int)PhotonNetwork.CurrentRoom.CustomProperties["dungeonIndex"];
            }
            
        }

        Managers.Photon.updatePlayerList();
        //Managers.Photon.SetPlayerClass();



        // Popup 에서 생성하고 시작될 예정
        if (isSoloPlay)
        {
            FindObjectOfType<Dungeon_Enter_Popup_UI>().DungeonEnter(null);
            return;
        }

        FindObjectOfType<Lobby_Popup_UI>().UpdatePartyInfo();
        FindObjectOfType<Lobby_Popup_UI>().UpdateSelectedDungeon();


    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        FindObjectOfType<Lobby_Popup_UI>().UpdatePartyInfo();
    }

    public void SetUpMannequins()
    {
        MannequinController[] treeMannequin = GameObject.FindObjectsOfType<MannequinController>();

        //Debug.Log(treeMannequin[0].index);

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
            //Debug.Log(player.NickName);
            //Debug.Log((int)player.CustomProperties["PlayerIndex"]);
            //Debug.Log((string)player.CustomProperties["CurClass"]);
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



    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
    }

    public override void OnLeftRoom()
    {

        mannequins[0].EnterPlayer(Managers.Player.GetNickName(), Managers.Player.GetClassCode());
        mannequins[1].Init();
        mannequins[2].Init();
        FindObjectOfType<Lobby_Popup_UI>().UpdatePartyInfo();
    }

}
