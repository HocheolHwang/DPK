using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Lobby_Popup_UI : UI_Popup
{
    // 텍스트 인덱스
    enum Texts
    {
        Member_Nickname_Text_1,
        Dungeon_Name_Text,
        Party_Name_Text
    }

    // 버튼 인덱스
    enum Buttons
    {
        Open_Party_Join_Button,
        Open_Dungeon_Select_Button,
        Open_Dungeon_Enter_Button,
        Open_Chat_Button,
        Open_Menu_Button
    }

    // 클래스 멤버 변수로 선언
    private TextMeshProUGUI dungeonNameText;
    private TextMeshProUGUI memberNicknameText1;
    private TextMeshProUGUI partyNameText;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // 닉네임 및 파티 명
        memberNicknameText1 = GetText((int)Texts.Member_Nickname_Text_1);
        partyNameText = GetText((int)Texts.Party_Name_Text);
        UpdateUserInfo();

        // 선택된 던전
        dungeonNameText = GetText((int)Texts.Dungeon_Name_Text);
        UpdateSelectedDungeon();

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

    // 유저 정보 업데이트하기
    private void UpdateUserInfo()
    {
        memberNicknameText1.text = Managers.Player.GetNickName();
        partyNameText.text = $"{Managers.Player.GetNickName()}의 파티";
    }

    // 선택된 던전 업데이트하기
    private void UpdateSelectedDungeon()
    {
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);

        // 선택된 던전 번호에 따라 다른 텍스트를 설정
        switch (selectedDungeonNumber)
        {
            case 1:
                dungeonNameText.text = "깊은 숲";
                break;
            case 2:
                dungeonNameText.text = "잊혀진 신전";
                break;
            case 3:
                dungeonNameText.text = "별의 조각 평원";
                break;
            default:
                dungeonNameText.text = "알 수 없는 던전입니다.";
                break;
        }
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
