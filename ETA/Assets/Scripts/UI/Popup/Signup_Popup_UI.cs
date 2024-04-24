using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Signup_Popup_UI : UI_Popup
{
    // 텍스트 인덱스
    enum Texts
    {
        Warning_Text
    }

    // 입력 필드 인덱스
    enum InputFields
    {
        ID_InputField,
        Nickname_InputField,
        PW_InputField,
        PW_Check_InputField
    }

    // 버튼 인덱스
    enum Buttons
    {
        Signup_Button,
        Switch_Login_Button
    }

    // 클래스 멤버 변수로 선언
    private TextMeshProUGUI warning;
    private TMP_InputField userID;
    private TMP_InputField userNickname;
    private TMP_InputField userPW;
    private TMP_InputField userPWCheck;

    // 회원가입 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));

        // 경고 문구
        warning = GetText((int)Texts.Warning_Text);

        // 회원가입 입력 정보
        userID = Get<TMP_InputField>((int)InputFields.ID_InputField);
        userNickname = Get<TMP_InputField>((int)InputFields.Nickname_InputField);
        userPW = Get<TMP_InputField>((int)InputFields.PW_InputField);
        userPWCheck = Get<TMP_InputField>((int)InputFields.PW_Check_InputField);

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

        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager != null)
        {
            // 회원가입 요청 호출
            networkManager.SignUpCall(signUpDto, UpdateWarningText);
        }
        else
        {
            Debug.LogError("NetworkManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    // 회원가입 시도 후 콜백 함수로 경고 텍스트 업데이트
    private void UpdateWarningText(string message)
    {
        warning.text = message;
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
