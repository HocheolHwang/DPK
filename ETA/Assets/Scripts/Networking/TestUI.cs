using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestUI : MonoBehaviour
{
    public GameObject ShowObj;
    public GameObject partyLeader;
    NetworkManager req;

    private void Start()
    {
        //req = gameObject.GetComponent<NetworkManager>();
        req = Managers.Network;
    }

    private void Update()
    {
        Show();
        ShowPartyLeader();
    }
    public void Post()
    {
        ResponseDto test = new ResponseDto();

        test.gold = 124812048;
        test.id = "Tkvl";
        test.nickName = "이싸피";

        string testData = JsonUtility.ToJson(test);

        req.HTTPCall("POST", "post", testData);
    }

    public void Put()
    {
        ResponseDto test = new ResponseDto();

        test.gold = 124812048;
        test.id = "ssafy";
        test.nickName = "김싸피";

        string testData = JsonUtility.ToJson(test);

        req.HTTPCall("PUT", "put", testData);
    }

    public void Get()
    {

        req.HTTPCall("GET", "get");
    }

    public void MakeRoom()
    {
        //PhotonManager photon = gameObject.GetComponent<PhotonManager>();

        Managers.Photon.MakeRoom();
    }
    public void JoinRoom()
    {
        Managers.Photon.JoinRandomRoom();
    }

    public void Show() {
        ShowObj.SetActive(PhotonNetwork.InRoom);
    }
    public void ExitRoom() {
        Managers.Photon.ExitRoom();
    }

    public void ShowPartyLeader()
    {
        partyLeader.SetActive(Managers.PlayerInfo.GetPartyLeader());
    }

    //// 테스트 로그인
    //public void signIn()
    //{
    //    PlayerSignInReqDto player = new PlayerSignInReqDto();
    //    player.playerId = "helloworld";
    //    player.playerPassword = "qwe123123";

    //    //req.SignInCall(player);
    //    Managers.Network.SignInCall(player);

    //    //PartyReqDto party = new PartyReqDto();
    //    //party.partyId = "123445678";
    //    //party.partyTitle = "암ㄴ애ㅏㅇ";
    //    //req.CreatePartyCall(party);
    //}

    //// 테스트 회원가입
    //public void signUp()
    //{
    //    PlayerSignUpReqDto player = new PlayerSignUpReqDto();
    //    player.playerId = "helloworld";
    //    player.nickname = "뉴월드입니다";
    //    player.playerPassword = "qwe123123";
    //    player.playerPasswordCheck = "qwe123123";

    //    req.SignUpCall(player);
    //}
}
