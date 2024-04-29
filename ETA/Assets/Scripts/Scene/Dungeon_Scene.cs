using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Scene : MonoBehaviour
{
    void Start()
    {
        Managers.UI.ShowPopupUI<Dungeon_Popup_UI>("[Dungeon]_Dungeon_Popup_UI");
    }
}
