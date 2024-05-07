using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SendRoomLog : MonoBehaviour
{
    PhotonView photonView;
     void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();

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
        PartyReqDto dto = new PartyReqDto();

        dto.partyId = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomID"];

        Managers.Network.EnterPartyCall(dto);
    }
}
