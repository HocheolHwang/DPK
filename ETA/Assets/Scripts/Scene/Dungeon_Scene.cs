using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dungeon_Scene : BaseScene
{
    PlayerZone playerZone;

    bool isStarted;
    int totalPlayerCnt;
    
    private void Start()
    {
        //totalPlayerCnt = PhotonNetwork.PlayerList.Length;

    }

    protected override void Init()
    {
        base.Init();
        Debug.Log("나는 첫째");
        Managers.UI.ShowPopupUI<Dungeon_Popup_UI>("[Dungeon]_Dungeon_Popup_UI");
        Managers.Sound.Play("BackgroundMusic/DeepForest");
        Managers.UI.ShowPopupUI<Fade_Effect_UI>("[Common]_Fade_Effect_UI");

        GameSystem mp = GameObject.FindObjectOfType<GameSystem>();
        mp.SendLoadMsg();

        Managers.Resource.Instantiate("MonsterManager").name = "@MonsterManager";

        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.LocalPlayer.CustomProperties;
        properties["currentScene"] = Define.Scene.Unknown;
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

    }


    public override void Clear()
    {
        Debug.Log("Dungeon Scene Clear");
    }

    private void Update()
    {
        //if(playerZone == null)
        //{
        //    PlayerZone playerZone = FindObjectOfType<PlayerZone>();
        //    if(playerZone != null) Camera.main.GetComponent<CameraController>()._player = playerZone.gameObject;
        //}

    }

    // Dungeon
    //


    // 플레이어가 나갈때 자기 캐릭터 삭제
    public override void OnLeftRoom()
    {
    }
    public void AnyOneDied()
    {
        var players = GameObject.FindObjectsOfType<PlayerController>();

        if (players.Length != 0) return;
        FindObjectOfType<Dungeon_Popup_UI>().HandlePlayerDestroyed();

    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log($"# On Player Left Room {PhotonNetwork.IsMasterClient}");
        if (PhotonNetwork.IsMasterClient == false) return;
        var players = FindObjectsOfType<PlayerController>();

        foreach(var p in players)
        {
            Debug.Log($"# On Player Left Room {PhotonNetwork.IsMasterClient} ... {p.photonView.Owner.ActorNumber} {otherPlayer.ActorNumber}");
            if(otherPlayer.CustomProperties.TryGetValue("viewID", out object value))
            {
                int viewId = (int)value;
                if (p.photonView.ViewID == viewId)
                {
                    PhotonNetwork.Destroy(p.gameObject);
                }
            }

        }
    }


}
