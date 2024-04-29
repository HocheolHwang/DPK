using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Result_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Load_Lobby_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

        // 로비로 이동하기 버튼 이벤트 등록
        Button loadLobbyButton = GetButton((int)Buttons.Load_Lobby_Button);
        AddUIEvent(loadLobbyButton.gameObject, LoadLobbyScene);
        AddUIKeyEvent(loadLobbyButton.gameObject, () => LoadLobbyScene(null), KeyCode.Return);
    }

    // 로비로 이동
    private void LoadLobbyScene(PointerEventData data)
    {
        // 씬 이동하기 전에 모든 스택을 비움
        CloseAllPopupUI();

        // 로비 씬으로 이동
        SceneManager.LoadScene("Lobby");
    }
}
