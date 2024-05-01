using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Load_Lobby_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button loadLobbyButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 로비로 이동하기 버튼 이벤트 등록
        loadLobbyButton = GetButton((int)Buttons.Load_Lobby_Button);
        AddUIEvent(loadLobbyButton.gameObject, LoadLobbyScene);
        AddUIKeyEvent(loadLobbyButton.gameObject, () => LoadLobbyScene(null), KeyCode.Return);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 로비로 이동하기 메서드
    private void LoadLobbyScene(PointerEventData data)
    {
        // 씬 이동하기 전에 모든 스택을 비움
        CloseAllPopupUI();

        // 로비 씬으로 이동
        SceneManager.LoadScene("Lobby");
    }
}
