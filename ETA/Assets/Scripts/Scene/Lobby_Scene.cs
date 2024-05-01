using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Scene : BaseScene
{
    private void Start()
    {
        // 첫 로그인 여부를 확인
        bool isFirstLogin = PlayerPrefs.GetInt("FirstLogin", 1) == 1;

        if (Managers.Player.GetFirst() && isFirstLogin)
        {
            // 첫 로그인 O: 자격증 Popup UI를 띄움
            Managers.UI.ShowPopupUI<Certificate_Popup_UI>("[Lobby]_Certificate_Popup_UI");

            // 첫 로그인 여부 0으로 저장
            PlayerPrefs.SetInt("FirstLogin", 0);
            PlayerPrefs.Save();
        }
        else
        {
            // 첫 로그인 X: 로비 Popup UI를 띄움
            Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
        }

        Managers.Sound.Play("BackgroundMusic/Lobby");
    }

    public override void Clear()
    {
        Debug.Log("Lobby Scene Clear");
    }
}
