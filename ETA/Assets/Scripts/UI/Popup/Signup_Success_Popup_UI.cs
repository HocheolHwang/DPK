using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Signup_Success_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Signup_Success_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<Button>(typeof(Buttons));

        // 로그인 시도 버튼 이벤트 등록
        Button signupSuccessButton = GetButton((int)Buttons.Signup_Success_Button);
        AddUIEvent(signupSuccessButton.gameObject, SignupSuccess);
        AddEnterKeyEvent(signupSuccessButton.gameObject, () => SignupSuccess(null));
    }

    // 회원가입 Popup UI로 전환
    private void SignupSuccess(PointerEventData data)
    {
        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 로그인 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Before_Login_Popup_UI>("[Login]_Before_Login_Popup_UI");
        Managers.UI.ShowPopupUI<Login_Popup_UI>("[Login]_Login_Popup_UI");
    }
}
