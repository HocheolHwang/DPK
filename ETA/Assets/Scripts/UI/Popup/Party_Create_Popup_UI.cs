using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button partyCreateButton;
    private TMP_InputField partyNameInputField;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));

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
        partyNameInputField.OnPointerClick(new PointerEventData(EventSystem.current));
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 파티 만들기 메서드
    private void PartyCreate(PointerEventData data)
    {
        // TODO: 파티 만드는 코드 추가 필요
        Debug.Log($"파티 이름: {partyNameInputField.text}");

        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 로비 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }
}
