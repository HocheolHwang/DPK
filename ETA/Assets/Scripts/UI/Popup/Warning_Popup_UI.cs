using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Warning_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button
    }

    enum Texts
    {
        Warning_Title_Text,
        Warning_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private TextMeshProUGUI warningTitleText;
    private TextMeshProUGUI warningText;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 덛기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 경고 텍스트 초기화
        warningTitleText = GetText((int)Texts.Warning_Title_Text);
        warningText = GetText((int)Texts.Warning_Text);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 닫기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 경고 텍스트 설정 메서드
    public void SetWarningText(string title, string message)
    {
        if (warningTitleText != null && warningText != null)
        {
            warningTitleText.text = title;
            warningText.text = message;
        }
    }
}
