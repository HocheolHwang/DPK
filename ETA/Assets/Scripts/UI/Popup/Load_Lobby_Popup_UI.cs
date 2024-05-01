using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Load_Lobby_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Cancel_Button,
        Load_Lobby_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<Button>(typeof(Buttons));

        // 남아있기 버튼 이벤트 등록
        Button cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 로비로 돌아가기 버튼 이벤트 등록
        Button loadLobbyButton = GetButton((int)Buttons.Load_Lobby_Button);
        AddUIEvent(loadLobbyButton.gameObject, LoadLobby);
        AddUIKeyEvent(loadLobbyButton.gameObject, () => LoadLobby(null), KeyCode.Return);
    }

    // 남아있기
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 로비로 돌아가기
    private void LoadLobby(PointerEventData data)
    {
        // 모든 팝업 UI를 닫습니다.
        CloseAllPopupUI();

        // 로비 Scene으로 이동
        SceneManager.LoadScene("Lobby");
    }
}
