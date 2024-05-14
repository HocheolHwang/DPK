using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Party_Create_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button,
        Party_Create_Button
    }

    enum InputFields
    {
        Party_Name_InputField
    }

    enum Texts
    {
        Warning_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button partyCreateButton;
    private TMP_InputField partyNameInputField;
    private TextMeshProUGUI warning;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 파티 만들기 버튼 이벤트 등록
        partyCreateButton = GetButton((int)Buttons.Party_Create_Button);
        AddUIEvent(partyCreateButton.gameObject, PartyCreate);
        AddUIKeyEvent(partyCreateButton.gameObject, () => PartyCreate(null), KeyCode.Return);

        // 파티 이름 입력 정보 및 포커스 설정
        partyNameInputField = Get<TMP_InputField>((int)InputFields.Party_Name_InputField);

        // 인풋 필드에 포커스
        FocusOnInputField();

        // 경고 문구
        warning = GetText((int)Texts.Warning_Text);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 인풋 필드에 포커스하는 메서드
    private void FocusOnInputField()
    {
        partyNameInputField.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    // 파티 만들기 메서드
    private void PartyCreate(PointerEventData data)
    {
        warning.text = "";

        // 파티 이름 입력 필드 비어있을 시 경고문구를 출력하고 메서드 종료
        if (string.IsNullOrEmpty(partyNameInputField.text))
        {
            warning.text = "파티 이름을 입력해주세요.";
            FocusOnInputField();
            return;
        }

        // 파티 이름이 16자 초과일 경우 경고문구를 출력하고 메서드 종료
        if (partyNameInputField.text.Length > 16)
        {
            warning.text = "파티 이름은 16자 이하로 설정해주세요.";
            FocusOnInputField();
            return;
        }

        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 로비 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");

        // 파티 생성
        Managers.Photon.MakeRoom(partyNameInputField.text);
        try
        {
        }
        catch(RoomCreationException ex)
        {
            var popupUI = GameObject.FindFirstObjectByType<Lobby_Popup_UI>();
            if (popupUI != null)
            {
                Managers.Coroutine.StartCoroutine(popupUI.ShowWarningPopupCoroutine(ex.Title, ex.Message));
            }
            else
            {
                Debug.LogError("Lobby_Popup_UI is not found in the scene.");
            }
        }

    }

}
