using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dungeon_Scene : BaseScene
{
    PlayerZone playerZone;

    bool isStarted;
    
    private void Start()
    {
        Managers.UI.ShowPopupUI<Dungeon_Popup_UI>("[Dungeon]_Dungeon_Popup_UI");
        Managers.Sound.Play("BackgroundMusic/DeepForest");
        Managers.UI.ShowPopupUI<Fade_Effect_UI>("[Common]_Fade_Effect_UI");

        //Managers.Photon.OnEnterDungeon += InitPlayers;
        //Managers.Photon.Connect();

        //PhotonNetwork.Instantiate("Creatures/Player/Player.prefab", new Vector3(13.09f, 0.5f, 0f),new Quaternion());
        MyPhoton mp = GameObject.FindObjectOfType<MyPhoton>();
        mp.SendLoadMsg();

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.Instantiate("Prefabs/Creatures/Player/PlayerZone", new Vector3(16f, 0.5f, 0f), Quaternion.Euler(0,90,0));
        //}
    }
    
    public override void Clear()
    {
        Debug.Log("Dungeon Scene Clear");
    }

    private void Update()
    {
        if(playerZone == null)
        {
            PlayerZone playerZone = FindObjectOfType<PlayerZone>();
            if(playerZone != null) Camera.main.GetComponent<CameraController>()._player = playerZone.gameObject;
        }

    }

    public void InitPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject playerZone = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/PlayerZone", new Vector3(14.0f, 1.21f, 0f), Quaternion.Euler(0, 90, 0));
            Camera.main.GetComponent<CameraController>()._player = playerZone;
        }
        else
        {

        }
        GameObject player = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/Player", new Vector3(13.09f, 0.5f, 0f), new Quaternion());
    }


}
