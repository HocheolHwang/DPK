using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Signup_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Signup_Button,
        Switch_Login_Button,
        Game_Exit_Button
    }

    enum InputFields
    {
        ID_InputField,
        Nickname_InputField,
        PW_InputField,
        PW_Check_InputField
    }

    enum Texts
    {
        Warning_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button signupButton;
    private Button switchLoginButton;
    private Button gameExitButton;
    private TMP_InputField userID;
    private TMP_InputField userNickname;
    private TMP_InputField userPW;
    private TMP_InputField userPWCheck;
    private TextMeshProUGUI warning;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 회원가입 시도 버튼 이벤트 등록
        signupButton = GetButton((int)Buttons.Signup_Button);
        AddUIEvent(signupButton.gameObject, Signup);
        AddUIKeyEvent(signupButton.gameObject, () => Signup(null), KeyCode.Return);

        // 로그인 전환 버튼 이벤트 등록
        switchLoginButton = GetButton((int)Buttons.Switch_Login_Button);
        AddUIEvent(switchLoginButton.gameObject, SwitchLogin);

        // 게임 종료 버튼 이벤트 등록
        gameExitButton = GetButton((int)Buttons.Game_Exit_Button);
        AddUIEvent(gameExitButton.gameObject, OpenGameExit);
        AddUIKeyEvent(gameExitButton.gameObject, () => OpenGameExit(null), KeyCode.Escape);

        // 회원가입 입력 정보
        userID = Get<TMP_InputField>((int)InputFields.ID_InputField);
        userNickname = Get<TMP_InputField>((int)InputFields.Nickname_InputField);
        userPW = Get<TMP_InputField>((int)InputFields.PW_InputField);
        userPWCheck = Get<TMP_InputField>((int)InputFields.PW_Check_InputField);

        // 경고 문구
        warning = GetText((int)Texts.Warning_Text);
    }


    // ------------------------------ 유니티 생명주기 메서드 ------------------------------

    void Update()
    {
        // 비밀번호에 한글 입력시 자동으로 영어로 변환
        if (userPW.isFocused || userPWCheck.isFocused)
        {
            Input.imeCompositionMode = IMECompositionMode.Off;
        }
        else
        {
            Input.imeCompositionMode = IMECompositionMode.On;
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 회원가입 시도 메서드
    private void Signup(PointerEventData data)
    {
        // 입력 필드 검증
        if (string.IsNullOrEmpty(userID.text))
        {
            UpdateWarningText("아이디를 입력해주세요.");
            return;
        }
        if (string.IsNullOrEmpty(userNickname.text))
        {
            UpdateWarningText("닉네임을 입력해주세요.");
            return;
        }
        if (string.IsNullOrEmpty(userPW.text))
        {
            UpdateWarningText("비밀번호를 입력해주세요.");
            return;
        }
        if (string.IsNullOrEmpty(userPWCheck.text))
        {
            UpdateWarningText("비밀번호 확인을 입력해주세요.");
            return;
        }

        PlayerSignUpReqDto signUpDto = new PlayerSignUpReqDto
        {
            playerId = userID.text,
            nickname = userNickname.text,
            playerPassword = userPW.text,
            playerPasswordCheck = userPWCheck.text,
        };

        Managers.Network.SignUpCall(signUpDto, UpdateWarningText);
    }

    // 회원가입 시도 후 콜백 함수로 경고 텍스트 업데이트하는 메서드
    private void UpdateWarningText(string message)
    {
        // 메시지가 "success"인 경우 회원가입 완료 Popup UI 띄움
        if (message == "success")
        {
            // 회원가입 완료 Popup UI를 띄움
            Managers.UI.ShowPopupUI<Signup_Success_Popup_UI>("[Login]_Signup_Success_Popup_UI");
        }
        else
        {
            // 회원가입 실패 시 경고 메세지를 업데이트
            warning.text = message;
        }
    }

    // 로그인 Popup UI로 전환하는 메서드
    private void SwitchLogin(PointerEventData data)
    {
        // 현재 Popup UI를 닫음
        ClosePopupUI();

        // 로그인 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Login_Popup_UI>("[Login]_Login_Popup_UI");
    }

    // 게임 종료 Popup UI 띄우기 메서드
    private void OpenGameExit(PointerEventData data)
    {
        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Game_Exit_Popup_UI>("[Common]_Game_Exit_Popup_UI");
    }
}
