using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;
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
        
        // 현재 파티 리스트 업데이트
        UpdatePartyList();
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

    // 현재 파티 리스트 업데이트 메서드
    public void UpdatePartyList()
    {
        // Managers.Resource.Load<GameObject>();

        List<RoomInfo> roomlist = Managers.Photon.roomlist;

        for (int i = 0; i < roomlist.Count; i++)
        {
            GameObject partyPrefab= Managers.Resource.Instantiate("UI/SubItem/Party_Item");
            Transform partyInfo = partyPrefab.transform.GetChild(0);

            string roomName = roomlist[i].Name;
            int lastIndex = roomName.LastIndexOf("`");

            if (lastIndex != -1)
                roomName = roomName.Substring(0, lastIndex);
            // 방 이름, 지역, 방장, 인원 수
            partyInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = roomName;
            // 지역

            // 선택된 던전 번호를 가져옴
            int selectedDungeonNumber = roomlist[i].CustomProperties["dungeonIndex"]==null? 1 : (int)roomlist[i].CustomProperties["dungeonIndex"];

            // 선택된 던전 번호에 따라 다른 텍스트를 설정
            string dungeonName = selectedDungeonNumber switch
            {
                1 => "깊은 숲",
                2 => "잊혀진 신전",
                3 => "별의 조각 평원",
                _ => "알 수 없는 던전입니다."
            };

            partyInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = dungeonName;
            // 방장
            partyInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (string)roomlist[i].CustomProperties["partyLeader"];
            // 인원 수
            string number = roomlist[i].PlayerCount + " / " + roomlist[i].MaxPlayers.ToString();
            partyInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = number;
            // 파티 컨테이너에 배치
            partyPrefab.transform.SetParent(gameObject.transform.Find("Party_Participation_Content_Container/Scroll View/Viewport/Content"));
        }
    }
}
