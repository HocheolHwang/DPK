using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Signup_Success_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Signup_Success_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button signupSuccessButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 로그인하러 가기 버튼 이벤트 등록
        signupSuccessButton = GetButton((int)Buttons.Signup_Success_Button);
        AddUIEvent(signupSuccessButton.gameObject, SignupSuccess);
        AddUIKeyEvent(signupSuccessButton.gameObject, () => SignupSuccess(null), KeyCode.Return);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 로그인 Popup UI로 전환하는 메서드
    private void SignupSuccess(PointerEventData data)
    {
        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 회원가입 전 Popup UI 및 로그인 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Before_Login_Popup_UI>("[Login]_Before_Login_Popup_UI");
        Managers.UI.ShowPopupUI<Login_Popup_UI>("[Login]_Login_Popup_UI");
    }
}
