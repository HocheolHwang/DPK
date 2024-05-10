using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Lobby_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Open_Party_Join_Button,
        Open_Party_Leave_Button,
        Open_Dungeon_Select_Button,
        Open_Dungeon_Enter_Button,
        Open_Chat_Button,
        Open_Menu_Button
    }

    enum Texts
    {
        // 파티원 상태
        Member_Level_Text_1,
        Member_Level_Text_2,
        Member_Level_Text_3,
        Member_Nickname_Text_1,
        Member_Nickname_Text_2,
        Member_Nickname_Text_3,

        // 파티 정보
        Party_Name_Text,
        Dungeon_Name_Text,
        Party_Size_Text,
    }

    enum Images
    {
        // 파티원 상태
        Party_Member_1,
        Party_Member_2,
        Party_Member_3,

        // 파티원 클래스 아이콘
        Warrior_Icon_1,
        Acher_Icon_1,
        Mage_Icon_1,
        Warrior_Icon_2,
        Acher_Icon_2,
        Mage_Icon_2,
        Warrior_Icon_3,
        Acher_Icon_3,
        Mage_Icon_3,

        // 파티 정보
        Party_Info
    }

    // UI 컴포넌트 바인딩 변수
    private Button openPartyJoinButton;
    private Button openPartyLeaveButton;
    private Button openDungeonSelectButton;
    private Button openDungeonEnterButton;
    private Button openChatButton;
    private Button openMenuButton;
    private TextMeshProUGUI[] memberLevelTexts = new TextMeshProUGUI[3];
    private TextMeshProUGUI[] memberNicknameTexts = new TextMeshProUGUI[3];
    private TextMeshProUGUI partyNameText;
    private TextMeshProUGUI dungeonNameText;
    private TextMeshProUGUI partySizeText;
    private Image[] partyMembers = new Image[3];
    private Image[][] partyMemberIcons = new Image[3][];
    private Image partyInfoImage;

    // 파티원 수
    private int partySize = PhotonNetwork.PlayerList.Length;

    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        // 파티 참가 Popup UI 띄우기 버튼 이벤트 등록
        openPartyJoinButton = GetButton((int)Buttons.Open_Party_Join_Button);
        AddUIEvent(openPartyJoinButton.gameObject, OpenPartyJoin);

        // 파티 탈퇴 Popup UI 띄우기 버튼 이벤트 등록
        openPartyLeaveButton = GetButton((int)Buttons.Open_Party_Leave_Button);
        AddUIEvent(openPartyLeaveButton.gameObject, OpenPartyLeave);

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

        // 파티 정보 초기화
        partyInfoImage = GetImage((int)Images.Party_Info);
        partyNameText = GetText((int)Texts.Party_Name_Text);
        partySizeText = GetText((int)Texts.Party_Size_Text);

        // 파티원 아이콘 초기화
        partyMemberIcons[0] = new Image[3];
        partyMemberIcons[1] = new Image[3];
        partyMemberIcons[2] = new Image[3];

        // 파티원 정보 초기화
        for (int i = 0; i < 3; i++)
        {
            // 파티원 정보
            partyMembers[i] = GetImage((int)Images.Party_Member_1 + i);
            memberLevelTexts[i] = GetText((int)Texts.Member_Level_Text_1 + i);
            memberNicknameTexts[i] = GetText((int)Texts.Member_Nickname_Text_1 + i);

            // 클래스 아이콘
            partyMemberIcons[0][i] = GetImage((int)Images.Warrior_Icon_1 + i);
            partyMemberIcons[1][i] = GetImage((int)Images.Warrior_Icon_2 + i);
            partyMemberIcons[2][i] = GetImage((int)Images.Warrior_Icon_3 + i);
        }

        // 파티 정보 업데이트
        UpdatePartyInfo();

        // 선택된 던전 업데이트
        dungeonNameText = GetText((int)Texts.Dungeon_Name_Text);
        UpdateSelectedDungeon();
    }


    // ------------------------------ 유니티 생명주기 메서드 ------------------------------

    private void Update()
    {
        // 파티원 수가 바뀌면 파티 정보 UI 및 파티원 수 업데이트
        if (partySize != PhotonNetwork.PlayerList.Length)
        {
            UpdatePartyInfo();
            partySize = PhotonNetwork.PlayerList.Length;
        }
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

    // 파티 탈퇴 Popup UI 띄우기 메서드
    private void OpenPartyLeave(PointerEventData data)
    {
        // 파티 탈퇴 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Party_Leave_Popup_UI>("[Lobby]_Party_Leave_Popup_UI");
    }

    // 던전 선택 Popup UI 띄우기 메서드
    private void OpenDungeonSelect(PointerEventData data)
    {
        if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
        {
            // 파티원일 경우 Coroutine을 시작하여 경고 Popup UI를 띄움
            StartCoroutine(ShowWarningPopupCoroutine("던전 선택 불가", "던전 선택은 파티장만 가능합니다."));
        }
        else
        {
            // 모든 Popup UI를 닫음
            CloseAllPopupUI();

            // 파티장이거나 파티 미소속일 경우 던전 선택 Popup UI를 띄움
            Managers.UI.ShowPopupUI<Dungeon_Select_Popup_UI>("[Lobby]_Dungeon_Select_Popup_UI");
        }
    }

    // 던전 입장 Popup UI 띄우기 메서드
    private void OpenDungeonEnter(PointerEventData data)
    {
        if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
        {
            // 파티원일 경우 Coroutine을 시작하여 경고 Popup UI를 띄움
            StartCoroutine(ShowWarningPopupCoroutine("던전 입장 불가", "던전 입장은 파티장만 가능합니다."));
        }
        else
        {
            // 파티장이거나 파티 미소속일 경우 던전 입장 Popup UI를 띄움
            Managers.UI.ShowPopupUI<Dungeon_Enter_Popup_UI>("[Lobby]_Dungeon_Enter_Popup_UI");
        }
    }

    // 경고 문구 업데이트 코루틴
    private IEnumerator ShowWarningPopupCoroutine(string title, string message)
    {
        // 경고 Popup UI를 띄움
        var warningPopup = Managers.UI.ShowPopupUI<Warning_Popup_UI>("[Lobby]_Warning_Popup_UI");

        // 프레임을 넘겨서 UI가 완전히 열릴 시간을 줌
        yield return null;

        // 매개변수로 받은 텍스트를 사용하여 팝업이 열린 이후에 텍스트를 설정
        warningPopup.SetWarningText(title, message);
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
    public void UpdatePartyInfo()
    {
        if (PhotonNetwork.InRoom) // 파티 참가 상태일 경우
        {
            // 파티 참가 버튼 비활성화 및 탈퇴 버튼 활성화
            openPartyJoinButton.gameObject.SetActive(false);
            openPartyLeaveButton.gameObject.SetActive(true);

            // 파티 정보 활성화 및 업데이트
            partyInfoImage.gameObject.SetActive(true);
            partyNameText.text = $"{Managers.Photon.ConvertRoomName(PhotonNetwork.CurrentRoom.Name)}";
            partySizeText.text = $"{PhotonNetwork.PlayerList.Length} / 3";

            // 파티 멤버 정보 업데이트
            for (int i = 0; i < partyMembers.Length; i++)
            {
                if (i < PhotonNetwork.PlayerList.Length)
                {
                    Photon.Realtime.Player player = PhotonNetwork.PlayerList[i];

                    // 파티 멤버 정보 업데이트
                    partyMembers[i].gameObject.SetActive(true);
                    memberLevelTexts[i].text = $"Lv. {(int)player.CustomProperties["PlayerLevel"]}";
                    memberNicknameTexts[i].text = PhotonNetwork.PlayerList[i].NickName;

                    // 클래스 아이콘 업데이트
                    UpdateClassIcon(i, (string)player.CustomProperties["CurClass"]);
                }
                else
                {
                    // 나머지 파티 멤버 정보 비활성화
                    partyMembers[i].gameObject.SetActive(false);
                }
            }
        }
        else // 파티 미참가 상태일 경우
        {
            // 파티 참가 버튼 활성화 및 탈퇴 버튼 비활성화
            openPartyJoinButton.gameObject.SetActive(true);
            openPartyLeaveButton.gameObject.SetActive(false);

            // 파티 정보 비활성화
            partyInfoImage.gameObject.SetActive(false);

            // 플레이어 정보 업데이트
            partyMembers[0].gameObject.SetActive(true);
            memberLevelTexts[0].text = $"Lv. {Managers.Player.GetLevel()}";
            memberNicknameTexts[0].text = Managers.Player.GetNickName();

            // 클래스 아이콘 업데이트
            UpdateClassIcon(0, Managers.Player.GetClassCode());

            // 나머지 파티 멤버 정보 비활성화
            partyMembers[1].gameObject.SetActive(false);
            partyMembers[2].gameObject.SetActive(false);
        }
    }

    // 아이콘 업데이트 메서드
    private void UpdateClassIcon(int memberIndex, string classCode)
    {
        // 모든 아이콘을 비활성화
        for (int i = 0; i < partyMemberIcons[memberIndex].Length; i++)
        {
            partyMemberIcons[memberIndex][i].gameObject.SetActive(false);
        }

        // 클래스 코드에 따라 해당 아이콘만 활성화
        switch (classCode)
        {
            case "C001":
                partyMemberIcons[memberIndex][0].gameObject.SetActive(true);
                break;
            case "C002":
                partyMemberIcons[memberIndex][1].gameObject.SetActive(true);
                break;
            case "C003":
                partyMemberIcons[memberIndex][2].gameObject.SetActive(true);
                break;
            default:
                Debug.Log("알 수 없는 클래스 코드: " + classCode);
                break;
        }
    }

    // 선택된 던전 업데이트 메서드
    public void UpdateSelectedDungeon()
    {
        // 선택된 던전 번호를 가져옴
        int selectedDungeonNumber = FindObjectOfType<Lobby_Scene>().currentDungeonNumber;

        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            FindObjectOfType<GameSystem>().SendCurrentDungeon(selectedDungeonNumber);
            Managers.Photon.DungeonIndex = selectedDungeonNumber;
        }

        // 선택된 던전 번호에 따라 다른 텍스트를 설정
        dungeonNameText.text = selectedDungeonNumber switch
        {
            1 => "별의 조각 평원",
            2 => "잊혀진 신전",
            3 => "심연의 바다",
            _ => "알 수 없는 던전입니다."
        };
    }
}
