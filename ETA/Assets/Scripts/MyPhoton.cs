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
            string classCode = Managers.Player.GetClassCode();
            if(classCode == "C001")
            {
                myController = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/Warrior", new Vector3(13.0f, 0.5f, 0f), Quaternion.Euler(0, 90, 0)).GetComponent<PlayerController>();
            }
            else if(classCode == "C002")
            {
                myController = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/Archer", new Vector3(13.0f, 0.5f, 0f), Quaternion.Euler(0, 90, 0)).GetComponent<PlayerController>();
            }
            else if(classCode == "C003")
            {
                myController = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/Wizard", new Vector3(13.0f, 0.5f, 0f), Quaternion.Euler(0, 90, 0)).GetComponent<PlayerController>();
            }
            else
            {
                Debug.LogError("존재하지 않는 클래스 입니다.");
            }

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
