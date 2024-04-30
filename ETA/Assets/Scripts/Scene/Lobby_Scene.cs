using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Scene : BaseScene
{
    void Start()
    {
        // PlayerPrefs에서 첫 번째 로그인 여부를 확인
        // "FirstLogin"이 0이면 첫 로그인이 아님, 1이면 첫 로그인임
        bool isFirstLogin = PlayerPrefs.GetInt("FirstLogin", 1) == 1;

        // 첫 번째 로그인일 경우, 자격증 획득 팝업을 띄움
        if (Managers.Player.GetFirst() && isFirstLogin)
        {
            Managers.UI.ShowPopupUI<Certificate_Popup_UI>("[Lobby]_Certificate_Popup_UI");

            // 이제 첫 번째 로그인이 처리되었으므로, "FirstLogin"을 0으로 설정하여 저장
            PlayerPrefs.SetInt("FirstLogin", 0);
            PlayerPrefs.Save(); // 변경 사항을 저장
        }
        else
        {
            Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
        }
    }

    public override void Clear()
    {
        Debug.Log("Login Scene Clear");
    }
}
