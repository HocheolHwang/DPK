using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login_Scene : MonoBehaviour
{
    void Start()
    {
        Managers.UI.ShowPopupUI<Before_Login_Popup_UI>("[Login]_Before_Login_Popup_UI");
        Managers.UI.ShowPopupUI<Login_Popup_UI>("[Login]_Login_Popup_UI");
        // Managers.UI.ShowPopupUI<Login_Loading_Popup_UI>("[Login]_Login_Loading_Popup_UI");
    }
}
