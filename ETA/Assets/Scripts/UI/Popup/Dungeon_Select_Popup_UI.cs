using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Dungeon_Select_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Open_Party_Join_Button,
        Open_Party_Leave_Button,
        Open_Dungeon_Select_Button,
        Open_Dungeon_Enter_Button,
        Select_DeepForest_Button,
        Select_ForgottenTemple_Button,
        Select_StarShardPlain_Button,
        Cancel_Button,
        Dungeon_Select_Button
    }

    enum Texts
    {
        Selected_Dungeon_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button openPartyJoinButton;
    private Button openPartyLeaveButton;
    private Button openDungeonSelectButton;
    private Button openDungeonEnterButton;
    private Button selectDeepForestButton;
    private Button selectForgottenTempleButton;
    private Button selectStarShardPlainButton;
    private Button cancelButton;
    private Button dungeonSelectButton;
    private TextMeshProUGUI selectedDungeonText;

    // 현재 던전 정보 변수
    private int existingDungeonNumber;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // 현재 던전 정보를 저장
        existingDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);

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

        // 깊은 숲 선택 버튼 이벤트 등록
        selectDeepForestButton = GetButton((int)Buttons.Select_DeepForest_Button);
        AddUIEvent(selectDeepForestButton.gameObject, SelectDeepForest);

        // 잊혀진 신전 선택 버튼 이벤트 등록
        selectForgottenTempleButton = GetButton((int)Buttons.Select_ForgottenTemple_Button);
        // AddUIEvent(selectForgottenTempleButton.gameObject, SelectForgottenTemple);

        // 별의 조각 평원 선택 버튼 이벤트 등록
        selectStarShardPlainButton = GetButton((int)Buttons.Select_StarShardPlain_Button);
        // AddUIEvent(selectStarShardPlainButton.gameObject, SelectStarShardPlain);

        // 돌아가기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 던전 선택 버튼 이벤트 등록
        dungeonSelectButton = GetButton((int)Buttons.Dungeon_Select_Button);
        AddUIEvent(dungeonSelectButton.gameObject, DungeonSelect);
        AddUIKeyEvent(dungeonSelectButton.gameObject, () => DungeonSelect(null), KeyCode.Return);

        // 선택된 던전
        selectedDungeonText = GetText((int)Texts.Selected_Dungeon_Text);
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

    // 파티 탈퇴 Popup UI 띄우기 메서드
    private void OpenPartyLeave(PointerEventData data)
    {
        // 파티 탈퇴 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Party_Leave_Popup_UI>("[Lobby]_Party_Leave_Popup_UI");
    }

    // 던전 선택 Popup UI 띄우기 메서드
    private void OpenDungeonSelect(PointerEventData data)
    {
        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 던전 선택 Popup UI를 띄움 (새로고침)
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

    // 깊은 숲 선택 메서드
    private void SelectDeepForest(PointerEventData data)
    {
        PlayerPrefs.SetInt("SelectedDungeonNumber", 1);
        PlayerPrefs.Save();
        UpdateSelectedDungeon();
    }

    // 잊혀진 신전 선택 메서드
    private void SelectForgottenTemple(PointerEventData data)
    {
        PlayerPrefs.SetInt("SelectedDungeonNumber", 2);
        PlayerPrefs.Save();
        UpdateSelectedDungeon();
    }

    // 별의 조각 평원 선택 메서드
    private void SelectStarShardPlain(PointerEventData data)
    {
        PlayerPrefs.SetInt("SelectedDungeonNumber", 3);
        PlayerPrefs.Save();
        UpdateSelectedDungeon();
    }

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        // 취소할 경우 기존 선택된 던전 정보로 저장
        PlayerPrefs.SetInt("SelectedDungeonNumber", existingDungeonNumber);
        PlayerPrefs.Save();

        // 파티 참가 Popup UI를 닫음
        ClosePopupUI();

        // 로비 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }

    // 저장하기 메서드
    private void DungeonSelect(PointerEventData data)
    {
        // 파티 참가 Popup UI를 닫음
        ClosePopupUI();

        // 로비 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }

    // 선택된 던전 업데이트 메서드
    private void UpdateSelectedDungeon()
    {
        // 선택된 던전 번호를 가져옴
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);

        // 선택된 던전 번호에 따라 다른 텍스트를 설정
        selectedDungeonText.text = selectedDungeonNumber switch
        {
            1 => "선택된 던전: [깊은 숲]",
            2 => "선택된 던전: [잊혀진 신전]",
            3 => "선택된 던전: [별의 조각 평원]",
            _ => "알 수 없는 던전입니다."
        };
    }
}
