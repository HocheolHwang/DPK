using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Certificate_Popup_UI : UI_Popup
{
    // 텍스트 인덱스
    enum Texts
    {
        Nickname_Text,
        Date_Text
    }

    // 버튼 인덱스
    enum Buttons
    {
        Enter_Button
    }

    // 변수 추가
    private TextMeshProUGUI nicknameText;
    private TextMeshProUGUI dateText;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // 텍스트
        nicknameText = GetText((int)Texts.Nickname_Text);
        dateText = GetText((int)Texts.Date_Text);
        UpdateUserInfo();

        // 확인하기 버튼 이벤트 등록
        Button enterButton = GetButton((int)Buttons.Enter_Button);
        AddUIEvent(enterButton.gameObject, Enter);
        AddUIKeyEvent(enterButton.gameObject, () => Enter(null), KeyCode.Return);
    }

    // 유저 정보 업데이트하기
    private void UpdateUserInfo()
    {
        nicknameText.text = Managers.Player.GetNickName();
        dateText.text = System.DateTime.Now.ToString("yyyy.MM.dd");
    }

    // 확인하기
    private void Enter(PointerEventData data)
    {
        // 자격증 Popup UI 닫은 후 로비 Popup UI를 띄움
        ClosePopupUI();
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }
}
