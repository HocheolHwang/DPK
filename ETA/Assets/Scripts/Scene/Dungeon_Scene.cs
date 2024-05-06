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

        GameSystem mp = GameObject.FindObjectOfType<GameSystem>();
        mp.SendLoadMsg();

        Managers.Resource.Instantiate("MonsterManager").name = "@MonsterManager";
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


}
