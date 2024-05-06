using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Party_Leave_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button,
        Party_Leave_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button partyLeaveButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 바인딩
        Bind<Button>(typeof(Buttons));

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 파티 탈퇴 버튼 이벤트 등록
        partyLeaveButton = GetButton((int)Buttons.Party_Leave_Button);
        AddUIEvent(partyLeaveButton.gameObject, PartyLeave);
        AddUIKeyEvent(partyLeaveButton.gameObject, () => PartyLeave(null), KeyCode.Return);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 파티 탈퇴 메서드
    private void PartyLeave(PointerEventData data)
    {
        // 파티 탈퇴
        PhotonNetwork.LeaveRoom();


    }

    public override void OnLeftRoom() // 파티 탈퇴하고 나면 처리
    {
        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 로비 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }


}
