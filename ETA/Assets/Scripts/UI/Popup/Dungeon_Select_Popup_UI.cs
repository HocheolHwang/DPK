using System.Collections;
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
        Select_StarShardPlain_Button,
        Select_ForgottenTemple_Button,
        Select_SeaOfAbyss_Button,
        Cancel_Button,
        Dungeon_Select_Button
    }

    enum Texts
    {
        Selected_Dungeon_Text,
        Selected_Dungeon_Detail_Text
    }

    enum Images
    {
        StarShardPlain_Unselected,
        ForgottenTemple_Unselected,
        SeaOfAbyss_Unselected,
        Selected_Dungeon_Background
    }

    // UI 컴포넌트 바인딩 변수
    private Button openPartyJoinButton;
    private Button openPartyLeaveButton;
    private Button openDungeonSelectButton;
    private Button openDungeonEnterButton;
    private Button selectStarShardPlainButton;
    private Button selectForgottenTempleButton;
    private Button selectSeaOfAbyssButton;
    private Button cancelButton;
    private Button dungeonSelectButton;
    private TextMeshProUGUI selectedDungeonText;
    private TextMeshProUGUI selectedDungeonDetailText;
    private Image[] dungeonUnselecteds = new Image[3];
    private Image selectedDungeonBackground;

    // 현재 던전 정보 변수
    private int existingDungeonNumber;

    // 선택된 던전 번호
    private int selectedDungeonNumber;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        // 현재 던전 정보를 저장
        existingDungeonNumber = FindObjectOfType<Lobby_Scene>().currentDungeonNumber;

        // 파티 참가 Popup UI 띄우기 버튼 이벤트 등록
        openPartyJoinButton = GetButton((int)Buttons.Open_Party_Join_Button);
        AddUIEvent(openPartyJoinButton.gameObject, OpenPartyJoin);

        // 파티 탈퇴 Popup UI 띄우기 버튼 이벤트 등록
        openPartyLeaveButton = GetButton((int)Buttons.Open_Party_Leave_Button);
        AddUIEvent(openPartyLeaveButton.gameObject, OpenPartyLeave);
        openPartyLeaveButton.gameObject.SetActive(false); // 임시로 비활성화

        // 던전 선택 Popup UI 띄우기 버튼 이벤트 등록
        openDungeonSelectButton = GetButton((int)Buttons.Open_Dungeon_Select_Button);
        AddUIEvent(openDungeonSelectButton.gameObject, OpenDungeonSelect);

        // 던전 입장 Popup UI 띄우기 버튼 이벤트 등록
        openDungeonEnterButton = GetButton((int)Buttons.Open_Dungeon_Enter_Button);
        AddUIEvent(openDungeonEnterButton.gameObject, OpenDungeonEnter);

        // 별의 조각 평원 선택 버튼 이벤트 등록
        selectStarShardPlainButton = GetButton((int)Buttons.Select_StarShardPlain_Button);
        AddUIEvent(selectStarShardPlainButton.gameObject, SelectStarShardPlain);

        // 잊혀진 신전 선택 버튼 이벤트 등록
        selectForgottenTempleButton = GetButton((int)Buttons.Select_ForgottenTemple_Button);
        AddUIEvent(selectForgottenTempleButton.gameObject, SelectForgottenTemple);

        // 심연의 바다 선택 버튼 이벤트 등록
        selectSeaOfAbyssButton = GetButton((int)Buttons.Select_SeaOfAbyss_Button);
        AddUIEvent(selectSeaOfAbyssButton.gameObject, SelectSeaOfAbyss);

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
        selectedDungeonDetailText = GetText((int)Texts.Selected_Dungeon_Detail_Text);
        selectedDungeonBackground = GetImage((int)Images.Selected_Dungeon_Background);

        // 던전 미선택 이미지 초기화
        for (int i = 0; i < dungeonUnselecteds.Length; i++)
        {
            dungeonUnselecteds[i] = GetImage((int)Images.StarShardPlain_Unselected + i);
        }

        // 던전 정보 업데이트 코루틴
        StartCoroutine(UpdatedDungeonInfo());
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

    // 별의 조각 평원 선택 메서드
    private void SelectStarShardPlain(PointerEventData data)
    {
        FindObjectOfType<Lobby_Scene>().currentDungeonNumber = 1;
        
        // 던전 정보 업데이트 코루틴
        StartCoroutine(UpdatedDungeonInfo());
    }

    // 잊혀진 신전 선택 메서드
    private void SelectForgottenTemple(PointerEventData data)
    {
        FindObjectOfType<Lobby_Scene>().currentDungeonNumber = 2;

        // 던전 정보 업데이트 코루틴
        StartCoroutine(UpdatedDungeonInfo());
    }

    // 심연의 바다 선택 메서드
    private void SelectSeaOfAbyss(PointerEventData data)
    {
        FindObjectOfType<Lobby_Scene>().currentDungeonNumber = 3;

        // 던전 정보 업데이트 코루틴
        StartCoroutine(UpdatedDungeonInfo());
    }

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        // 취소할 경우 기존 선택된 던전 정보로 저장
        FindObjectOfType<Lobby_Scene>().currentDungeonNumber = existingDungeonNumber;

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

    // 던전 정보 업데이트 코루틴
    private IEnumerator UpdatedDungeonInfo()
    {
        // 선택된 던전 번호를 가져옴
        selectedDungeonNumber = FindObjectOfType<Lobby_Scene>().currentDungeonNumber;

        // 선택된 던전 번호를 가져올때까지 기다림
        yield return null;

        // 참조 완료 후 UI 업데이트
        UpdateSelectedDungeon();
    }

    // 선택된 던전 업데이트 메서드
    public void UpdateSelectedDungeon()
    {
        // 던전 미선택 이미지 업데이트
        for (int i = 0; i < dungeonUnselecteds.Length; i++)
        {
            dungeonUnselecteds[i].gameObject.SetActive(i + 1 != selectedDungeonNumber);
        }

        // 선택된 던전 이미지 업데이트
        string dungeonName = selectedDungeonNumber switch
        {
            1 => "StarShardPlain",
            2 => "ForgottenTemple",
            3 => "SeaOfAbyss",
            _ => ""
        };

        selectedDungeonBackground.sprite = Resources.Load<Sprite>($"Sprites/Dungeon Select/{dungeonName} Selected");

        // 선택된 던전 번호에 따라 다른 텍스트와 상세 설명을 설정
        (selectedDungeonText.text, selectedDungeonDetailText.text) = selectedDungeonNumber switch
        {
            1 => ("별의 조각 평원",
                  "별이 쏟아진 평원을 탐험하세요. 빛나는 별의 조각들이 고대의 힘을 간직하고 있습니다.\n" +
                  "하지만 조심하세요, 평화롭게 보이는 이 평원에는 예측할 수 없는 위험이 도사리고 있습니다.\n" +
                  "용감한 모험가들만이 진정한 별의 비밀을 밝혀낼 수 있을 것입니다."),
            2 => ("잊혀진 신전",
                  "숲속 깊은 곳에 숨겨진 잊혀진 신전을 탐험하세요. 고대 문명의 유산이 잠들어 있습니다.\n" +
                  "수많은 함정과 수수께끼가 모험가를 기다리고 있으며, 신전을 지키는 수호자들도 만만치 않습니다.\n" +
                  "잊혀진 신전의 비밀을 풀고, 그 속에 숨겨진 보물을 찾아내세요."),
            3 => ("심연의 바다",
                  "어둠이 내려앉은 심연의 바다로 떠나세요. 이곳은 상상을 초월하는 생명체들의 서식지입니다.\n" +
                  "심연의 바다는 끝없는 보물과 함께, 끝없는 위험도 감추고 있습니다. 빛이 닿지 않는 곳에서,\n" +
                  "당신의 용기와 지혜가 시험될 것입니다. 심연의 비밀을 밝혀내세요."),
            _ => ("알 수 없는 던전입니다.", "알 수 없는 던전입니다. 선택한 던전 번호가 올바르지 않습니다.")
        };
    }
}
