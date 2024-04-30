using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class After_Login_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Game_Start_Button,
        Game_Exit_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

        // 게임 시작 버튼 이벤트 등록
        Button gameStartButton = GetButton((int)Buttons.Game_Start_Button);
        AddUIEvent(gameStartButton.gameObject, LoadNextScene);
        AddUIKeyEvent(gameStartButton.gameObject, () => LoadNextScene(null), KeyCode.Return);

        // 게임 종료 버튼 이벤트 등록
        Button gameExitButton = GetButton((int)Buttons.Game_Exit_Button);
        AddUIEvent(gameExitButton.gameObject, OpenGameExit);
        AddUIKeyEvent(gameExitButton.gameObject, () => OpenGameExit(null), KeyCode.Escape);
    }

    // 로비로 이동
    private void LoadNextScene(PointerEventData data)
    {
        // 씬 이동하기 전에 모든 스택을 비움
        CloseAllPopupUI();

        if (Managers.Player.GetFirst())
        {
            // 첫 번째 로그인일 경우 선택된 던전을 0번으로 저장
            PlayerPrefs.SetInt("SelectedDungeonNumber", 0);
            PlayerPrefs.Save();

            // 튜토리얼 Scene으로 이동
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            // 첫 번째 로그인이 아닐 경우 선택된 던전을 1번으로 저장
            PlayerPrefs.SetInt("SelectedDungeonNumber", 1);
            PlayerPrefs.Save();

            // 바로 로비 Scene으로 이동
            SceneManager.LoadScene("Lobby");
        }
    }

    // 게임 종료
    private void OpenGameExit(PointerEventData data)
    {
        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Game_Exit_Popup_UI>("[Common]_Game_Exit_Popup_UI");
    }
}
