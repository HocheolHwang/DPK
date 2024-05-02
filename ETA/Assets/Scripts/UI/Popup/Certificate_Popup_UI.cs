using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Certificate_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Get_Certificate_Button
    }

    enum Texts
    {
        Nickname_Text,
        Date_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button getCertificateButton;
    private TextMeshProUGUI nicknameText;
    private TextMeshProUGUI dateText;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 획득하기 버튼 이벤트 등록
        getCertificateButton = GetButton((int)Buttons.Get_Certificate_Button);
        AddUIEvent(getCertificateButton.gameObject, GetCertificate);
        AddUIKeyEvent(getCertificateButton.gameObject, () => GetCertificate(null), KeyCode.Return);

        // 유저 닉네임 및 오늘 날짜를 표시
        nicknameText = GetText((int)Texts.Nickname_Text);
        dateText = GetText((int)Texts.Date_Text);
        UpdateUserInfo();
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 획득하기 메서드
    private void GetCertificate(PointerEventData data)
    {
        // 자격증 Popup UI를 닫음
        ClosePopupUI();

        // 로비 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }

    // 유저 닉네임 및 오늘 날짜를 업데이트하는 메서드
    private void UpdateUserInfo()
    {
        nicknameText.text = Managers.Player.GetNickName();
        dateText.text = System.DateTime.Now.ToString("yyyy.MM.dd");
    }
}
