using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginFixedUI : UI_Fixed
{
    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 로그인 시도 버튼 이벤트 등록
        GameObject loginButton = GetObject((int)Buttons.LoginButton);
        UI_Base.AddUIEvent(loginButton, OnLoginClicked);

        // 회원가입 전환 버튼 이벤트 등록
        GameObject switchSignupButton = GetObject((int)Buttons.SwitchSignupButton);
        UI_Base.AddUIEvent(switchSignupButton, OnSwitchSignupClicked);
    }

    // 로그인 시도 버튼 클릭 이벤트 핸들러
    private void OnLoginClicked(PointerEventData data)
    {
        // 로그인 처리...
        Debug.Log("로그인 시도");
    }

    // 회원가입 전환 버튼 클릭 이벤트 핸들러
    private void OnSwitchSignupClicked(PointerEventData data)
    {
        // 회원가입 Fixed UI를 띄움
        Managers.UI.ShowFixedUI<SignupFixedUI>("Signup Fixed UI");
    }

    // 버튼 인덱스
    enum Buttons
    {
        LoginButton,
        SwitchSignupButton
    }
}