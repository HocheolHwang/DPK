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
        SceneType = Define.Scene.Tutorial;

        gameSystem = GameObject.FindObjectOfType<GameSystem>();
        gameSystem.SceneLoaded();

        Managers.UI.ShowPopupUI<Dungeon_Popup_UI>("[Dungeon]_Dungeon_Popup_UI");
        Managers.Sound.Play("BackgroundMusic/Tutorial", Define.Sound.BGM);

    }

    public override void Clear()
    {
        Debug.Log("<color=red>Tutorial Scene Clear</color>");
    }
}
