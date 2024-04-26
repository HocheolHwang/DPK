using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Chat_Popup_UI : UI_Popup
{
    // 입력 필드 인덱스
    enum InputFields
    {
        Chat_InputField
    }

    // 버튼 인덱스
    enum Buttons
    {
        Cancel_Button,
        Chat_Enter_Button
    }

    // 클래스 멤버 변수로 선언
    private TMP_InputField chatMessage;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));

        // 로그인 입력 정보
        chatMessage = Get<TMP_InputField>((int)InputFields.Chat_InputField);

        // 뒤로가기 버튼 이벤트 등록
        Button cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 입력하기 버튼 이벤트 등록
        Button chatEnterButton = GetButton((int)Buttons.Chat_Enter_Button);
        AddUIEvent(chatEnterButton.gameObject, ChatEnter);
        AddUIKeyEvent(chatEnterButton.gameObject, () => ChatEnter(null), KeyCode.Return);
    }

    // 뒤로가기
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 입력하기
    private void ChatEnter(PointerEventData data)
    {
        // 입력 필드가 비어있을 시 메서드 종료
        if (string.IsNullOrEmpty(chatMessage.text)) return;


    }
}
