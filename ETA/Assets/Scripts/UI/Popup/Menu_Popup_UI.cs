using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Cancel_Button,
        Load_Lobby_Button,
        Game_Exit_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

        // 남아 있기 버튼 이벤트 등록
        Button cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 로비로 이동하기 버튼 이벤트 등록
        Button loadLobbyButton = GetButton((int)Buttons.Load_Lobby_Button);
        AddUIEvent(loadLobbyButton.gameObject, LoadLobbyScene);
        AddUIKeyEvent(loadLobbyButton.gameObject, () => LoadLobbyScene(null), KeyCode.Return);

        // 게임 종료 버튼 이벤트 등록
        Button gameExitButton = GetButton((int)Buttons.Game_Exit_Button);
        AddUIEvent(gameExitButton.gameObject, GameExit);
        AddUIKeyEvent(gameExitButton.gameObject, () => GameExit(null), KeyCode.Return);
    }

    // 남아 있기
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 로비로 이동하기
    private void LoadLobbyScene(PointerEventData data)
    {
        // 씬 이동하기 전에 모든 스택을 비움
        CloseAllPopupUI();

        // 로비로 이동
        SceneManager.LoadScene("Lobby");
    }

    // 게임 종료
    private void GameExit(PointerEventData data)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
