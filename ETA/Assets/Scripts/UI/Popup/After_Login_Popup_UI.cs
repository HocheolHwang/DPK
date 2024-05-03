using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class After_Login_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Game_Start_Button,
        Open_Game_Exit_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button gameStartButton;
    private Button openGameExitButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 게임 시작 버튼 이벤트 등록
        gameStartButton = GetButton((int)Buttons.Game_Start_Button);
        AddUIEvent(gameStartButton.gameObject, GameStart);
        AddUIKeyEvent(gameStartButton.gameObject, () => GameStart(null), KeyCode.Return);

        // 게임 종료 Popup UI 띄우기 버튼 이벤트 등록
        openGameExitButton = GetButton((int)Buttons.Open_Game_Exit_Button);
        AddUIEvent(openGameExitButton.gameObject, OpenGameExit);
        AddUIKeyEvent(openGameExitButton.gameObject, () => OpenGameExit(null), KeyCode.Escape);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 게임 시작 메서드
    private void GameStart(PointerEventData data)
    {
        // Scene 이동 전에 모든 스택을 비움
        CloseAllPopupUI();

        if (Managers.Player.GetFirst())
        {
            // 첫 로그인 O: 선택된 던전을 0으로 저장
            PlayerPrefs.SetInt("SelectedDungeonNumber", 0);
            PlayerPrefs.Save();

            // 첫 로그인 여부 1로 저장  
            PlayerPrefs.SetInt("FirstLogin", 1);
            PlayerPrefs.Save();

            // 튜토리얼 Scene으로 이동
            
            Managers.Scene.LoadScene(Define.Scene.Tutorial);
        }
        else
        {
            // 첫 로그인 X: 선택된 던전을 1로 저장
            PlayerPrefs.SetInt("SelectedDungeonNumber", 1);
            PlayerPrefs.Save();

            // 첫 로그인 여부 0으로 저장
            PlayerPrefs.SetInt("FirstLogin", 0);
            PlayerPrefs.Save();

            // 로비 Scene으로 이동
            Managers.Scene.LoadScene(Define.Scene.Lobby);
        }
    }

    // 게임 종료 Popup UI 띄우기 메서드
    private void OpenGameExit(PointerEventData data)
    {
        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Game_Exit_Popup_UI>("[Common]_Game_Exit_Popup_UI");
    }
}
