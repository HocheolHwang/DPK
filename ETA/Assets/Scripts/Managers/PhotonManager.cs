using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region private fields
    private string gameVersion = "1";
    private byte maxplayersPerRoom = 3;
    private bool isConnecting;
    private string roomName;


    // 던전 이름
    private int dungeonIndex;

    // room list -use> update, print room
    public List<RoomInfo> roomlist = new List<RoomInfo>();

    // current room
    public RoomInfo selectRoom;


    #endregion

    #region public fields
    public bool IsConnecting {
        set { isConnecting = value; }
        get { return isConnecting; }
    }

    // 방 이름
    public string RoomName
    {
        set { roomName = value; }
        get {


            return ConvertRoomName(roomName);
        }
    }

    // 현재 선택된 던전 번호
    public int DungeonIndex
    {
        set {
            if (dungeonIndex == value) return;
            dungeonIndex = value;

            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                ExitGames.Client.Photon.Hashtable curProperties = PhotonNetwork.CurrentRoom.CustomProperties;
                // 던전 세팅
                curProperties["dungeonIndex"] = dungeonIndex;
                PhotonNetwork.CurrentRoom.SetCustomProperties(curProperties);
            }
        }
        get
        {
            return dungeonIndex;
        }
    }

    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        //Connect();
    }
    void Start()
    {
        //MannequinController[] treeMannequin = GameObject.FindObjectsOfType<MannequinController>();

        //Debug.Log(treeMannequin[0].index);

        //for(int i = 0; i<3; i++)
        //  mannequins[treeMannequin[i].index] = treeMannequin[i];


        //mannequins[0].EnterPlayer("0", "C001");
        //mannequins[1].EnterPlayer("1", "C002");
        //mannequins[2].EnterPlayer("2", "C003");
    }



    #endregion


    // #region public Methods
    // 네트워크 연결을 위한 Connect 함수
    public void Connect()
    {
        //PhotonNetwork.SerializationRate = 10;
        //PhotonNetwork.PrecisionForFloatSynchronization = 0.1f;
        //PhotonNetwork.SendRate = 60;
        // 임시 코드         
        if (Managers.Player.GetNickName() == null)
            Managers.Player.SetNickName(UnityEngine.Random.Range(0, 13412).ToString());
        //Managers.Player.SetNickName(Managers.Player.GetNickName());
        //Debug.Log(Managers.PlayerInfo.GetNickName());

        if (isConnecting)
        {
            // 이미 연결된 경우
            Debug.Log("PhotonNetwork: 이미 연결되어 있습니다.");
        }
        else
        {
            // setting
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.NickName = Managers.Player.GetNickName();

            // 포톤 연결 서버를 kr로 고정하여 한국 서버에만 연결되도록 설정
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "kr";

            // start connect
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    // Make multy Room
    public void MakeRoom(string newRoomName)
    {
        // 나중에 수정할 것
        roomName = newRoomName; // "RoomName";
        string partyLeader = Managers.Player.GetNickName();

        if (roomName.Length == 0) return;

        if (partyLeader == null)
            partyLeader = "방장";
        if (roomName == null)
            roomName = "파티";
        //if (partyLeader == null || roomName == null) return;

        RoomOptions room = new RoomOptions();
        room.MaxPlayers = maxplayersPerRoom;
        room.IsVisible = true;
        room.IsOpen = true;
        room.CleanupCacheOnLeave = false;

        // 방 ID 생성
        // 새로운 유니크 아이디(UID)를 생성하고 이를 문자열로 변환
        Guid newGuid = Guid.NewGuid();  // 새 GUID 생성
        string guidString = newGuid.ToString();  // GUID를 문자열로 변환

        // 전송 데이터 생성
        PartyReqDto partyData = new PartyReqDto();
        partyData.partyId = guidString;
        partyData.partyTitle = roomName;

        // 시드 생성
        int seed = (int)System.DateTime.Now.Ticks;
        // 생성된 방 이름 + ` + 시드 값
        roomName = roomName + "`" + seed;
        //dungeonIndex = PlayerPrefs.GetInt("SelectedDungeonNumber", 1); 
        if (FindObjectOfType<Lobby_Scene>() != null)
        {
            dungeonIndex = FindObjectOfType<Lobby_Scene>().currentDungeonNumber;
        }
        else
        {
            dungeonIndex = 0;
        }

        // 로비에 Properties 등록해야 로비에서 설정 확인 가능
        room.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "partyLeader", partyLeader }, { "seed", seed }, { "roomID", guidString }, { "dungeonIndex", DungeonIndex } };
        room.CustomRoomPropertiesForLobby = new string[] { "partyLeader", "seed", "roomID", "dungeonIndex" };

        //string roomInfo =  "\n partyLeader : " + room.CustomRoomProperties["partyLeader"] + "\n dungeon : " + room.CustomRoomProperties["dungeonIndex"] + "\n" + room.CustomRoomProperties["ispassword"] + " / " + room.CustomRoomProperties["password"];
        //Debug.Log("makeeeeeeeeeeeeeeee : " + roomInfo);

        //Managers.Network.CreatePartyCall(partyData);
        // 방장 체크
        Managers.Player.SetPartyLeader(true);

        try
        {
            // 파티 생성
            PhotonNetwork.CreateRoom(roomName, room);
        }
        catch (RoomCreationException ex)
        {
                Debug.Log("여기서 exceptin 처리할수있냐:????");
            var popupUI = GameObject.FindFirstObjectByType<Lobby_Popup_UI>();
            if (popupUI != null)
            {
                Managers.Coroutine.StartCoroutine(popupUI.ShowWarningPopupCoroutine(ex.Title, ex.Message));
            }
            else
            {
                Debug.LogError("Lobby_Popup_UI is not found in the scene.");
            }
        }
    }

    public string ConvertRoomName(string newRoomName)
    {
        int lastIndex = newRoomName.LastIndexOf("`");

        if (lastIndex != -1)
            newRoomName = newRoomName.Substring(0, lastIndex);
        return newRoomName;
    }

    public void SendDungeonEnd(string ClearTimeText, bool cleared)
    {
        // 보내야하는 시간은 int
        string[] splitTime = ClearTimeText.Split(":");

        int minute = int.Parse(splitTime[0]);
        int second = int.Parse(splitTime[1]);
        int clearTime = (minute * 60) + second;
        Debug.Log($"{minute}분 {second} 초 : {(minute * 60) + second}");

        if (!PhotonNetwork.IsMasterClient) return;
        DungeonReqDto dto = new DungeonReqDto();
        dto.partyId = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomID"];
        dto.dungeonCode = "D00" + DungeonIndex;
        dto.cleared = cleared;
        dto.clearTime = clearTime;
        Managers.Network.EndDungeonCall(dto);

        UpdateUUID();
    }



    // 새로운 UUID 업데이트
    public void UpdateUUID()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 새로운 유니크 아이디(UID)를 생성하고 이를 문자열로 변환
            Guid newGuid = Guid.NewGuid();  // 새 GUID 생성
            string guidString = newGuid.ToString();  // GUID를 문자열로 변환
            ExitGames.Client.Photon.Hashtable curProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            curProperties["roomID"] = guidString;
            PhotonNetwork.CurrentRoom.SetCustomProperties(curProperties);
        }
    }

    public void ClickRoom(int roomNumber)
    {
        selectRoom = roomlist[roomNumber];
    }

    // 현재 플레이어 직업 저장
    public void SetPlayerClass()
    {
        Debug.Log($"현재 직업은 {Managers.Player.GetClassCode()} 입니다. 현재 레벨은 {Managers.Player.GetLevel()} 입니다.");
        ExitGames.Client.Photon.Hashtable currentProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        currentProperties["CurClass"] = Managers.Player.GetClassCode();
        currentProperties["PlayerLevel"] = Managers.Player.GetLevel();
        //ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable() { { "CurClass", Managers.Player.GetClassCode() }, { "PlayerLevel ", Managers.Player.GetLevel() } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(currentProperties);
    }
    // 현재 플레이어 레벨 저장
    public void SetPlayerLevel()
    {
        Debug.Log($"현재 레벨은 {Managers.Player.GetLevel()} 입니다.");
        ExitGames.Client.Photon.Hashtable currentProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        currentProperties["PlayerLevel"] = Managers.Player.GetLevel();

        PhotonNetwork.LocalPlayer.SetCustomProperties(currentProperties);
    }

    public void roomEnter()
    {
        // 나중에 수정
        string nickname = Managers.Player.GetNickName();

        if (nickname == null) return;

        foreach (RoomInfo newRoom in roomlist)
        {
            Debug.Log(roomName);
            Debug.Log(newRoom.Name);
            string newRoomName = ConvertRoomName(newRoom.Name);
            if (newRoomName == roomName)
            {
                selectRoom = newRoom;
                break;
            }
        }

        if(selectRoom == null)
        {
            throw new RoomCreationException("파티에 입장 할 수 없습니다.", "방이 존재하지 않습니다.");
        }

        if (selectRoom.MaxPlayers <= selectRoom.PlayerCount)
        {
            // 방에 모든 사람이 들어가면 더 이상 들어갈 수 없음
            throw new RoomCreationException("파티에 입장 할 수 없습니다.", "방이 다 찼습니다.");
            //return;
        }

        try
        {

            PhotonNetwork.JoinRoom(selectRoom.Name);
        }
        catch(RoomCreationException ex)
        {
            Debug.Log("아니 여기는 들어오냐????");
        }
    }

    // 테스트용
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    // 방 리스트
    public List<RoomInfo> printList()
    {
        int idx = 0;
        foreach (RoomInfo room in roomlist)
        {
            ExitGames.Client.Photon.Hashtable has = room.CustomProperties;

            // 마지막 ` 이후 시드값을 제외하고 출력
            string printRoomName = ConvertRoomName(room.Name);
            idx++;
        }

        return roomlist;
    }

    // 던전 들어갈 때 부르기
    // 방장만 파티 생성
    public void SendRoomLog(Action callback)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PartyReqDto dto = new PartyReqDto();

        dto.partyId = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomID"];
        dto.partyTitle = RoomName;

        Debug.Log("파티 생성");
        Managers.Network.CreatePartyCall(dto, callback);
    }

    // 파티 구성이 바뀌면 내번호도 바뀔 수 있음
    public void updatePlayerList()
    {
        int idx = 0;
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsLocal)
            {
                Managers.Player.SetIndex(idx);

                ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;
                if (properties["PlayerIndex"] == null)
                {
                    properties.Add("PlayerIndex", idx);
                }
                else
                    properties["PlayerIndex"] = idx;

                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }
            idx++;
        }
    }

    public void CloseRoom()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void OpenRoom()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;
    }
    // #endregion

    #region MonoBehaviourPunCallbacks callbacks

    public override void OnLeftRoom()
    {
        Managers.Player.SetPartyLeader(false);
        Destroy(GameObject.FindObjectOfType<GameSystem>().gameObject);
    }
    public override void OnConnectedToMaster()
    {
        isConnecting = true;
        Debug.Log("1. OnConnectedToMaster : 자동으로 로비로 입장");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("02. Joined Lobby");
        // 방장 해제
        Managers.Player.SetPartyLeader(false);
    }

    // 방 정보 저장
    public override void OnRoomListUpdate(List<RoomInfo> newRooms)
    {
        foreach (RoomInfo room in newRooms)
        {
            // 리스트에 방이 있는지 없는지 판단
            bool change = false;
            for (int i = 0; i < roomlist.Count; i++)
            {
                // 지금 보고있는 방이 리스트에 있다.
                if (roomlist[i].Name == room.Name)
                {
                    // 풀방 아님
                    if (room.PlayerCount != 0)
                        roomlist[i] = room;
                    // no player, no open, no multy
                    else if (room.PlayerCount == 0 || !room.IsOpen || !room.IsVisible)
                    {
                        roomlist.Remove(roomlist[i]);
                    }
                    change = true;
                    break;
                }
            }

            // 바꿀게 없다
            if (!change)
            {
                if (room.PlayerCount != 0)
                    roomlist.Add(room);
            }
        }

        // 방 출력
        printList();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError(returnCode + " : " + message);
        // 파티 다 찼을 때 경고 줘야함

        string title = "파티 만들기 실패";
        string content = "파티 생성에 실패했습니다.";

        throw new RoomCreationException(title, content);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError(returnCode + " : " + message);
        // 파티 다 찼을 때 경고 줘야함

        string title = "파티에 입장 할 수 없습니다.";
        string content = "파티 입장에 실패했습니다.";
        if (returnCode == 32758)
            content = "방이 존재하지 않습니다.";
        else if (returnCode == 32764)
            content = "방이 다 찼습니다.";
        else if (returnCode == 32765)
            content = "방이 닫혔습니다.";
        else if (returnCode == 32767)
            content = "유효하지 않은 방입니다.";
        else if (returnCode == 32756)
            content = "서버에 접속할 수 없습니다.";

        throw new RoomCreationException(title, content);

    }

    public override void OnJoinedRoom()
    {
        // 방에 들어오자마자 닉네임 설정
        // TODO: 문제가 생기는지 확인
        //if (PhotonNetwork.NickName == null)
        //    PhotonNetwork.NickName = Managers.Player.GetNickName();

        //if (PhotonNetwork.IsMasterClient) { 
        //    Managers.Player.SetPartyLeader(true);
        //    GameObject gameSystem = PhotonNetwork.Instantiate("Prefabs/GameSystem", new Vector3(), new Quaternion());
        //    gameSystem.name = "GameSystem";
        //    DontDestroyOnLoad(gameSystem);
        //}
        //updatePlayerList();
        //SetPlayerClass();



        //Debug.Log("방 입장");
        //Debug.Log($"방 이름 : {PhotonNetwork.CurrentRoom.Name} \n 파티장 : {(string)PhotonNetwork.CurrentRoom.CustomProperties["partyLeader"]} " +
        //$"\n 방 id : {(string)PhotonNetwork.CurrentRoom.CustomProperties["roomID"]} \n 던전 id : {(int)PhotonNetwork.CurrentRoom.CustomProperties["dungeonIndex"]} ");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // 방 나간사람이 있는 경우 번호 갱신
        updatePlayerList();
    }

    // 마스터 클라이언트가 변경 되었을 때 호출
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable curProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            // 방장 변경
            Managers.Player.SetPartyLeader(true);
            curProperties["partyLeader"] = Managers.Player.GetNickName();
            PhotonNetwork.CurrentRoom.SetCustomProperties(curProperties);
        }

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        updatePlayerList();
    }
    #endregion
}
