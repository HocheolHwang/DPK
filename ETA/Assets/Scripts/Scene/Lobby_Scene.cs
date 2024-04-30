using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Scene : MonoBehaviour
{
    void Start()
    {
        // 첫 번째 로그인일 경우, 자격증 획득 팝업을 띄움
        if (Managers.Player.GetFirst())
        {
            Managers.UI.ShowPopupUI<Certificate_Popup_UI>("[Lobby]_Certificate_Popup_UI");
        }
        else
        {
            Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
        }
    }
}
