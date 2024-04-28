using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhotonChat : MonoBehaviour
{
    // Serialization to send to Photon
    [System.Serializable]
    public class ChatMessage
    {
        public string sender;
        public string message;
    }

    // 채팅창 활성화 되어 있을 시 메세지를 보내는 메서드
    // rpc로 send -> get message로 전달
    public void SendMessage()
    {
        ChatMessage message = new ChatMessage();


        // empty message
        if (message.message.Length < 1) return;

        message.sender = Managers.PlayerInfo.GetNickName();

        Debug.Log("adiasjdiojaiopd " + PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Number"));

        string messageJson = JsonUtility.ToJson(message);
        //view.RPC("ReceiveMessage", RpcTarget.All, messageJson);
    }

    [PunRPC]
    public void ReceiveMessage(string message)
    {
        ChatMessage chatMessage = JsonUtility.FromJson<ChatMessage>(message);

        //GameObject chatPrefab = Instantiate(ChatItem[chatMessage.index]);

        //chatPrefab.transform.SetParent(chatContainer.transform, false);
    }
}
