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
    private int selectedDungeonNumber = 1;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // 선택된 던전
        selectedDungeonText = GetText((int)Texts.Selected_Dungeon_Text);
        selectedDungeonText.text = "선택된 던전: 깊은 숲";

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
        selectedDungeonNumber = 1;
        selectedDungeonText.text = "선택된 던전: 깊은 숲";
    }

    // 잊혀진 신전 선택
    private void SelectForgottenTemple(PointerEventData data)
    {
        selectedDungeonNumber = 2;
        selectedDungeonText.text = "선택된 던전: 잊혀진 신전";
    }

    // 별의 조각 평원 선택
    private void SelectStarShardPlain(PointerEventData data)
    {
        selectedDungeonNumber = 3;
        selectedDungeonText.text = "선택된 던전: 별의 조각 평원";
    }

    // 돌아가기
    private void Cancel(PointerEventData data)
    {
        // 파티 참가 Popup UI를 닫은 뒤 로비 Popup UI를 띄움
        ClosePopupUI();
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }

    // 던전 선택
    private void DungeonSelect(PointerEventData data)
    {
        Debug.Log($"니가 고른 던전은? {selectedDungeonNumber}");

        // decidedDungeonNumber 값을 PlayerPrefs에 저장
        PlayerPrefs.SetInt("SelectedDungeonNumber", selectedDungeonNumber);
        PlayerPrefs.Save();

        // ClosePopupUI();
    }
}
