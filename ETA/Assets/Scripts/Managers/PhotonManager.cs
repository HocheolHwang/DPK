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
    private string dungeonIndex;

    // room list -use> update, print room
    List<RoomInfo> roomlist = new List<RoomInfo>();

    // current room
    private RoomInfo selectRoom;
    #endregion

    #region public fields
    public bool IsConnecting{
        set { isConnecting = value; }
        get { return isConnecting; } 
    }

    // 방 이름
    public string RoomName
    {
        set { roomName = value; }
        get {
            roomName = selectRoom.Name;
            int lastIndex = selectRoom.Name.LastIndexOf("`");
            if (lastIndex != -1)
                roomName = roomName.Substring(0, lastIndex);
            return roomName;
        }
    }

    // 현재 선택된 던전 번호
    public string DungeonIndex
    {
        set { 
            dungeonIndex = value;

            if (PhotonNetwork.InRoom)
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
        PhotonNetwork.AutomaticallySyncScene = true;
        //Connect();
    }
    void Start()
    {
    }

    #endregion


    // #region public Methods
    // 네트워크 연결을 위한 Connect 함수
    public void Connect()
    {
        // 임시 코드         
        //Managers.PlayerInfo.SetNickName(UnityEngine.Random.Range(0, 13412).ToString());
        Managers.Player.SetNickName(Managers.Player.GetNickName());
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

        if (partyLeader == null || roomName == null) return;

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

        // 로비에 Properties 등록해야 로비에서 설정 확인 가능
        room.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "partyLeader", partyLeader }, { "seed", seed }, { "roomID", guidString }, { "dungeonIndex", dungeonIndex } };
        room.CustomRoomPropertiesForLobby = new string[] { "partyLeader", "seed", "roomID", "dungeonIndex" };

        //string roomInfo =  "\n partyLeader : " + room.CustomRoomProperties["partyLeader"] + "\n dungeon : " + room.CustomRoomProperties["dungeonIndex"] + "\n" + room.CustomRoomProperties["ispassword"] + " / " + room.CustomRoomProperties["password"];
        //Debug.Log("makeeeeeeeeeeeeeeee : " + roomInfo);

        //Managers.Network.CreatePartyCall(partyData);
        Managers.Player.SetPartyLeader(true);

        // 방장여부 체크
        if (PhotonNetwork.IsMasterClient)
            Managers.Player.SetPartyLeader(true);

        PhotonNetwork.CreateRoom(roomName, room);
    }

    public void ClickRoom(int roomNumber)
    {
        selectRoom = roomlist[roomNumber];
    }

    // 현재 플레이어 직업 저장
    public void SetPlayerClass()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["CurClass"] == null)
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable() { { "CurClass", Managers.Player.GetClassCode() } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties["CurClass"] = Managers.Player.GetClassCode();
        }
    }

    public void roomEnter()
    {
        // 나중에 수정
        string nickname = Managers.Player.GetNickName();

        if (nickname == null) return;

        if (selectRoom.MaxPlayers <= selectRoom.PlayerCount)
        {
            // 방에 모든 사람이 들어가면 더 이상 들어갈 수 없음
            return;
        }

        PhotonNetwork.JoinRoom(selectRoom.Name);
    }

    // 테스트용
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void ExitRoom()
    {
        // 방이 아니면 탈퇴 불가
        if (!PhotonNetwork.InRoom) return;
        // room -> Lobby
        PhotonNetwork.LeaveRoom();
    }

    // 방 리스트
    public List<RoomInfo> printList()
    {
        int idx = 0;
        foreach (RoomInfo room in roomlist)
        {
            ExitGames.Client.Photon.Hashtable has = room.CustomProperties;

            // 마지막 ` 이후 시드값을 제외하고 출력
            string printRoomName = room.Name;
            int lastIndex = printRoomName.LastIndexOf("`");
            if (lastIndex != -1)
                printRoomName = printRoomName.Substring(0, lastIndex);
            idx++;
        }

        return roomlist;
    }

    // 던전 들어갈 때 부르기
    // 방장만 파티 생성
    public void SendRoomLog()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PartyReqDto dto = new PartyReqDto();

        dto.partyId = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomID"];
        dto.partyTitle = PhotonNetwork.CurrentRoom.Name;

        Managers.Network.CreatePartyCall(dto);
    }

    // 던전 들어갈 때 부르기
    // 모두 파티에 등록
    public void SendRoomEnterLog()
    {
        PartyReqDto dto = new PartyReqDto();

        dto.partyId = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomID"];

        Managers.Network.EnterPartyCall(dto);
    }

    // 파티 구성이 바뀌면 내번호도 바뀔 수 있음
    public void updatePlayerList()
    {  
        int idx = 0;
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            // Debug.Log(idx + " : " + player.NickName);
            if (player.IsLocal)
            {
                Managers.Player.SetIndex(idx);
            }
            idx++;
        }
    }
    // #endregion

    #region MonoBehaviourPunCallbacks callbacks
    public override void OnConnectedToMaster()
    {
        isConnecting = true;
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // 방장 해제
        Managers.Player.SetPartyLeader(false);
    }

    // 방 정보 저장
    public override void OnRoomListUpdate(List<RoomInfo> newRooms)
    {
        foreach (RoomInfo rooom in newRooms)
        {
            // 리스트에 방이 있는지 없는지 판단
            bool change = false;
            for (int i = 0; i < roomlist.Count; i++)
            {
                // 지금 보고있는 방이 리스트에 있다.
                if (roomlist[i].Name == rooom.Name)
                {
                    // 풀방 아님
                    if (rooom.PlayerCount != 0)
                        roomlist[i] = rooom;    
                    // no player, no open, no multy
                    else if (rooom.PlayerCount == 0 || !rooom.IsOpen || !rooom.IsVisible)
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
                if (rooom.PlayerCount != 0)
                    roomlist.Add(rooom);
            }
        }

        // 방 출력
        printList();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
        // 파티 다 찼을 때 경고 줘야함
    }

    public override void OnJoinedRoom()
    {
        // 방에 들어오자마자 닉네임 설정
        if (PhotonNetwork.NickName == null)
            PhotonNetwork.NickName = Managers.Player.GetNickName();
        
        updatePlayerList();
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
