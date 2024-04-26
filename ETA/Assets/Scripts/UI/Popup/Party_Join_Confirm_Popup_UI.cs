using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Party_Join_Confirm_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Cancel_Button,
        Party_Join_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

        // 취소하기 버튼 이벤트 등록
        Button cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 참가하기 버튼 이벤트 등록
        Button partyJoinButton = GetButton((int)Buttons.Party_Join_Button);
        AddUIEvent(partyJoinButton.gameObject, PartyJoin);
        AddUIKeyEvent(partyJoinButton.gameObject, () => PartyJoin(null), KeyCode.Return);
    }

    // 취소하기
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 참가하기
    private void PartyJoin(PointerEventData data)
    {
        // TODO: 파티에 참가하는 코드 추가 필요

        // Popup UI를 모두 닫은 뒤 로비 Popup UI를 띄움
        CloseAllPopupUI();
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }
}
