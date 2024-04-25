using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Scene : MonoBehaviour
{
    void Start()
    {
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }
}
