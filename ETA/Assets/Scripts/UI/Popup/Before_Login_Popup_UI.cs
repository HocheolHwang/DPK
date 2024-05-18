using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Before_Login_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Mute_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button muteButton;

    // 음소거 여부를 확인하는 변수
    private bool IsMute
    {
        get { return _isMute; }
        set
        {
            _isMute = value;
            UpdateMuteIcon();
            UpdateSound();
        }
    }

    // 실제 변수 값
    private bool _isMute;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 음소거 버튼 할당 및 이벤트 등록
        muteButton = GetButton((int)Buttons.Mute_Button);
        AddUIEvent(muteButton.gameObject, MuteSound);

        // 음소거 상태 초기화
        IsMute = false;
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 음소거 토글 메서드
    private void MuteSound(PointerEventData data)
    {
        IsMute = !IsMute; // 음소거 상태 토글
    }

    // 음소거 아이콘 업데이트 메서드
    private void UpdateMuteIcon()
    {
        string iconPath = IsMute ? "Sprites/Settings/Mute Icon" : "Sprites/Settings/Listen Icon";
        muteButton.image.sprite = Resources.Load<Sprite>(iconPath);
    }

    // 소리 업데이트 메서드
    private void UpdateSound()
    {
        // TODO: IsMute 값에 따라 소리를 켜고 끄는 로직 추가 필요
    }
}
