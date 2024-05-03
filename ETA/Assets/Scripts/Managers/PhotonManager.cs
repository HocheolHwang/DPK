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
        PhotonNetwork.AutomaticallySyncScene = false;
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
    public void MakeRoom()
    {
        // 나중에 수정할 것
        roomName = "RoomName";
        string partyLeader = Managers.Player.GetNickName();

        if (roomName.Length == 0) return;

        if (partyLeader == null)
            partyLeader = "초이";

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

        string roomInfo =  "\n partyLeader : " + room.CustomRoomProperties["partyLeader"] + "\n dungeon : " + room.CustomRoomProperties["dungeonIndex"] + "\n" + room.CustomRoomProperties["ispassword"] + " / " + room.CustomRoomProperties["password"];

        Debug.Log("makeeeeeeeeeeeeeeee : " + roomInfo);

        //Managers.Network.CreatePartyCall(partyData);
        Managers.Player.SetPartyLeader(true);
        PhotonNetwork.CreateRoom(roomName, room);
    }

    public void ClickRoom(int roomNumber)
    {
        selectRoom = roomlist[roomNumber];
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

        PhotonNetwork.JoinRandomRoom();
    }

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

            //string roomInfo = "room : " + room.Name + " \n" + room.PlayerCount + " / " + room.MaxPlayers + "\n" + "isvisible : " + room.IsVisible + "\n" + "isopen : " + room.IsOpen
            //    + "\n partyLeader : " + has["partyLeader"] + "\n dungeon : " + has["dungeonIndex"] + "\n" + has["ispassword"] + " / " + has["password"];

            //Debug.Log(roomInfo);
        }

        return roomlist;
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

        // 테스트코드
        // PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedLobby()
    {
        // 로비에 들어오자마자 닉네임 설정
        if(PhotonNetwork.NickName == null)
            PhotonNetwork.NickName = Managers.Player.GetNickName();
        // 방장 해제
        Managers.Player.SetPartyLeader(false);

        // TMP
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> newRooms)
    {
        foreach (RoomInfo rooom in newRooms)
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
        updatePlayerList();
        if (PhotonNetwork.IsMasterClient) { 
            Managers.Player.SetPartyLeader(true);
            GameObject MyPhoton = PhotonNetwork.Instantiate("Prefabs/MyPhoton", new Vector3(), new Quaternion());
            MyPhoton.name = "MyPhoton";
            DontDestroyOnLoad(MyPhoton);
        }




    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //        if(otherPlayer.IsMasterClient)
        updatePlayerList();
        // 만약 나간 플레이어가 방장인 경우
         
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        // 마스터 클라이언트가 변경 되었을 때 호출

        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable curProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            // 방장 변경
            Managers.Player.SetPartyLeader(true);
            curProperties["partyLeader"] = Managers.Player.GetNickName();
            PhotonNetwork.CurrentRoom.SetCustomProperties(curProperties);

            //Managers.Player.RemovePlayer(otherPlayer);
            //Managers.Player.LoadPlayersInfoInCurrentRoom();

            //PlayerController[] list = GameObject.FindObjectsOfType<PlayerController>();
            //object viewIDObj;
            //otherPlayer.CustomProperties.TryGetValue("ViewID", out viewIDObj);

            //foreach (PlayerController p in list)
            //{

            //    if (p.GetComponent<PhotonView>().ViewID == (int)viewIDObj)
            //    {
            //        // 마스터가 지워라.
            //        if (PhotonNetwork.IsMasterClient) PhotonNetwork.Destroy(p.gameObject);
            //    }
            //}
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        updatePlayerList();
    }
    #endregion

}
