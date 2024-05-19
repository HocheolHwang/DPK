using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game_Exit_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button,
        Game_Exit_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button gameExitButton;


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

        // 게임 종료 버튼 이벤트 등록
        gameExitButton = GetButton((int)Buttons.Game_Exit_Button);
        AddUIEvent(gameExitButton.gameObject, GameExit);
        AddUIKeyEvent(gameExitButton.gameObject, () => GameExit(null), KeyCode.Return);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 게임 종료 메서드
    private void GameExit(PointerEventData data)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
