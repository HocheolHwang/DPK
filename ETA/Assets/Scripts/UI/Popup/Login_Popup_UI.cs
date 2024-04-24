using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Login_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Login_Button,
        Switch_Signup_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

        // 로그인 시도 버튼 이벤트 등록
        Button loginButton = GetButton((int)Buttons.Login_Button);
        AddUIEvent(loginButton.gameObject, Login);

        // 회원가입 전환 버튼 이벤트 등록
        Button switchSignupButton = GetButton((int)Buttons.Switch_Signup_Button);
        AddUIEvent(switchSignupButton.gameObject, SwitchSignup);
    }

    // 로그인 시도 버튼 클릭 이벤트 핸들러
    private void Login(PointerEventData data)
    {
        // 로그인 처리...
        Debug.Log("로그인 시도");
    }

    // 회원가입 전환 버튼 클릭 이벤트 핸들러
    private void SwitchSignup(PointerEventData data)
    {
        // 현재 Popup UI를 닫음
        ClosePopupUI();

        // 회원가입 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Signup_Popup_UI>("[Login]_Signup_Popup_UI");
    }
}
