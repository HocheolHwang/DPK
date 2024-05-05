using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Scene : BaseScene
{

    protected override void Init()
    {
        base.Init();
        Managers.UI.ShowPopupUI<Dungeon_Popup_UI>("[Dungeon]_Dungeon_Popup_UI");
        Managers.Sound.Play("BackgroundMusic/Tutorial");

        MyPhoton mp = GameObject.FindObjectOfType<MyPhoton>();
        mp.SendLoadMsg();
        //Managers.Photon.Connect();

    }

    public override void Clear()
    {
        Debug.Log("Tutorial Scene Clear");
    }
}
