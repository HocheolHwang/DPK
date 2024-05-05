using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Party_Join_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Open_Party_Join_Button,
        Open_Dungeon_Select_Button,
        Open_Dungeon_Enter_Button,
        Cancel_Button,
        Open_Party_Create_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button openPartyJoinButton;
    private Button openDungeonSelectButton;
    private Button openDungeonEnterButton;
    private Button cancelButton;
    private Button openPartyCreateButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 파티 참가 Popup UI 띄우기 버튼 이벤트 등록
        openPartyJoinButton = GetButton((int)Buttons.Open_Party_Join_Button);
        AddUIEvent(openPartyJoinButton.gameObject, OpenPartyJoin);

        // 던전 선택 Popup UI 띄우기 버튼 이벤트 등록
        openDungeonSelectButton = GetButton((int)Buttons.Open_Dungeon_Select_Button);
        AddUIEvent(openDungeonSelectButton.gameObject, OpenDungeonSelect);

        // 던전 입장 Popup UI 띄우기 버튼 이벤트 등록
        openDungeonEnterButton = GetButton((int)Buttons.Open_Dungeon_Enter_Button);
        AddUIEvent(openDungeonEnterButton.gameObject, OpenDungeonEnter);

        // 닫기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 파티 만들기 Popup UI 띄우기 버튼 이벤트 등록
        openPartyCreateButton = GetButton((int)Buttons.Open_Party_Create_Button);
        AddUIEvent(openPartyCreateButton.gameObject, OpenPartyCreate);
        AddUIKeyEvent(openPartyCreateButton.gameObject, () => OpenPartyCreate(null), KeyCode.C);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 파티 참가 Popup UI 띄우기 메서드
    private void OpenPartyJoin(PointerEventData data)
    {
        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 파티 참가 Popup UI를 띄움 (새로고침)
        Managers.UI.ShowPopupUI<Party_Join_Popup_UI>("[Lobby]_Party_Join_Popup_UI");
    }

    // 던전 선택 Popup UI 띄우기 메서드
    private void OpenDungeonSelect(PointerEventData data)
    {
        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 던전 선택 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Dungeon_Select_Popup_UI>("[Lobby]_Dungeon_Select_Popup_UI");
    }

    // 던전 입장 Popup UI 띄우기 메서드
    private void OpenDungeonEnter(PointerEventData data)
    {
        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 로비 Popup UI를 띄운 뒤에 던전 입장 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
        Managers.UI.ShowPopupUI<Dungeon_Enter_Popup_UI>("[Lobby]_Dungeon_Enter_Popup_UI");
    }

    // 닫기 메서드
    private void Cancel(PointerEventData data)
    {
        // 파티 참가 Popup UI를 닫음
        ClosePopupUI();

        // 로비 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }

    // 파티 만들기 Popup UI 띄우기 메서드
    private void OpenPartyCreate(PointerEventData data)
    {
        // 파티 만들기 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Party_Create_Popup_UI>("[Lobby]_Party_Create_Popup_UI");
    }
}
