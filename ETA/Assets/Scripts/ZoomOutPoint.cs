using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOutPoint : MonoBehaviour
{
    private Dungeon_Popup_UI dungeonPopupUI;

    private void Update()
    {
        dungeonPopupUI?.UpdateDragonHP();
    }

    void OnTriggerEnter(Collider other)
    {

        PhotonView player = other.GetComponent<PhotonView>();

        if (player != null)
        {
            if(player.IsMine) Camera.main.GetComponent<CameraController>().ZoomOut();

            dungeonPopupUI = FindObjectOfType<Dungeon_Popup_UI>();
            if (dungeonPopupUI == null) return;

            dungeonPopupUI.UpdateDragonStatus();
        }
    }


}
