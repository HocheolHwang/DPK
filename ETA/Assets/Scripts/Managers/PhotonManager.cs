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

    

    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }
    void Start()
    {
    }

    #endregion


    // #region public Methods
    // 네트워크 연결을 위한 Connect 함수
    public void Connect()
    {
        if (isConnecting)
        {
            // 이미 연결된 경우
            Debug.Log("PhotonNetwork: 이미 연결되어 있습니다.");
        }
        else
        {
            // setting
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.NickName = "Player";

            // 포톤 연결 서버를 kr로 고정하여 한국 서버에만 연결되도록 설정
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "kr";

            // start connect
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    // Make multy Room
    public void MakeRoom()
    {
        // 나중에 수정할 것
        roomName = "RoomName";
        string captainName = "captain";

        if (roomName.Length == 0) return;

        if (captainName == null)
            captainName = "player";

        if (captainName == null || roomName == null) return;

        RoomOptions room = new RoomOptions();
        room.MaxPlayers = maxplayersPerRoom;
        room.IsVisible = true;
        room.IsOpen = true;
        room.CleanupCacheOnLeave = false;

        // 방 ID 생성
        // 새로운 유니크 아이디(UID)를 생성하고 이를 문자열로 변환
        Guid newGuid = Guid.NewGuid();  // 새 GUID 생성
        string guidString = newGuid.ToString();  // GUID를 문자열로 변환
        
        PartyReqDto partyData = new PartyReqDto();
        partyData.partyId = guidString;
        partyData.partyTitle = roomName;

        // 시드 생성
        int seed = (int)System.DateTime.Now.Ticks;
        // 생성된 방 이름 + ` + 시드 값
        roomName = roomName + "`" + seed;

        // Register in lobby
        room.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "captain", captainName }, { "seed", seed }, { "roomID", guidString } };
        room.CustomRoomPropertiesForLobby = new string[] { "captain", "seed", "roomID" };
        
        Managers.Network.CreatePartyCall(partyData);

        PhotonNetwork.CreateRoom(roomName, room);
    }

    public void ClickRoom(int roomNumber)
    {
        selectRoom = roomlist[roomNumber];
    }

    public void roomEnter()
    {
        // 나중에 수정
        string nickname = "Player";//UserInfo.GetInstance().getNickName();

        if (nickname == null) return;

        if (selectRoom.MaxPlayers <= selectRoom.PlayerCount)
        {
            // 방에 모든 사람이 들어가면 더 이상 들어갈 수 없음
            return;
        }

        PhotonNetwork.JoinRoom(selectRoom.Name);

        PhotonNetwork.JoinRandomRoom();
    }

    public void ExitRoom()
    {
        // 방이 아니면 탈퇴 불가
        if (!PhotonNetwork.InRoom) return;
        // room -> Lobby
        PhotonNetwork.LeaveRoom();
    }

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

            //string roomInfo = "room : " + room.Value.Name + " \n" + room.Value.PlayerCount + " / " + room.Value.MaxPlayers + "\n" + "isvisible : " + room.Value.IsVisible + "\n" + "isopen : " + room.Value.IsOpen
            //    + "\n captain : " + has["captain"] + "\n" + has["ispassword"] + " / " + has["password"];
        }

        return roomlist;
    }

    public void PrintPlayer()
    {

    }

    // 던전 들어갈 때 부르기
    // 방장만
    public void SendRoomLog()
    {
        PartyReqDto dto = new PartyReqDto();

        dto.partyId = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomID"];
        dto.partyTitle = PhotonNetwork.CurrentRoom.Name;

        Managers.Network.CreatePartyCall(dto);
    }

    // 던전 들어갈 때 부르기
    public void SendRoomEnterLog()
    {
        PartyReqDto dto = new PartyReqDto();

        dto.partyId = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomID"];

        Managers.Network.EnterPartyCall(dto);
    }

    // #endregion

    #region MonoBehaviourPunCallbacks callbacks
    public override void OnConnectedToMaster()
    {
        isConnecting = true;
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo rooom in roomList)
        {
            bool change = false;
            for (int i = 0; i < roomlist.Count; i++)
            {
                if (roomlist[i].Name == rooom.Name)
                {
                    if (rooom.PlayerCount != 0)
                        roomlist[i] = rooom;    
                    // no player, no open, no multy
                    else if (rooom.PlayerCount == 0 || !rooom.IsOpen || !rooom.IsVisible)
                    {
                        roomlist.Remove(roomlist[i]);
                    }
                    change = true;
                }
            }

            if (!change)
            {
                if (rooom.PlayerCount != 0)
                    roomlist.Add(rooom);
            }
        }
        printList();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
        // 파티 다 찼을 때 경고 줘야함
    }

    public override void OnJoinedRoom()
    {
        // 로컬 플레이어의 캐릭터를 생성하고 Photon 네트워크에 등록
        //GameObject player = PhotonNetwork.Instantiate("Prefabs/Player/Player", Vector3.zero, Quaternion.identity);

    }

    

    #endregion

}
