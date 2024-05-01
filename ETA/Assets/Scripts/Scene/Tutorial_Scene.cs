using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Scene : BaseScene
{
    private void Start()
    {
        Managers.UI.ShowPopupUI<Dungeon_Popup_UI>("[Dungeon]_Dungeon_Popup_UI");
    }

    public override void Clear()
    {
        Debug.Log("Tutorial Scene Clear");
    }
}
