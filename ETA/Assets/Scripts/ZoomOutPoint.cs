using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOutPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {

        PhotonView player = other.GetComponent<PhotonView>();

        if (player != null)
        {
            if(player.IsMine) Camera.main.GetComponent<CameraController>().ZoomOut();

        }
        
    }
}
