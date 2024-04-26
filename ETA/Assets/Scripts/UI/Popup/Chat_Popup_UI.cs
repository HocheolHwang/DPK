using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Chat_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Cancel_Button,
        Chat_Enter_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

        // 뒤로가기 버튼 이벤트 등록
        Button cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.C);

        // 입력하기 버튼 이벤트 등록
        Button chatEnterButton = GetButton((int)Buttons.Chat_Enter_Button);
        AddUIEvent(chatEnterButton.gameObject, ChatEnter);
        AddUIKeyEvent(chatEnterButton.gameObject, () => ChatEnter(null), KeyCode.Return);
    }

    // 남아 있기
    private void Cancel(PointerEventData data)
    {
        // 현재 Popup UI를 닫음
        ClosePopupUI();
    }

    // 게임 종료
    private void ChatEnter(PointerEventData data)
    {
        // TODO: 채팅을 보내는 코드 작성 필요
    }
}
