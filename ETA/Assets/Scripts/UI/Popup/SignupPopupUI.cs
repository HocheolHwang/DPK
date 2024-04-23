using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SignupPopupUI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        SignupButton,
        SwitchLoginButton
    }

    // 회원가입 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

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
        // 현재 Popup UI를 닫음
        ClosePopupUI();

        Debug.Log("로그인 창으로 변경!");

        // 로그인 Popup UI를 띄움
        Managers.UI.ShowPopupUI<LoginPopupUI>("[Login] Login Popup UI");
    }
}
