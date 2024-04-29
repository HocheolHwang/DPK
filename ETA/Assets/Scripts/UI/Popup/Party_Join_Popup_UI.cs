using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Party_Join_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Open_Party_Join_Button,
        Open_Dungeon_Select_Button,
        Open_Dungeon_Enter_Button,
        Cancel_Button,
        Open_Party_Join_Confirm_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

        // 파티 참가 Popup UI 열기 버튼 이벤트 등록
        Button openPartyJoinButton = GetButton((int)Buttons.Open_Party_Join_Button);
        AddUIEvent(openPartyJoinButton.gameObject, OpenPartyJoin);

        // 던전 선택 Popup UI 열기 버튼 이벤트 등록
        Button openDungeonSelectButton = GetButton((int)Buttons.Open_Dungeon_Select_Button);
        AddUIEvent(openDungeonSelectButton.gameObject, OpenDungeonSelect);

        // 던전 입장 Popup UI 열기 버튼 이벤트 등록
        Button openDungeonEnterButton = GetButton((int)Buttons.Open_Dungeon_Enter_Button);
        AddUIEvent(openDungeonEnterButton.gameObject, OpenDungeonEnter);

        // 돌아가기 버튼 이벤트 등록
        Button cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 파티 참가 확인 Popup UI 열기 버튼 이벤트 등록
        Button openPartyJoinConfirmButton = GetButton((int)Buttons.Open_Party_Join_Confirm_Button);
        AddUIEvent(openPartyJoinConfirmButton.gameObject, OpenPartyJoinConfirm);
        AddUIKeyEvent(openPartyJoinConfirmButton.gameObject, () => OpenPartyJoinConfirm(null), KeyCode.Return);
    }

    // 파티 참가 Popup UI 열기
    private void OpenPartyJoin(PointerEventData data)
    {
        // 창을 닫고 다시 여므로 새로고침 하게 됨
        CloseAllPopupUI();
        Managers.UI.ShowPopupUI<Party_Join_Popup_UI>("[Lobby]_Party_Join_Popup_UI");
    }

    // 던전 선택 Popup UI 열기
    private void OpenDungeonSelect(PointerEventData data)
    {
        // 창을 모두 닫은 뒤에 던전 선택 Popup UI를 띄움
        CloseAllPopupUI();
        Managers.UI.ShowPopupUI<Dungeon_Select_Popup_UI>("[Lobby]_Dungeon_Select_Popup_UI");
    }

    // 던전 입장 Popup UI 열기
    private void OpenDungeonEnter(PointerEventData data)
    {
        // 창을 모두 닫고 로비 Popup UI를 띄운 뒤에 던전 입장 Popup UI를 띄움
        CloseAllPopupUI();
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
        Managers.UI.ShowPopupUI<Dungeon_Enter_Popup_UI>("[Lobby]_Dungeon_Enter_Popup_UI");
    }

    // 돌아가기
    private void Cancel(PointerEventData data)
    {
        // 파티 참가 Popup UI를 닫은 뒤 로비 Popup UI를 띄움
        ClosePopupUI();
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }

    // 파티 참가 확인 Popup UI 열기
    private void OpenPartyJoinConfirm(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Party_Join_Confirm_Popup_UI>("[Lobby]_Party_Join_Confirm_Popup_UI");
    }
}
