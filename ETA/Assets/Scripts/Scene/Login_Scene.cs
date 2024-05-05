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
        // 로그인 후 성공적으로 로비 입장시 UI가 뜬다.
        Managers.UI.ShowPopupUI<After_Login_Popup_UI>("[Login]_After_Login_Popup_UI");
    }
}
