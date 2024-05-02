using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Lobby_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Open_Party_Join_Button,
        Open_Dungeon_Select_Button,
        Open_Dungeon_Enter_Button,
        Open_Chat_Button,
        Open_Menu_Button
    }

    enum Texts
    {
        Member_Nickname_Text_1,
        Party_Name_Text,
        Dungeon_Name_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button openPartyJoinButton;
    private Button openDungeonSelectButton;
    private Button openDungeonEnterButton;
    private Button openChatButton;
    private Button openMenuButton;
    private TextMeshProUGUI memberNicknameText1;
    private TextMeshProUGUI partyNameText;
    private TextMeshProUGUI dungeonNameText;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 파티 참가 Popup UI 띄우기 버튼 이벤트 등록
        openPartyJoinButton = GetButton((int)Buttons.Open_Party_Join_Button);
        AddUIEvent(openPartyJoinButton.gameObject, OpenPartyJoin);

        // 던전 선택 Popup UI 띄우기 버튼 이벤트 등록
        openDungeonSelectButton = GetButton((int)Buttons.Open_Dungeon_Select_Button);
        AddUIEvent(openDungeonSelectButton.gameObject, OpenDungeonSelect);

        // 던전 입장 Popup UI 띄우기 버튼 이벤트 등록
        openDungeonEnterButton = GetButton((int)Buttons.Open_Dungeon_Enter_Button);
        AddUIEvent(openDungeonEnterButton.gameObject, OpenDungeonEnter);

        // 채팅 Popup UI 띄우기 버튼 이벤트 등록
        openChatButton = GetButton((int)Buttons.Open_Chat_Button);
        AddUIEvent(openChatButton.gameObject, OpenChat);
        AddUIKeyEvent(openChatButton.gameObject, () => OpenChat(null), KeyCode.C);

        // 메뉴 Popup UI 띄우기 버튼 이벤트 등록
        openMenuButton = GetButton((int)Buttons.Open_Menu_Button);
        AddUIEvent(openMenuButton.gameObject, OpenMenu);
        AddUIKeyEvent(openMenuButton.gameObject, () => OpenMenu(null), KeyCode.Escape);

        // 파티 정보 업데이트
        memberNicknameText1 = GetText((int)Texts.Member_Nickname_Text_1);
        partyNameText = GetText((int)Texts.Party_Name_Text);
        UpdatePartyInfo();

        // 선택된 던전 업데이트
        dungeonNameText = GetText((int)Texts.Dungeon_Name_Text);
        UpdateSelectedDungeon();
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 파티 참가 Popup UI 띄우기 메서드
    private void OpenPartyJoin(PointerEventData data)
    {
        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 파티 참가 Popup UI를 띄움
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
        Managers.UI.ShowPopupUI<Dungeon_Enter_Popup_UI>("[Lobby]_Dungeon_Enter_Popup_UI");
    }

    // 채팅 Popup UI 띄우기 메서드
    private void OpenChat(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Chat_Popup_UI>("[Common]_Chat_Popup_UI");
    }

    // 메뉴 Popup UI 띄우기 메서드
    private void OpenMenu(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Menu_Popup_UI>("[Common]_Menu_Popup_UI");
    }

    // 파티 정보 업데이트 메서드
    private void UpdatePartyInfo()
    {
        memberNicknameText1.text = Managers.Player.GetNickName();
        partyNameText.text = $"{Managers.Player.GetNickName()}의 파티";
    }

    // 선택된 던전 업데이트 메서드
    private void UpdateSelectedDungeon()
    {
        // 선택된 던전 번호를 가져옴
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);

        // 선택된 던전 번호에 따라 다른 텍스트를 설정
        dungeonNameText.text = selectedDungeonNumber switch
        {
            1 => "깊은 숲",
            2 => "잊혀진 신전",
            3 => "별의 조각 평원",
            _ => "알 수 없는 던전입니다."
        };
    }
}
