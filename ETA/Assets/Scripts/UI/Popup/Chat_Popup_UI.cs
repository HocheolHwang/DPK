using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Chat_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button,
        Chat_Enter_Button
    }

    enum InputFields
    {
        Chat_InputField
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button chatEnterButton;
    private TMP_InputField chatMessage;

    // 포톤 채팅
    private PhotonChat chat;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));

        // 닫기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 입력하기 버튼 이벤트 등록
        chatEnterButton = GetButton((int)Buttons.Chat_Enter_Button);
        AddUIEvent(chatEnterButton.gameObject, ChatEnter);
        AddUIKeyEvent(chatEnterButton.gameObject, () => ChatEnter(null), KeyCode.Return);

        // 로그인 입력 정보
        chatMessage = Get<TMP_InputField>((int)InputFields.Chat_InputField);

        // PhotonChat 컴포넌트 찾기 및 참조
        chat = GameObject.Find("@Scene").GetComponent<PhotonChat>();
        chat.chatUI = this;

        // Chat_InputField에 포커스 설정
        StartCoroutine(SetFocusOnInputField());
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 닫기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 입력하기 메서드
    private void ChatEnter(PointerEventData data)
    {
        // Input Field가 비어있을 시
        if (string.IsNullOrEmpty(chatMessage.text))
        {
            // Input Field에 포커스하고 메서드 종료
            SetFocusToInputField();
            return;
        }

        // 메세지 전송
        if (chat != null)
            chat.SendMessage(chatMessage.text);
        else
        {
            chat = GameObject.Find("@Scene").GetComponent<PhotonChat>();
            chat.SendMessage(chatMessage.text);
        }

        // 메시지 전송 후 입력 필드 안의 내용 비우기
        chatMessage.text = "";

        // 메시지 전송 후 Input Field에 다시 포커스 설정
        SetFocusToInputField();
    }

    // Input Field에 자동으로 포커스를 맞추는 코루틴
    private IEnumerator SetFocusOnInputField()
    {
        // 프레임이 완전히 렌더링될 때까지 기다림
        yield return null;

        // Input Field에 포커스 설정하는 메서드 호출
        SetFocusToInputField();
    }

    // Input Field에 포커스 설정하는 메서드
    private void SetFocusToInputField()
    {
        EventSystem.current.SetSelectedGameObject(chatMessage.gameObject);
        chatMessage.ActivateInputField();
    }

    // 다른 플레이어로부터 메시지를 받았을 때 화면에 표시하는 메서드
    public void ReceiveMessage(string sender, string message)
    {
        // Managers.Resource.Load<GameObject>();
        GameObject chatPrefab = Managers.Resource.Instantiate("UI/SubItem/Chat_Item");

        // 받는 메시지 형태 설정: "보낸 사람 : 메시지"
        chatPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sender + " : " + message;

        // 메시지를 UI의 채팅 컨테이너에 배치하기
        chatPrefab.transform.SetParent(gameObject.transform.Find("Chat_Popup_Container/Chat_Container/Scroll View/Viewport/Content"));
    }
}
