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
}
