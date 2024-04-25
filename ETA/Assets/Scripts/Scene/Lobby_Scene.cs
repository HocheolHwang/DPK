using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Scene : MonoBehaviour
{
    void Start()
    {
        Managers.UI.ShowPopupUI<Lobby_Dungeon_Popup_UI>("[Common]_Lobby_Dungeon_Popup_UI");
    }
}
