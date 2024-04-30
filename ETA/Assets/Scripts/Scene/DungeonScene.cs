using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScene : BaseScene
{
    private void Start()
    {
        Managers.UI.ShowPopupUI<Dungeon_Popup_UI>("[Dungeon]_Dungeon_Popup_UI");
    }
    // Start is called before the first frame update
    public override void Clear()
    {
        Debug.Log("Dungeon Scene Clear");
    }
}
