using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using PlayerStates;

public class Dungeon_Popup_UI : UI_Popup
{
    // 게임오브젝트 인덱스
    enum GameObjects
    {
        DeepForest_Icon,
        ForgottenTemple_Icon,
        StarShardPlain_Icon,
        Boss_Status
    }

    // 텍스트 인덱스
    enum Texts
    {
        Dungeon_Name_Text,
        Time_Text,
        Member_Nickname_Text_1,
        Player_Nickname_Text,
        Player_HP_Text
    }

    // Slider 인덱스
    enum Sliders
    {
        Dungeon_Progress_Bar,
        Member_HP_Slider_1,
        Player_HP_Slider,
        Player_EXP_Slider
    }

    // 버튼 인덱스
    enum Buttons
    {
        Open_Chat_Button,
        Open_Menu_Button
    }

    // 추가된 멤버 변수
    private GameObject deepForestIcon;
    private GameObject forgottenTempleIcon;
    private GameObject starShardPlainIcon;
    private GameObject bossStatus;
    private TextMeshProUGUI dungeonNameText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI memberNicknameText1;
    private TextMeshProUGUI playerNicknameText;
    private TextMeshProUGUI playerHPText;
    private Slider dungeonProgressBar;
    private Slider memberHPSlider1;
    private Slider playerHPSlider;
    private Slider playerEXPSlider;
    public Transform[] checkpoints;
    private int totalCheckpoints = 3;
    private int currentCheckpointIndex = 0;
    private float gameTime = 0f;

    public Stat playerStat;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
        Bind<Button>(typeof(Buttons));

        // 닉네임
        memberNicknameText1 = GetText((int)Texts.Member_Nickname_Text_1);
        playerNicknameText = GetText((int)Texts.Player_Nickname_Text);
        UpdateUserInfo();

        // 던전 아이콘
        deepForestIcon = GetObject((int)GameObjects.DeepForest_Icon);
        forgottenTempleIcon = GetObject((int)GameObjects.ForgottenTemple_Icon);
        starShardPlainIcon = GetObject((int)GameObjects.StarShardPlain_Icon);

        // 보스 상태창
        bossStatus = GetObject((int)GameObjects.Boss_Status);
        bossStatus.SetActive(false);

        // 게임 시간
        timeText = GetText((int)Texts.Time_Text);

        // 선택된 던전
        dungeonNameText = GetText((int)Texts.Dungeon_Name_Text);
        UpdateSelectedDungeon();

        // Slider와 GameObject 연결
        dungeonProgressBar = GetSlider((int)Sliders.Dungeon_Progress_Bar);
        dungeonProgressBar.value = 0;

        playerHPText = GetText((int)Texts.Player_HP_Text);
        playerHPSlider = GetSlider((int)Sliders.Player_HP_Slider);
        playerHPSlider.value = 1;
        memberHPSlider1 = GetSlider((int)Sliders.Member_HP_Slider_1);
        memberHPSlider1.value = 1;

        playerEXPSlider = GetSlider((int)Sliders.Player_EXP_Slider);

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerStat = playerObject.GetComponent<Stat>();
        }

        KnightGController.OnBossDestroyed += HandleBossDestroyed;

        // 채팅 Popup UI 열기 버튼 이벤트 등록
        Button openChatButton = GetButton((int)Buttons.Open_Chat_Button);
        AddUIEvent(openChatButton.gameObject, OpenChat);
        AddUIKeyEvent(openChatButton.gameObject, () => OpenChat(null), KeyCode.C);

        // 메뉴 Popup UI 열기 버튼 이벤트 등록
        Button openMenuButton = GetButton((int)Buttons.Open_Menu_Button);
        AddUIEvent(openMenuButton.gameObject, OpenMenu);
        AddUIKeyEvent(openMenuButton.gameObject, () => OpenMenu(null), KeyCode.Escape);
    }

    void Update()
    {
        // 던전에 있는 동안 시간 흐름
        gameTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        UpdateHP();
    }

    void OnDestroy()
    {
        KnightGController.OnBossDestroyed -= HandleBossDestroyed;
    }

    // 유저 정보 업데이트하기
    private void UpdateUserInfo()
    {
        memberNicknameText1.text = Managers.Player.GetNickName();
        playerNicknameText.text = Managers.Player.GetNickName();
    }

    // 현재 던전 정보 업데이트하기
    private void UpdateSelectedDungeon()
    {
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);
        deepForestIcon.SetActive(false);
        forgottenTempleIcon.SetActive(false);
        starShardPlainIcon.SetActive(false);

        // 던전 번호에 따라 다른 텍스트를 설정
        switch (selectedDungeonNumber)
        {
            case 1:
                dungeonNameText.text = "깊은 숲";
                deepForestIcon.SetActive(true);
                break;
            case 2:
                dungeonNameText.text = "잊혀진 신전";
                forgottenTempleIcon.SetActive(true);
                break;
            case 3:
                dungeonNameText.text = "별의 조각 평원";
                starShardPlainIcon.SetActive(true);
                break;
            default:
                dungeonNameText.text = "알 수 없는 던전입니다.";
                break;
        }
    }

    public void UpdateHP()
    {
        // 체력 업데이트
        playerHPText.text = $"{playerStat.Hp} / {playerStat.MaxHp}";
        playerHPSlider.value = (float)playerStat.Hp / playerStat.MaxHp;

        memberHPSlider1.value = (float)playerStat.Hp / playerStat.MaxHp;
    }

    public void UpdateProgress()
    {
        if (currentCheckpointIndex < totalCheckpoints)
        {
            // 체크포인트 통과시 인덱스 증가
            currentCheckpointIndex++;

            // 체크포인트 인덱스에 따라 진행바를 업데이트
            dungeonProgressBar.value = (float) currentCheckpointIndex / totalCheckpoints;
        }

        if (dungeonProgressBar.value == 1)
        {
            // 보스 상태창을 염
            bossStatus.SetActive(true);
        }
    }

    // 던전 결과 Popup UI 열기
    private void HandleBossDestroyed()
    {
        Managers.UI.ShowPopupUI<Result_Popup_UI>("[Dungeon]_Result_Popup_UI");
    }

    // 채팅 Popup UI 열기
    private void OpenChat(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Chat_Popup_UI>("[Common]_Chat_Popup_UI");
    }

    // 메뉴 Popup UI 열기
    private void OpenMenu(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Menu_Popup_UI>("[Common]_Menu_Popup_UI");
    }
}
