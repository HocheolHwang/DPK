using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignupFixedUI : UI_Fixed
{
    // 회원가입 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 회원가입 시도 버튼 이벤트 등록
        GameObject signupButton = GetObject((int)Buttons.SignupButton);
        UI_Base.AddUIEvent(signupButton, OnSignupClicked);

        // 로그인 전환 버튼 이벤트 등록
        GameObject switchLoginButton = GetObject((int)Buttons.SwitchLoginButton);
        UI_Base.AddUIEvent(switchLoginButton, OnSwitchLoginClicked);
    }

    // 회원가입 시도 버튼 클릭 이벤트 핸들러
    private void OnSignupClicked(PointerEventData data)
    {
        // 회원가입 처리...
        Debug.Log("회원가입 시도");
    }

    // 로그인 전환 버튼 클릭 이벤트 핸들러
    private void OnSwitchLoginClicked(PointerEventData data)
    {
        // 로그인 Fixed UI를 띄움
        Managers.UI.ShowFixedUI<LoginFixedUI>("Login Fixed UI");
    }

    // 버튼 인덱스
    enum Buttons
    {
        SignupButton,
        SwitchLoginButton
    }
}
