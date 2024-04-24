using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Login_Popup_UI : UI_Popup
{
    // 텍스트 인덱스
    enum Texts
    {
        User_ID_Text,
        User_PW_Text
    }

    // 버튼 인덱스
    enum Buttons
    {
        Login_Button,
        Switch_Signup_Button
    }
    
    // 클래스 멤버 변수로 선언
    private TextMeshProUGUI userID;
    private TextMeshProUGUI userPW;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // 로그인 입력 정보
        userID = GetText((int)Texts.User_ID_Text);
        userPW = GetText((int)Texts.User_PW_Text);

        // 로그인 시도 버튼 이벤트 등록
        Button loginButton = GetButton((int)Buttons.Login_Button);
        AddUIEvent(loginButton.gameObject, Login);
        AddEnterKeyEvent(loginButton.gameObject, () => Login(null));

        // 회원가입 전환 버튼 이벤트 등록
        Button switchSignupButton = GetButton((int)Buttons.Switch_Signup_Button);
        AddUIEvent(switchSignupButton.gameObject, SwitchSignup);
    }

    // 로그인 시도
    private void Login(PointerEventData data)
    {
        // PlayerSignInReqDto 객체 생성
        PlayerSignInReqDto signInDto = new PlayerSignInReqDto
        {
            playerId = userID.text,
            playerPassword = userPW.text,
        };

        // NetworkManager 인스턴스에 접근
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        //Managers.Network.SignInCall(signInDto);
        if (networkManager != null)
        {
            // 로그인 요청 호출
            networkManager.SignInCall(signInDto);
            Debug.Log("로그인 요청 호출~");
            Debug.Log($"아이디: {signInDto.playerId}");
            Debug.Log($"비번: {signInDto.playerPassword}");
        }
        else
        {
            Debug.LogError("NetworkManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    // 회원가입 Popup UI로 전환
    private void SwitchSignup(PointerEventData data)
    {
        // 현재 Popup UI를 닫음
        ClosePopupUI();

        // 회원가입 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Signup_Popup_UI>("[Login]_Signup_Popup_UI");
    }
}
