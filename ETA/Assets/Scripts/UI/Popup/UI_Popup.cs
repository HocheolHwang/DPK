using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "Popup UI"를 관리하는 클래스
/// </summary>
public class UI_Popup : UI_Base
{
    // "Popup UI"를 초기화하는 메서드
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    // "Popup UI"를 닫는 메서드
    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
