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

    // 채팅 리스트
    Queue<ChatMessage> chatLog = new Queue<ChatMessage>();

    private void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
    }

    // 채팅창 활성화 되어 있을 시 메세지를 보내는 메서드
    // rpc로 send -> get message로 전달
    public void SendMessage(string msg)
    {
        // 메세지 유효성 검사
        if (string.IsNullOrEmpty(msg)) return;

        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }

        ChatMessage message = new ChatMessage()
        {
            message = msg,
            sender = Managers.Player.GetNickName()
        };

        // json 변환
        string messageJson = JsonUtility.ToJson(message);

        // 방에 접속되어 있으면 보내고 아니면 본인 채팅창에만 보여야함
        if (PhotonNetwork.InRoom)
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
        chatLog.Enqueue(chatMessage);

        if (chatLog.Count > 50)
        {
            chatLog.Dequeue();
        }

        if(chatUI!=null)
            chatUI.ReceiveMessage(chatMessage.sender, chatMessage.message);
    }

    public List<ChatMessage> GetChatMessage(){
        return new List<ChatMessage>( chatLog);
    }
}