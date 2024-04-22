using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "Scene UI"를 관리하는 클래스
/// </summary>
public class UI_Scene : UI_Base
{
    // "Scene UI"를 초기화하는 메서드
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);
    }
}
