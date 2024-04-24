using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Signup_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Signup_Button,
        Switch_Login_Button
    }

    // 회원가입 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

        // 회원가입 시도 버튼 이벤트 등록
        Button signupButton = GetButton((int)Buttons.Signup_Button);
        AddUIEvent(signupButton.gameObject, Signup);

        // 로그인 전환 버튼 이벤트 등록
        Button switchLoginButton = GetButton((int)Buttons.Switch_Login_Button);
        AddUIEvent(switchLoginButton.gameObject, SwitchLogin);
    }

    // 회원가입 시도 버튼 클릭 이벤트 핸들러
    private void Signup(PointerEventData data)
    {
        // 회원가입 처리...
        Debug.Log("회원가입 시도");
    }

    // 로그인 전환 버튼 클릭 이벤트 핸들러
    private void SwitchLogin(PointerEventData data)
    {
        // 현재 Popup UI를 닫음
        ClosePopupUI();

        // 로그인 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Login_Popup_UI>("[Login]_Login_Popup_UI");
    }
}
