using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Close_Button,
        Open_Setting_Button,
        Close_Setting_Button,
        Open_Load_Lobby_Button,
        Open_Game_Exit_Button,
        Cancel_Button
    }

    enum GameObjects
    {
        Setting_Panel,
        Load_Lobby_Button_Continaer
    }

    enum Sliders
    {
        // 전체 음량
        Sound_Setting_Slider
    }

    enum Texts
    {
        // 전체 음량 텍스트
        Sound_Setting_Ratio_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button closeButton;
    private Button openSettingButton;
    private Button closeSettingButton;
    private Button openLoadLobbyButton;
    private Button openGameExitButton;
    private Button cancelButton;
    private GameObject settingPanel;
    private GameObject loadLobbyButtonContinaer;
    private Slider soundSettingSlider;
    private TextMeshProUGUI soundSettingRatioText;

    // 현재 Scene 상태 변수
    private bool isLobbyScene;

    // 오디오 믹서
    public AudioMixer soundMixer;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Slider>(typeof(Sliders));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 닫기 버튼 이벤트 등록
        closeButton = GetButton((int)Buttons.Close_Button);
        AddUIEvent(closeButton.gameObject, Cancel);

        // 설정 띄우기 버튼 이벤트 등록
        openSettingButton = GetButton((int)Buttons.Open_Setting_Button);
        AddUIEvent(openSettingButton.gameObject, OpenSetting);

        // 설정 닫기 버튼 이벤트 등록
        closeSettingButton = GetButton((int)Buttons.Close_Setting_Button);
        AddUIEvent(closeSettingButton.gameObject, CloseSetting);

        // 로비로 돌아가기 Popup UI 띄우기 버튼 이벤트 등록
        openLoadLobbyButton = GetButton((int)Buttons.Open_Load_Lobby_Button);
        AddUIEvent(openLoadLobbyButton.gameObject, OpenLoadLobbyScene);

        // 게임 종료 Popup UI 띄우기 버튼 이벤트 등록
        openGameExitButton = GetButton((int)Buttons.Open_Game_Exit_Button);
        AddUIEvent(openGameExitButton.gameObject, OpenGameExit);

        // 닫기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 설정 패널 초기화 및 비활성화
        settingPanel = GetObject((int)GameObjects.Setting_Panel);

        // 전체 음량 슬라이더 초기화
        soundSettingSlider = GetSlider((int)Sliders.Sound_Setting_Slider);
        soundSettingSlider.value = 1;
        soundSettingSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });

        // 전체 음량 텍스트 초기화
        soundSettingRatioText = GetText((int)Texts.Sound_Setting_Ratio_Text);
        UpdateSoundText(soundSettingSlider.value);

        // 설정 닫기
        CloseSetting();

        // 현재 Scene이 로비 Scene인지 확인
        loadLobbyButtonContinaer = GetObject((int)GameObjects.Load_Lobby_Button_Continaer);
        isLobbyScene = SceneManager.GetActiveScene().name == "Lobby";

        if (isLobbyScene)
        {
            // 로비 Scene일 경우, 로비로 돌아가기 버튼 숨김
            loadLobbyButtonContinaer.SetActive(false);
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------


    // 슬라이더 값 변경 시 호출될 메서드
    private void OnSliderValueChanged()
    {
        SoundControl();
        UpdateSoundText(soundSettingSlider.value);
    }

    // 사운드 설정 메서드
    private void SoundControl()
    {
        float soundValue = soundSettingSlider.value;
        float volume = Mathf.Log10(Mathf.Max(soundValue, 0.0001f)) * 20;
        soundMixer.SetFloat("Master", volume);
    }

    // 음량 텍스트 업데이트 메서드
    private void UpdateSoundText(float value)
    {
        soundSettingRatioText.text = $"{Mathf.FloorToInt(value * 100)}";
    }


    // 설정 띄우기 메서드
    private void OpenSetting(PointerEventData data)
    {
        // 버튼 교체
        closeSettingButton.gameObject.SetActive(true);
        openSettingButton.gameObject.SetActive(false);

        // 설정 패널 띄우기
        settingPanel.SetActive(true);
    }

    // 설정 닫기 메서드
    private void CloseSetting(PointerEventData data)
    {
        CloseSetting();
    }

    // 설정 닫기 메서드
    private void CloseSetting()
    {
        // 버튼 교체
        closeSettingButton.gameObject.SetActive(false);
        openSettingButton.gameObject.SetActive(true);

        // 설정 패널 닫기
        settingPanel.SetActive(false);
    }

    // 로비로 돌아가기 Popup UI 띄우기 메서드
    private void OpenLoadLobbyScene(PointerEventData data)
    {
        // 설정 닫기
        CloseSetting();

        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Load_Lobby_Popup_UI>("[Dungeon]_Load_Lobby_Popup_UI");
    }

    // 게임 종료 Popup UI 띄우기 메서드
    private void OpenGameExit(PointerEventData data)
    {
        // 설정 닫기
        CloseSetting();

        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Game_Exit_Popup_UI>("[Common]_Game_Exit_Popup_UI");
    }

    // 닫기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }
}
