using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Lobby_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Open_Party_Join_Button,
        Open_Dungeon_Select_Button,
        Open_Dungeon_Enter_Button,
        Open_Chat_Button,
        Open_Menu_Button
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

        // 채팅 Popup UI 열기 버튼 이벤트 등록
        Button openChatButton = GetButton((int)Buttons.Open_Chat_Button);
        AddUIEvent(openChatButton.gameObject, OpenChat);
        AddUIKeyEvent(openChatButton.gameObject, () => OpenChat(null), KeyCode.C);

        // 메뉴 Popup UI 열기 버튼 이벤트 등록
        Button openMenuButton = GetButton((int)Buttons.Open_Menu_Button);
        AddUIEvent(openMenuButton.gameObject, OpenMenu);
        AddUIKeyEvent(openMenuButton.gameObject, () => OpenMenu(null), KeyCode.Escape);
    }

    // 파티 참가 Popup UI 열기
    private void OpenPartyJoin(PointerEventData data)
    {
        // 모든 Popup UI를 닫고 파티 참가 Popup UI를 띄움
        CloseAllPopupUI();
        Managers.UI.ShowPopupUI<Party_Join_Popup_UI>("[Lobby]_Party_Join_Popup_UI");
    }

    // 던전 선택 Popup UI 열기
    private void OpenDungeonSelect(PointerEventData data)
    {
        // 모든 Popup UI를 닫고 던전 선택 Popup UI를 띄움
        CloseAllPopupUI();
        Managers.UI.ShowPopupUI<Dungeon_Select_Popup_UI>("[Lobby]_Dungeon_Select_Popup_UI");
    }

    // 던전 입장 Popup UI 열기
    private void OpenDungeonEnter(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Dungeon_Enter_Popup_UI>("[Lobby]_Dungeon_Enter_Popup_UI");
    }

    // 채팅 Popup UI 열기
    private void OpenChat(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Chat_Popup_UI>("[Common]_Chat_Popup_UI");
    }

    // 메뉴 Popup UI 열기
    private void OpenMenu(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Menu_Popup_UI>("[Common]_Menu_Popup_UI");
    }
}
