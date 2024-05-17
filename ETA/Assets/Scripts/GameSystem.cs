using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviourPunCallbacks
{
    public PhotonView PhotonView;
    int _loadedCnt = 0;
    int _createdCharacterCnt = 0;
    int _finishedClient = 0;
    public int LoadedSkillCnt = 0;
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

    public void SceneLoaded()
    {
        PhotonView.RPC("RPC_SceneLoaded", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_SceneLoaded()
    {
        // load된 사용자의 수
        _loadedCnt += 1;

        // load된 사용자 수가 방 인원만큼되면
        if(_loadedCnt >= PhotonNetwork.CurrentRoom.PlayerCount)
        {

            // 게임을 시작하려고 합니다.
            // 본인의 직업에 맞춰서 캐릭터를 생성합니다.
            // 캐릭터를 생성하면 그것도 신호를 보내줘야함

            InstantiateMyCharacter();
        }
    }
    void InstantiateMyCharacter()
    {
        string classCode = Managers.Player.GetClassCode();
        if (classCode == "C001")
        {
            myController = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/Warrior", new Vector3(13.0f, 0.5f, 0f), Quaternion.Euler(0, 90, 0)).GetComponent<PlayerController>();
        }
        else if (classCode == "C002")
        {
            myController = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/Archer", new Vector3(13.0f, 0.5f, 0f), Quaternion.Euler(0, 90, 0)).GetComponent<PlayerController>();
        }
        else if (classCode == "C003")
        {
            myController = PhotonNetwork.Instantiate("Prefabs/Creatures/Player/Mage", new Vector3(13.0f, 0.5f, 0f), Quaternion.Euler(0, 90, 0)).GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("존재하지 않는 클래스 입니다.");
        }
    }

    public void SendCharacherInstantiatedMsg()
    {
        // 캐릭터가 생성 될때 마다 카운트한다.
        _createdCharacterCnt += 1;

        // 내 화면에 모든 사용자의 캐릭터가 다 만들어지면
        if (_createdCharacterCnt >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            // 다 완성되었다고 메세지를 보낸다.
            PhotonView.RPC("RPC_AllCharachersInstantiated", RpcTarget.All);
        }

    }

    [PunRPC]
    public void RPC_AllCharachersInstantiated()
    {
        // 캐릭터들이 다 로드된 클라이언트 갯수
        _finishedClient += 1;
        if (_finishedClient >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            myController.ChangeState(myController.MOVE_STATE);
            myController.SkillSlot.SendSkillsInfo();
            FindObjectOfType<PlayerZone>().Run();
            FindObjectOfType<Dungeon_Popup_UI>().SetMembersInfo();
            CreateParty();
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
        _loadedCnt = 0;
        _createdCharacterCnt = 0;
        _finishedClient = 0;
        myController = null;
    }
    public void CreateParty()
    {
        // 파티 생성 요청 전송
        Managers.Photon.SendRoomLog(PartyEnter);
    }
    public void PartyEnter()
    {
        photonView.RPC("SendRoomEnterLog", RpcTarget.All);
    }

    // 던전 들어갈 때 부르기
    // 모두 파티에 등록
    [PunRPC]
    public void SendRoomEnterLog()
    {
        DungeonReqDto dto = new DungeonReqDto();

        dto.partyId = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomID"];

        Managers.Network.EnterPartyCall(dto);

    }
}
