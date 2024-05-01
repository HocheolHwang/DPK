using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Party_Join_Confirm_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button,
        Party_Join_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button partyJoinButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 참가하기 버튼 이벤트 등록
        partyJoinButton = GetButton((int)Buttons.Party_Join_Button);
        AddUIEvent(partyJoinButton.gameObject, PartyJoin);
        AddUIKeyEvent(partyJoinButton.gameObject, () => PartyJoin(null), KeyCode.Return);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 참가하기 메서드
    private void PartyJoin(PointerEventData data)
    {
        // TODO: 파티에 참가하는 코드 추가 필요

        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 로비 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }
}
