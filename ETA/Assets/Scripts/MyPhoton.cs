using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPhoton : MonoBehaviour
{
    public PhotonView PhotonView;
    int loadCnt = 0;
    int characterCnt = 0;
    int finish = 0;
    PlayerController myController;
    // Start is called before the first frame update
    void Start()
    {
        PhotonView = GetComponent<PhotonView>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSceneAllPlayer(Define.Scene scene)
    {
        PhotonView.RPC("RPC_ChangeScene", RpcTarget.Others, scene);
    }

    [PunRPC]
    void RPC_ChangeScene(Define.Scene scene)
    {
        Managers.Scene.LoadScene(scene);
    }

    public void SendLoadMsg()
    {
        PhotonView.RPC("RPC_LoadedScene", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_LoadedScene()
    {
        loadCnt += 1;

        if(loadCnt >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            myController = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/Player", new Vector3(13.0f, 0.5f, 0f), Quaternion.Euler(0,90,0)).GetComponent<PlayerController>();
        }
    }

    public void SendCharacherInstantiatedMsg()
    {
        characterCnt += 1;
        if (characterCnt >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            //myController.ChangeState(myController.MOVE_STATE);
            //FindObjectOfType<PlayerZone>().Run();
            PhotonView.RPC("RPC_CharacherInstantiated", RpcTarget.All);
        }

    }

    [PunRPC]
    public void RPC_CharacherInstantiated()
    {
        finish += 1;
        if (finish >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            myController.ChangeState(myController.MOVE_STATE);
            FindObjectOfType<PlayerZone>().Run();
        }

    }

}
