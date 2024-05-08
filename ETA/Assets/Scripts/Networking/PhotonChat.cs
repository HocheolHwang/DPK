using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System;

    // Serialization to send to Photon
    [System.Serializable]
    public class ChatMessage
    {
        public string sender;
        public string message;
    }

public class PhotonChat : MonoBehaviour
{
    public Chat_Popup_UI chatUI;
    PhotonView photonView;

    private void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
    }

    // 채팅창 활성화 되어 있을 시 메세지를 보내는 메서드
    // rpc로 send -> get message로 전달
    public void SendMessage(string msg)
    {
        if(photonView.Owner != PhotonNetwork.LocalPlayer)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
        }

        ChatMessage message = new ChatMessage();
        message.message = msg;
        message.sender = Managers.Player.GetNickName();


        // empty message
        if (message.message.Length < 1) return;

        message.sender = Managers.Player.GetNickName();


        string messageJson = JsonUtility.ToJson(message);

        if(PhotonNetwork.InRoom)
            photonView.RPC("ReceiveMessage", RpcTarget.All, messageJson);
        else
        {
            ReceiveMessage(messageJson);
        }
    }

    [PunRPC]
    public void ReceiveMessage(string message)
    {
        ChatMessage chatMessage = JsonUtility.FromJson<ChatMessage>(message);   
        if(chatUI!=null)
        chatUI.ReceiveMessage(chatMessage.sender, chatMessage.message);
    }

    
}
