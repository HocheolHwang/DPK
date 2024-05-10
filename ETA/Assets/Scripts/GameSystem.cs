using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviourPunCallbacks
{
    public PhotonView PhotonView;
    int loadCnt = 0;
    int characterCnt = 0;
    int finish = 0;
    public int currentDungeonNum;
    PlayerController myController;
    // Start is called before the first frame update
    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }
    void Start()
    {
        
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSceneAllPlayer(Define.Scene scene)
    {
        int num = 0;
        switch (scene)
        {
            case Define.Scene.StarShardPlain:
                num = 1;
                break;
            case Define.Scene.ForgottenTemple:
                num = 2;
                break;
            case Define.Scene.SeaOfAbyss:
                num = 3;
                break;
        }

        currentDungeonNum = num;
        GetComponent<PhotonView>().RPC("RPC_ChangeScene", RpcTarget.Others, scene);
    }

    [PunRPC]
    void RPC_ChangeScene(Define.Scene scene)
    {
        int num = 0;
        switch (scene)
        {
            case Define.Scene.StarShardPlain:
                num = 1;
                break;
            case Define.Scene.ForgottenTemple:
                num = 2;
                break;
            case Define.Scene.SeaOfAbyss:
                num = 3;
                break;
        }

        currentDungeonNum = num;
        Managers.Scene.LoadScene(scene);
    }

    public void SendLoadMsg()
    {
        PhotonView.RPC("RPC_LoadedScene", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_LoadedScene()
    {
        // load된 사용자의 수
        loadCnt += 1;

        // load된 사용자 수가 방 인원만큼되면
        if(loadCnt >= PhotonNetwork.CurrentRoom.PlayerCount)
        {

            // 게임을 시작하려고 합니다.
            // 본인의 직업에 맞춰서 캐릭터를 생성합니다.
            // 캐릭터를 생성하면 그것도 신호를 보내줘야함

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
                myController = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/Mage", new Vector3(13.0f, 0.5f, 0f), Quaternion.Euler(0, 90, 0)).GetComponent<PlayerController>();
            }
            else
            {
                Debug.LogError("존재하지 않는 클래스 입니다.");
            }

            

        }
    }

    public void SendCharacherInstantiatedMsg()
    {
        // 이게 내 화면에 캐릭터들이 다 완성되면 보냄
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
        // 캐릭터들이 다 로드된 클라이언트 갯수
        finish += 1;
        if (finish >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            myController.ChangeState(myController.MOVE_STATE);
            FindObjectOfType<PlayerZone>().Run();

            FindObjectOfType<Dungeon_Popup_UI>().SetMembersInfo();


            //var cons = FindObjectsOfType<PlayerController>();
            //foreach(var c in cons)
            //{
            //    Managers.UI.MakeWorldSpaceUI<UI_CharacterNickName>(c.gameObject.transform);

            //}
            
        }

    }

    public void SendCurrentDungeon(int num)
    {
        GetComponent<PhotonView>().RPC("RPC_SetCurrentDungeon", RpcTarget.Others, num);
    }
    [PunRPC]
    public void RPC_SetCurrentDungeon(int num)
    {
        FindObjectOfType<Lobby_Scene>().currentDungeonNumber = num;
        FindObjectOfType<Lobby_Popup_UI>().UpdateSelectedDungeon();
    }

    // 어차피 로비 오면 삭제할거 같은데 의미있나?
    public void Clear()
    {
        loadCnt = 0;
        characterCnt = 0;
        finish = 0;
        myController = null;
    }

    


}
