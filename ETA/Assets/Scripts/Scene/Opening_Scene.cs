using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening_Scene : BaseScene
{
    private void Start()
    {
        Managers.UI.ShowPopupUI<Opening_Popup_UI>("[Opening]_Opening_Popup_UI");
    }

    public override void Clear()
    {
        Debug.Log("Opening Scene Clear");
    }
}
