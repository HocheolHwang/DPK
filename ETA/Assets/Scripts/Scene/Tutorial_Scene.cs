using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Scene : BaseScene
{
    GameSystem gameSystem;
    protected override void Init()
    {
        base.Init();
        Managers.UI.ShowPopupUI<Dungeon_Popup_UI>("[Dungeon]_Dungeon_Popup_UI");
        Managers.Sound.Play("BackgroundMusic/Tutorial");

        GameSystem gameSystem = GameObject.FindObjectOfType<GameSystem>();
        gameSystem.SendLoadMsg();// 씬을 다 옮겼다는 메세지
        Debug.Log(gameSystem);
        //Managers.Photon.Connect();

    }

    public override void Clear()
    {
        Debug.Log("Tutorial Scene Clear");

        PhotonNetwork.Destroy(GameObject.FindObjectOfType<GameSystem>().gameObject);
    }
}
