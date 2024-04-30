using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

    //포톤 채팅
    private PhotonChat chat;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        chat = GameObject.Find("@Scene").GetComponent<PhotonChat>();
        chat.chatUI = this; 
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

        // Chat_InputField에 포커스 설정
        StartCoroutine(SetFocusOnInputField());
    }

    // 뒤로가기
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 입력하기
    private void ChatEnter(PointerEventData data)
    {
        // 입력 필드가 비어있을 시
        if (string.IsNullOrEmpty(chatMessage.text))
        {
            // 입력 필드에 포커스하고 메서드 종료
            SetFocusToInputField();
            return;
        }

        // 여기에 메시지를 보내는 등의 코드를 추가
        // 
        if (chat != null)
            chat.SendMessage(chatMessage.text);
        else
        {
            chat = GameObject.Find("@Scene").GetComponent<PhotonChat>();
            chat.SendMessage(chatMessage.text);

        }

        // 메시지 전송 후 입력 필드에 다시 포커스 설정
        SetFocusToInputField();
    }

    // 채팅창이 처음 열렸을 때 포커스 설정
    private IEnumerator SetFocusOnInputField()
    {
        // 프레임이 완전히 렌더링될 때까지 기다림
        yield return null;

        // 입력 필드에 포커스 설정하는 메서드 호출
        SetFocusToInputField();
    }

    // 입력 필드에 포커스 설정하는 메서드
    private void SetFocusToInputField()
    {
        EventSystem.current.SetSelectedGameObject(chatMessage.gameObject);
        chatMessage.ActivateInputField();
    }

    public  void ReceiveMessage(string sender, string message)
    {
        // Managers.Resource.Load<GameObject>();
        GameObject chatPrefab = Managers.Resource.Instantiate("UI/SubItem/Chat_Item");

        chatPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sender + " : " + message;

        chatPrefab.transform.SetParent(gameObject.transform.Find("Chat_Popup_Container/Chat_Container/Scroll View/Viewport/Content"));
    }
}
