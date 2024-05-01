using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Open_Load_Lobby_Button,
        Open_Game_Exit_Button,
        Cancel_Button,
        Save_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button openLoadLobbyButton;
    private Button openGameExitButton;
    private Button cancelButton;
    private Button saveButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 로비로 돌아가기 Popup UI 열기 버튼 이벤트 등록
        openLoadLobbyButton = GetButton((int)Buttons.Open_Load_Lobby_Button);
        AddUIEvent(openLoadLobbyButton.gameObject, OpenLoadLobbyScene);

        // 게임 종료 Popup UI 열기 버튼 이벤트 등록
        openGameExitButton = GetButton((int)Buttons.Open_Game_Exit_Button);
        AddUIEvent(openGameExitButton.gameObject, OpenGameExit);

        // 돌아가기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 저장하기 버튼 이벤트 등록
        saveButton = GetButton((int)Buttons.Save_Button);
        AddUIEvent(saveButton.gameObject, Save);
        AddUIKeyEvent(saveButton.gameObject, () => Save(null), KeyCode.Return);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 돌아가기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 저장하기 메서드
    private void Save(PointerEventData data)
    {
        // TODO: 변경사항 저장하는 코드 필요
        ClosePopupUI();
    }

    // 로비로 돌아가기 Popup UI 띄우기 메서드
    private void OpenLoadLobbyScene(PointerEventData data)
    {
        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Load_Lobby_Popup_UI>("[Dungeon]_Load_Lobby_Popup_UI");
    }

    // 게임 종료 Popup UI 띄우기 메서드
    private void OpenGameExit(PointerEventData data)
    {
        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Game_Exit_Popup_UI>("[Common]_Game_Exit_Popup_UI");
    }
}
