using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load_Lobby_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button,
        Load_Lobby_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button loadLobbyButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 로비로 돌아가기 버튼 이벤트 등록
        loadLobbyButton = GetButton((int)Buttons.Load_Lobby_Button);
        AddUIEvent(loadLobbyButton.gameObject, LoadLobby);
        AddUIKeyEvent(loadLobbyButton.gameObject, () => LoadLobby(null), KeyCode.Return);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 로비로 돌아가기 메서드
    private void LoadLobby(PointerEventData data)
    {
        // Scene 이동 전에 모든 스택을 비움
        CloseAllPopupUI();

        // 로비 Scene으로 이동
        //SceneManager.LoadScene("Lobby");
        Managers.Scene.LoadScene(Define.Scene.Lobby);
    }
}
