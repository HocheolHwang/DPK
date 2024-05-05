using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Dungeon_Select_Unable_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 바인딩
        Bind<Button>(typeof(Buttons));

        // 덛기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 닫기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }
}
