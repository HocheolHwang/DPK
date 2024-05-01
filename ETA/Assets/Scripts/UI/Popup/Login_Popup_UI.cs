using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Login_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Login_Button,
        Switch_Signup_Button,
        Game_Exit_Button
    }
    enum InputFields
    {
        ID_InputField,
        PW_InputField
    }

    enum Texts
    {
        Warning_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button loginButton;
    private Button switchSignupButton;
    private Button gameExitButton;
    private TMP_InputField userID;
    private TMP_InputField userPW;
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

        // 로그인 시도 버튼 이벤트 등록
        loginButton = GetButton((int)Buttons.Login_Button);
        AddUIEvent(loginButton.gameObject, Login);
        AddUIKeyEvent(loginButton.gameObject, () => Login(null), KeyCode.Return);

        // 회원가입 전환 버튼 이벤트 등록
        switchSignupButton = GetButton((int)Buttons.Switch_Signup_Button);
        AddUIEvent(switchSignupButton.gameObject, SwitchSignup);

        // 게임 종료 버튼 이벤트 등록
        gameExitButton = GetButton((int)Buttons.Game_Exit_Button);
        AddUIEvent(gameExitButton.gameObject, OpenGameExit);
        AddUIKeyEvent(gameExitButton.gameObject, () => OpenGameExit(null), KeyCode.Escape);

        // 로그인 입력 정보
        userID = Get<TMP_InputField>((int)InputFields.ID_InputField);
        userPW = Get<TMP_InputField>((int)InputFields.PW_InputField);

        // 경고 문구
        warning = GetText((int)Texts.Warning_Text);
    }


    // ------------------------------ 유니티 생명주기 메서드 ------------------------------

    void Update()
    {
        // 비밀번호에 한글 입력시 자동으로 영어로 변환
        if (userPW.isFocused)
        {
            Input.imeCompositionMode = IMECompositionMode.Off;
        }
        else
        {
            Input.imeCompositionMode = IMECompositionMode.On;
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 로그인 시도 메서드
    private void Login(PointerEventData data)
    {
        PlayerResDto dto = new PlayerResDto();
        // 입력 필드 검증
        if (string.IsNullOrEmpty(userID.text))
        {
            dto.message = "아이디를 입력해주세요.";
            UpdateWarningText(dto);
            return;
        }
        if (string.IsNullOrEmpty(userPW.text))
        {
            dto.message = "비밀번호를 입력해주세요.";
            UpdateWarningText(dto);
            return;
        }

        PlayerSignInReqDto signInDto = new PlayerSignInReqDto
        {
            playerId = userID.text,
            playerPassword = userPW.text,
        };

        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager != null)
        {
            // 로그인 요청 호출 및 콜백 함수 전달
            networkManager.SignInCall(signInDto, UpdateWarningText);
        }
        else
        {
            Debug.LogError("NetworkManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    // 로그인 시도 후 콜백 함수로 경고 텍스트 업데이트하는 메서드
    private void UpdateWarningText(PlayerResDto dto)
    {
        string message = dto.message;
        // 메시지가 "success"인 경우 After Login Popup UI 띄움
        if (message == "success")
        {
            Managers.Player.SetToken(dto.accessToken);
            Managers.Player.SetGold(dto.playerGold);
            Managers.Player.SetFirst(dto.first);
            Managers.Player.SetNickName(JWTDecord.DecodeJWT(dto.accessToken));

            // 모든 Popup UI를 닫음
            CloseAllPopupUI();

            // 로그인 완료 Popup UI를 띄움
            Managers.UI.ShowPopupUI<After_Login_Popup_UI>("[Login]_After_Login_Popup_UI");
        }
        else if (message == "Database error.")
        {
            warning.text = "아이디를 확인해주세요.";
        }
        else
        {
            //warning.text = message;            
            warning.text = "아이디와 비밀번호를 확인해주세요.";
        }
    }

    // 회원가입 Popup UI로 전환하는 메서드
    private void SwitchSignup(PointerEventData data)
    {
        // 현재 Popup UI를 닫음
        ClosePopupUI();

        // 회원가입 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Signup_Popup_UI>("[Login]_Signup_Popup_UI");
    }

    // 게임 종료 Popup UI 띄우기 메서드
    private void OpenGameExit(PointerEventData data)
    {
        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Game_Exit_Popup_UI>("[Common]_Game_Exit_Popup_UI");
    }
}
