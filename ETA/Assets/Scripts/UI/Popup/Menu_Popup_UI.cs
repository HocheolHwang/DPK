using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Open_Setting_Button,
        Open_Load_Lobby_Button,
        Open_Game_Exit_Button,
        Cancel_Button
    }

    enum GameObjects
    {
        Setting_Panel,
        Load_Lobby_Button_Continaer
    }

    // UI 컴포넌트 바인딩 변수
    private Button openSettingButton;
    private Button openLoadLobbyButton;
    private Button openGameExitButton;
    private Button cancelButton;
    private GameObject settingPanel;
    private GameObject loadLobbyButtonContinaer;

    // 현재 Scene 상태 변수
    private bool isLobbyScene;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        // 세팅 열기 버튼 이벤트 등록
        openSettingButton = GetButton((int)Buttons.Open_Setting_Button);
        AddUIEvent(openSettingButton.gameObject, OpenSetting);

        // 로비로 돌아가기 Popup UI 열기 버튼 이벤트 등록
        openLoadLobbyButton = GetButton((int)Buttons.Open_Load_Lobby_Button);
        AddUIEvent(openLoadLobbyButton.gameObject, OpenLoadLobbyScene);

        // 게임 종료 Popup UI 열기 버튼 이벤트 등록
        openGameExitButton = GetButton((int)Buttons.Open_Game_Exit_Button);
        AddUIEvent(openGameExitButton.gameObject, OpenGameExit);

        // 닫기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 설정 패널 초기화 및 비활성화
        settingPanel = GetObject((int)GameObjects.Setting_Panel);
        settingPanel.SetActive(false);

        // 현재 Scene이 로비 Scene인지 확인
        loadLobbyButtonContinaer = GetObject((int)GameObjects.Load_Lobby_Button_Continaer);
        isLobbyScene = SceneManager.GetActiveScene().name == "Lobby";

        if (isLobbyScene)
        {
            // 로비 Scene일 경우, 로비로 돌아가기 버튼 숨김
            loadLobbyButtonContinaer.SetActive(false);
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 설정 열기 메서드
    private void OpenSetting(PointerEventData data)
    {
        settingPanel.SetActive(true);
    }

    // 로비로 돌아가기 Popup UI 띄우기 메서드
    private void OpenLoadLobbyScene(PointerEventData data)
    {
        // 세팅 닫기
        settingPanel.SetActive(false);

        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Load_Lobby_Popup_UI>("[Dungeon]_Load_Lobby_Popup_UI");
    }

    // 게임 종료 Popup UI 띄우기 메서드
    private void OpenGameExit(PointerEventData data)
    {
        // 세팅 닫기
        settingPanel.SetActive(false);

        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Game_Exit_Popup_UI>("[Common]_Game_Exit_Popup_UI");
    }

    // 닫기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }
}
