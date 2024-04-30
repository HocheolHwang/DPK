using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Dungeon_Select_Popup_UI : UI_Popup
{
    // 텍스트 인덱스
    enum Texts
    {
        Selected_Dungeon_Text
    }

    // 버튼 인덱스
    enum Buttons
    {
        Open_Party_Join_Button,
        Open_Dungeon_Select_Button,
        Open_Dungeon_Enter_Button,
        Select_DeepForest_Button,
        Select_ForgottenTemple_Button,
        Select_StarShardPlain_Button,
        Cancel_Button,
        Dungeon_Select_Button
    }

    // 클래스 멤버 변수로 선언
    private TextMeshProUGUI selectedDungeonText;
    private int existingDungeonNumber;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // 선택된 던전
        selectedDungeonText = GetText((int)Texts.Selected_Dungeon_Text);
        UpdateSelectedDungeon();

        // 현재 던전 정보를 저장
        existingDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);

        // 파티 참가 Popup UI 열기 버튼 이벤트 등록
        Button openPartyJoinButton = GetButton((int)Buttons.Open_Party_Join_Button);
        AddUIEvent(openPartyJoinButton.gameObject, OpenPartyJoin);

        // 던전 선택 Popup UI 열기 버튼 이벤트 등록
        Button openDungeonSelectButton = GetButton((int)Buttons.Open_Dungeon_Select_Button);
        AddUIEvent(openDungeonSelectButton.gameObject, OpenDungeonSelect);

        // 던전 입장 Popup UI 열기 버튼 이벤트 등록
        Button openDungeonEnterButton = GetButton((int)Buttons.Open_Dungeon_Enter_Button);
        AddUIEvent(openDungeonEnterButton.gameObject, OpenDungeonEnter);

        // 깊은 숲 선택 버튼 이벤트 등록
        Button selectDeepForestButton = GetButton((int)Buttons.Select_DeepForest_Button);
        AddUIEvent(selectDeepForestButton.gameObject, SelectDeepForest);

        // 잊혀진 신전 선택 버튼 이벤트 등록
        Button selectForgottenTempleButton = GetButton((int)Buttons.Select_ForgottenTemple_Button);
        AddUIEvent(selectForgottenTempleButton.gameObject, SelectForgottenTemple);

        // 별의 조각 평원 선택 버튼 이벤트 등록
        Button selectStarShardPlainButton = GetButton((int)Buttons.Select_StarShardPlain_Button);
        AddUIEvent(selectStarShardPlainButton.gameObject, SelectStarShardPlain);

        // 돌아가기 버튼 이벤트 등록
        Button cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 던전 선택 버튼 이벤트 등록
        Button dungeonSelectButton = GetButton((int)Buttons.Dungeon_Select_Button);
        AddUIEvent(dungeonSelectButton.gameObject, DungeonSelect);
        AddUIKeyEvent(dungeonSelectButton.gameObject, () => DungeonSelect(null), KeyCode.Return);
    }

    // 선택된 던전 업데이트하기
    private void UpdateSelectedDungeon()
    {
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);
        
        // 선택된 던전 번호에 따라 다른 텍스트를 설정
        switch (selectedDungeonNumber)
        {
            case 1:
                selectedDungeonText.text = "선택된 던전: [깊은 숲]";
                break;
            case 2:
                selectedDungeonText.text = "선택된 던전: [잊혀진 신전]";
                break;
            case 3:
                selectedDungeonText.text = "선택된 던전: [별의 조각 평원]";
                break;
            default:
                selectedDungeonText.text = "알 수 없는 던전입니다.";
                break;
        }
    }

    // 파티 참가 Popup UI 열기
    private void OpenPartyJoin(PointerEventData data)
    {
        // 창을 모두 닫은 뒤에 파티 참가 Popup UI를 띄움
        CloseAllPopupUI();
        Managers.UI.ShowPopupUI<Party_Join_Popup_UI>("[Lobby]_Party_Join_Popup_UI");
    }

    // 던전 선택 Popup UI 열기
    private void OpenDungeonSelect(PointerEventData data)
    {
        // 창을 닫고 다시 여므로 새로고침 하게 됨
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

    // 깊은 숲 선택
    private void SelectDeepForest(PointerEventData data)
    {
        // 선택된 값을 PlayerPrefs에 저장
        PlayerPrefs.SetInt("SelectedDungeonNumber", 1);
        PlayerPrefs.Save();
        UpdateSelectedDungeon();
    }

    // 잊혀진 신전 선택
    private void SelectForgottenTemple(PointerEventData data)
    {
        // 선택된 값을 PlayerPrefs에 저장
        PlayerPrefs.SetInt("SelectedDungeonNumber", 2);
        PlayerPrefs.Save();
        UpdateSelectedDungeon();
    }

    // 별의 조각 평원 선택
    private void SelectStarShardPlain(PointerEventData data)
    {
        // 선택된 값을 PlayerPrefs에 저장
        PlayerPrefs.SetInt("SelectedDungeonNumber", 3);
        PlayerPrefs.Save();
        UpdateSelectedDungeon();
    }

    // 취소하기
    private void Cancel(PointerEventData data)
    {
        // 취소할 경우 기존 선택된 던전 정보로 저장
        PlayerPrefs.SetInt("SelectedDungeonNumber", existingDungeonNumber);
        PlayerPrefs.Save();

        // 파티 참가 Popup UI를 닫은 뒤 로비 Popup UI를 띄움
        ClosePopupUI();
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }

    // 저장하기 후 로비로 돌아가기
    private void DungeonSelect(PointerEventData data)
    {
        // 파티 참가 Popup UI를 닫은 뒤 로비 Popup UI를 띄움
        ClosePopupUI();
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }
}
