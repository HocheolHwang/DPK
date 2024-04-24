using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        AddUIEvent(gameStartButton.gameObject, LoadLobbyScene);

        // 게임 종료 버튼 이벤트 등록
        Button gameExitButton = GetButton((int)Buttons.Game_Exit_Button);
        AddUIEvent(gameExitButton.gameObject, GameExit);
    }

    private void LoadLobbyScene(PointerEventData data)
    {
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
