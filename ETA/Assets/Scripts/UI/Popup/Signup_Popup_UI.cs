using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class Signup_Popup_UI : UI_Popup
{
    // 텍스트 인덱스
    enum Texts
    {
        Warning_Text,
        User_ID_Text,
        User_Nickname_Text,
        User_PW_Text,
        User_PW_Check_Text
    }

    // 버튼 인덱스
    enum Buttons
    {
        Signup_Button,
        Switch_Login_Button
    }

    // 클래스 멤버 변수로 선언
    private TextMeshProUGUI warning;
    private TextMeshProUGUI userID;
    private TextMeshProUGUI userNickname;
    private TextMeshProUGUI userPW;
    private TextMeshProUGUI userPWCheck;

    // 회원가입 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // 경고 문구
        warning = GetText((int)Texts.Warning_Text);

        // 회원가입 입력 정보
        userID = GetText((int)Texts.User_ID_Text);
        userNickname = GetText((int)Texts.User_Nickname_Text);
        userPW = GetText((int)Texts.User_PW_Text);
        userPWCheck = GetText((int)Texts.User_PW_Check_Text);

        // 회원가입 시도 버튼 이벤트 등록
        Button signupButton = GetButton((int)Buttons.Signup_Button);
        AddUIEvent(signupButton.gameObject, Signup);
        AddEnterKeyEvent(signupButton.gameObject, () => Signup(null));

        // 로그인 전환 버튼 이벤트 등록
        Button switchLoginButton = GetButton((int)Buttons.Switch_Login_Button);
        AddUIEvent(switchLoginButton.gameObject, SwitchLogin);
    }

    // 회원가입 시도
    private void Signup(PointerEventData data)
    {
        PlayerSignUpReqDto signUpDto = new PlayerSignUpReqDto
        {
            playerId = userID.text,
            nickname = userNickname.text,
            playerPassword = userPW.text,
            playerPasswordCheck = userPWCheck.text,
        };

        // 비밀번호와 비밀번호 확인 미일치 시 경고 문구 출력 및 회원가입 종료
        if (signUpDto.playerPassword != signUpDto.playerPasswordCheck)
        {
            warning.text = "비밀번호가 일치하지 않습니다.";
            return;
        }

        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        //Managers.Network.SignInCall(signInDto);
        if (networkManager != null)
        {
            // 로그인 요청 호출
            networkManager.SignUpCall(signUpDto);
            Debug.Log("회원가입 요청 호출~");
            Debug.Log($"아이디: {signUpDto.playerId}");
            Debug.Log($"닉네임: {signUpDto.nickname}");
            Debug.Log($"비번: {signUpDto.playerPassword}");
            Debug.Log($"비번체크: {signUpDto.playerPasswordCheck}");
        }
        else
        {
            Debug.LogError("NetworkManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    // 로그인 Popup UI로 전환
    private void SwitchLogin(PointerEventData data)
    {
        // 현재 Popup UI를 닫음
        ClosePopupUI();

        // 로그인 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Login_Popup_UI>("[Login]_Login_Popup_UI");
    }
}
