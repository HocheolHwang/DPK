using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Opening_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Skip_Button,
    }

    // UI 컴포넌트 바인딩 변수
    private Button skipButton;

    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 스킵 버튼 이벤트 등록
        skipButton = GetButton((int)Buttons.Skip_Button);
        AddUIEvent(skipButton.gameObject, GoToLogin);
        AddUIKeyEvent(skipButton.gameObject, () => GoToLogin(null), KeyCode.Return);
    }

    private void GoToLogin(PointerEventData data)
    {
        // Scene 이동 전에 모든 스택을 비움
        CloseAllPopupUI();

        Managers.Scene.LoadScene(Define.Scene.Login);
    }
}
