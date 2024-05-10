using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login_Scene : BaseScene
{
    private void Start()
    {
        Managers.UI.ShowPopupUI<Before_Login_Popup_UI>("[Login]_Before_Login_Popup_UI");
        Managers.UI.ShowPopupUI<Login_Popup_UI>("[Login]_Login_Popup_UI");
        Managers.Sound.Play("BackgroundMusic/Login");
    }

    public override void Clear()
    {
        Debug.Log("Login Scene Clear");
    }

    public override void OnJoinedLobby()
    {
        // 로그인 후 성공적으로 포톤(서버)에 Connection후
        // 로비 입장시 UI가 뜬다.


        // 첫 로그인 일 경우
        if (Managers.Player.GetFirst())
        {
            // 방을 만들어줘 줘야함
            // 왜냐하면, TutorialScene으로 넘어갈거기 때문에
            Guid newGuid = Guid.NewGuid();
            Managers.Photon.MakeRoom(Managers.Player.GetPlayerId() + newGuid.ToString()); // 자동으로 참여 하게 됨
        }
        else // 첫 로그인이 아닐 경우
        {
            // 그냥 바로 게임시작 UI 띄워준다.
            // UI를 이용해서 입장 하자
            Managers.UI.ClosePopupUI();
            Managers.UI.ShowPopupUI<After_Login_Popup_UI>("[Login]_After_Login_Popup_UI");
        }
    }

    public override void OnJoinedRoom()
    {
        // 첫 로그인 사용자가 로그인 씬에서 방까지 다 만들었을 경우에만 실행됨
        // UI를 이용해서 입장 하자
        Managers.UI.ClosePopupUI();
        Managers.UI.ShowPopupUI<After_Login_Popup_UI>("[Login]_After_Login_Popup_UI");

        Managers.Photon.CloseRoom();
    }
}
