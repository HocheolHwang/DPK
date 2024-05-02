using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using PlayerStates;
using System;

public class Dungeon_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Open_Chat_Button,
        Open_Menu_Button
    }

    enum GameObjects
    {
        Tutorial_Icon,
        DeepForest_Icon,
        ForgottenTemple_Icon,
        StarShardPlain_Icon,
        Boss_Status
    }

    enum Images
    {
        Skill_1_Cooldown_Image,
        Skill_2_Cooldown_Image,
        Skill_3_Cooldown_Image,
        Skill_4_Cooldown_Image,
        Skill_5_Cooldown_Image,
        Skill_6_Cooldown_Image,
        Skill_7_Cooldown_Image,
        Skill_8_Cooldown_Image,
        Skill_1_Unable_Image,
        Skill_2_Unable_Image,
        Skill_3_Unable_Image,
        Skill_4_Unable_Image,
        Skill_5_Unable_Image,
        Skill_6_Unable_Image,
        Skill_7_Unable_Image,
        Skill_8_Unable_Image
    }

    enum Texts
    {
        Dungeon_Name_Text,
        Time_Text,
        Member_Nickname_Text_1,
        Player_Tier_Text,
        Player_Nickname_Text,
        Player_HP_Text,
        Boss_Name_Text,
        Boss_HP_Text,
        Skill_1_Cooldown_Text,
        Skill_2_Cooldown_Text,
        Skill_3_Cooldown_Text,
        Skill_4_Cooldown_Text,
        Skill_5_Cooldown_Text,
        Skill_6_Cooldown_Text,
        Skill_7_Cooldown_Text,
        Skill_8_Cooldown_Text
    }

    enum Sliders
    {
        Dungeon_Progress_Bar,
        Member_HP_Slider_1,
        Player_HP_Slider,
        Player_EXP_Slider,
        Boss_HP_Slider
    }

    // UI 컴포넌트 바인딩 변수
    private Button openChatButton;
    private Button openMenuButton;
    private GameObject tutorialIcon;
    private GameObject deepForestIcon;
    private GameObject forgottenTempleIcon;
    private GameObject starShardPlainIcon;
    private GameObject bossStatus;
    private Image[] skillCooldownImages = new Image[8];
    private Image[] skillUnableImages = new Image[8];
    private TextMeshProUGUI dungeonNameText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI memberNicknameText1;
    private TextMeshProUGUI playerTierText;
    private TextMeshProUGUI playerNicknameText;
    private TextMeshProUGUI playerHPText;
    private TextMeshProUGUI bossNameText;
    private TextMeshProUGUI bossHPText;
    private TextMeshProUGUI[] skillCooldownTexts = new TextMeshProUGUI[8];
    private Slider dungeonProgressBar;
    private Slider memberHPSlider1;
    private Slider bossHPSlider;
    private Slider playerHPSlider;
    private Slider playerEXPSlider;

    // 게임 위치 및 진행 상태 변수
    public Transform[] checkpoints;
    private int totalCheckpoints = 3;
    private int currentCheckpointIndex = 0;
    private float gameTime = 0f;

    // 게임 플레이어 및 보스 상태 변수
    public Stat playerStat;
    public Stat bossStat;

    // 현재 Scene 상태 변수
    private bool isTutorialScene;

    // 현재 누적된 경험치
    private int currentExp = 0;

    // 스킬 슬롯
    private SkillSlot skillSlot;

    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));


        // --------------- 튜토리얼 및 UI 버튼 이벤트 설정 ---------------

        // 현재 Scene이 튜토리얼 Scene인지 확인
        isTutorialScene = SceneManager.GetActiveScene().name == "Tutorial";

        // UI 버튼 요소들을 가져옴
        openChatButton = GetButton((int)Buttons.Open_Chat_Button);
        openMenuButton = GetButton((int)Buttons.Open_Menu_Button);

        if (isTutorialScene)
        {
            // 튜토리얼 Scene일 경우, 채팅 및 메뉴 버튼 숨김
            openChatButton.gameObject.SetActive(false);
            openMenuButton.gameObject.SetActive(false);
        }
        else
        {
            // 튜토리얼 Scene이 아닐 경우, 버튼 이벤트 등록
            // 채팅 Popup UI 열기 버튼 이벤트 등록
            AddUIEvent(openChatButton.gameObject, OpenChat);
            AddUIKeyEvent(openChatButton.gameObject, () => OpenChat(null), KeyCode.C);

            // 메뉴 Popup UI 열기 버튼 이벤트 등록
            AddUIEvent(openMenuButton.gameObject, OpenMenu);
            AddUIKeyEvent(openMenuButton.gameObject, () => OpenMenu(null), KeyCode.Escape);
        }


        // --------------- 던전 정보 UI 초기화 ---------------

        // 던전 아이콘 초기화
        tutorialIcon = GetObject((int)GameObjects.Tutorial_Icon);
        deepForestIcon = GetObject((int)GameObjects.DeepForest_Icon);
        forgottenTempleIcon = GetObject((int)GameObjects.ForgottenTemple_Icon);
        starShardPlainIcon = GetObject((int)GameObjects.StarShardPlain_Icon);

        // 던전 이름 초기화
        dungeonNameText = GetText((int)Texts.Dungeon_Name_Text);

        // 던전 시간 초기화
        timeText = GetText((int)Texts.Time_Text);

        // 던전 진행상황 슬라이더 초기화
        dungeonProgressBar = GetSlider((int)Sliders.Dungeon_Progress_Bar);

        // 던전 정보 업데이트
        UpdateDungeonInfo();


        // --------------- 보스 정보 UI 초기화 ---------------

        // 보스 상태창 초기화 및 비활성화
        bossStatus = GetObject((int)GameObjects.Boss_Status);
        bossStatus.SetActive(false);

        // 보스 이름 및 HP 텍스트, HP 슬라이더 초기화
        bossNameText = GetText((int)Texts.Boss_Name_Text);
        bossHPText = GetText((int)Texts.Boss_HP_Text);
        bossHPSlider = GetSlider((int)Sliders.Boss_HP_Slider);

        // 보스 정보 업데이트
        UpdateBossInfo();


        // --------------- 파티원 및 플레이어 정보 UI 초기화 ---------------

        // 파티원 정보 초기화
        memberNicknameText1 = GetText((int)Texts.Member_Nickname_Text_1);
        memberHPSlider1 = GetSlider((int)Sliders.Member_HP_Slider_1);

        // 플레이어 정보 초기화
        playerTierText = GetText((int)Texts.Player_Tier_Text);
        playerNicknameText = GetText((int)Texts.Player_Nickname_Text);
        playerHPText = GetText((int)Texts.Player_HP_Text);
        playerHPSlider = GetSlider((int)Sliders.Player_HP_Slider);
        playerEXPSlider = GetSlider((int)Sliders.Player_EXP_Slider);

        // 파티원 및 플레이어 정보 업데이트
        UpdatePlayerInfo();

        // 플레이어 게임 오브젝트 태그 이용해 찾기
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerStat = playerObject.GetComponent<Stat>();
        }


        // --------------- 쿨타임 UI 초기화 ---------------

        // 스킬 슬롯 참조
        skillSlot = FindObjectOfType<SkillSlot>();

        // 이미지와 텍스트 초기화를 반복문으로 처리
        for (int i = 0; i < skillCooldownImages.Length; i++)
        {
            // 쿨다운 이미지 배열 초기화
            skillCooldownImages[i] = GetImage((int)Images.Skill_1_Cooldown_Image + i);

            // 스킬 사용불가 이미지 배열 초기화
            skillUnableImages[i] = GetImage((int)Images.Skill_1_Unable_Image + i);

            // 쿨타임 텍스트 배열 초기화
            skillCooldownTexts[i] = GetText((int)Texts.Skill_1_Cooldown_Text + i);
        }

        // 던전 입장 시 모든 스킬의 쿨타임 UI 초기화
        for (int i = 0; i < 8; i++)
        {
            ResetCooldownUI(i);
        }
    }


    // ------------------------------ 유니티 생명주기 메서드 ------------------------------

    void Update()
    {
        // 게임 시간 업데이트
        UpdateTime();

        // HP 업데이트
        UpdateHP();

        // 스킬 쿨타임 업데이트
        for (int i = 0; i < 5; i++)
        {
            UpdateCooldownUI(i);
        }
    }

    void OnDestroy()
    {
        KnightGController.OnBossDestroyed -= HandleBossDestroyed;

        if (isTutorialScene)
        {
            // 튜토리얼이 끝나면 채팅 버튼과 메뉴 버튼을 다시 활성화
            openChatButton.gameObject.SetActive(true);
            openMenuButton.gameObject.SetActive(true);
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 던전 정보 업데이트 메서드
    private void UpdateDungeonInfo()
    {
        // 선택된 던전 번호를 가져옴
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 0);
        
        // 튜토리얼 Scene일 경우, 선택된 던전 번호를 0으로 설정
        if (isTutorialScene) selectedDungeonNumber = 0;

        // 아이콘 비활성화
        tutorialIcon.SetActive(false);
        deepForestIcon.SetActive(false);
        forgottenTempleIcon.SetActive(false);
        starShardPlainIcon.SetActive(false);

        // 던전 번호에 따라 다른 텍스트와 아이콘 설정
        (string dungeonName, GameObject activeIcon) = selectedDungeonNumber switch
        {
            1 => ("깊은 숲", deepForestIcon),
            2 => ("잊혀진 신전", forgottenTempleIcon),
            3 => ("별의 조각 평원", starShardPlainIcon),
            _ => ("던전처리기사 실기 시험장", tutorialIcon)
        };

        // 던전 이름 설정
        dungeonNameText.text = dungeonName;

        // 해당하는 아이콘 활성화
        activeIcon.SetActive(true);

        // 던전 진행 슬라이더 설정
        dungeonProgressBar.value = 0;
    }

    // 보스 정보 업데이트 메서드
    private void UpdateBossInfo()
    {
        // 보스 HP 슬라이더 설정
        bossHPSlider.value = 1;

        // 보스 파괴 이벤트 핸들러 등록
        KnightGController.OnBossDestroyed += HandleBossDestroyed;
    }

    // 파티원 및 플레이어 정보 업데이트 메서드
    private void UpdatePlayerInfo()
    {
        // 파티원 닉네임 및 HP 슬라이더 설정
        memberNicknameText1.text = Managers.Player.GetNickName();
        memberHPSlider1.value = 1;

        // 플레이어 등급, 닉네임 및 HP 슬라이더 설정
        playerTierText.text = isTutorialScene ? "던전처리기사 수험생" : "견습 던전처리기사";
        playerNicknameText.text = Managers.Player.GetNickName();
        playerHPSlider.value = 1;
    }

    // 게임 시간 업데이트 메서드
    public void UpdateTime()
    {
        gameTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // HP 업데이트 메서드
    public void UpdateHP()
    {
        // 파티원 체력 업데이트
        memberHPSlider1.value = (float)playerStat.Hp / playerStat.MaxHp;

        // 플레이어 체력 업데이트
        playerHPText.text = $"{playerStat.Hp} / {playerStat.MaxHp}";
        playerHPSlider.value = (float)playerStat.Hp / playerStat.MaxHp;

        // 보스 체력 업데이트
        if (bossStatus.activeSelf == false) return;
        bossHPText.text = $"{bossStat.Hp} / {bossStat.MaxHp}";
        bossHPSlider.value = (float)bossStat.Hp / bossStat.MaxHp;
    }

    // 던전 진행 슬라이더 업데이트 메서드
    public void UpdateProgress()
    {
        if (currentCheckpointIndex < totalCheckpoints)
        {
            // 체크포인트 통과시 인덱스 증가
            currentCheckpointIndex++;

            // 체크 포인트 통과 시 받는 경험치 증가
            int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 0);
            currentExp += (10 * (5* selectedDungeonNumber)) * currentCheckpointIndex;

            // 체크포인트 인덱스에 따라 진행바를 업데이트
            dungeonProgressBar.value = (float) currentCheckpointIndex / totalCheckpoints;
        }

        if (dungeonProgressBar.value == 1)
        {
            BossMonster bossObject = FindObjectOfType<BossMonster>();
            if (bossObject != null)
            {
                bossStat = bossObject.GetComponent<Stat>();
            }

            // 보스 상태창을 띄움
            bossStatus.SetActive(true);
            bossNameText.text = bossStat.gameObject.name;
        }
    }

    // 보스 처치 시 실행되는 메서드
    private void HandleBossDestroyed()
    {
        // 보스 클리어
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 0);
        if(selectedDungeonNumber!= 0)
        {
            currentExp += (10 * (5 * selectedDungeonNumber)) * currentCheckpointIndex;
            SummaryExp();
        }

        // 던전 결과 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Result_Popup_UI>("[Dungeon]_Result_Popup_UI");

        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        PlayerZone zone = GameObject.FindObjectOfType<PlayerZone>();
        zone.StopMovement();
        foreach(var player in players)
        {
            player.isFinished = true;
        }
    }

    // 채팅 Popup UI 띄우기 메서드
    private void OpenChat(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Chat_Popup_UI>("[Common]_Chat_Popup_UI");
    }

    // 메뉴 Popup UI 띄우기 메서드
    private void OpenMenu(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<Menu_Popup_UI>("[Common]_Menu_Popup_UI");
    }

    // 경험치 리포트 메서드
    public void SummaryExp()
    {
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 0);

        // 현재 던전 경험치
        currentExp = ((selectedDungeonNumber-1) * 5) * 10;

        // 경험치 합산
        Managers.Player.AddExp(currentExp);
        EXPStatisticsReqDto dto = new EXPStatisticsReqDto();
        dto.classCode = Managers.Player.GetClassCode();
        dto.currentExp = Managers.Player.GetExp();
        dto.classCode = Managers.Player.GetClassCode();
        dto.playerLevel = Managers.Player.GetLevel();
        dto.reason = dungeonNameText.text + "던전 클리어";
        dto.expDelta = currentExp;
        Managers.Network.EXPStatisticsCall(dto);
    }

    // 쿨타임 UI 초기화 메서드
    private void ResetCooldownUI(int skillIndex)
    {
        skillCooldownImages[skillIndex].fillAmount = 0;
        skillUnableImages[skillIndex].gameObject.SetActive(false);
        skillCooldownTexts[skillIndex].text = "";
    }

    // 스킬 쿨타임 UI 업데이트 메서드
    private void UpdateCooldownUI(int skillIndex)
    {
        float cooldownTime = skillSlot.Skills[skillIndex].CooldownTime;
        float elapsedTime = skillSlot.Skills[skillIndex].ElapsedTime;

        // 남은 시간 초기화
        float remainingTime = cooldownTime - elapsedTime;

        // 남은 시간이 0보다 작거나 같으면 ResetCooldownUI 호출 후 함수 종료
        if (remainingTime <= 0)
        {
            ResetCooldownUI(skillIndex);
            return;
        }

        // 스킬 사용불가 이미지를 활성화
        skillUnableImages[skillIndex].gameObject.SetActive(true);

        // 남은 시간을 텍스트에 업데이트
        if (remainingTime > 1)
        {
            skillCooldownTexts[skillIndex].text = Mathf.Ceil(remainingTime).ToString() + "s";
        }
        else
        {
            skillCooldownTexts[skillIndex].text = remainingTime.ToString("F1");
        }

        // 남은 시간 / 스킬 쿨타임 비율에 따라 fllAmount 값 업데이트
        skillCooldownImages[skillIndex].fillAmount = remainingTime / cooldownTime;
    }
}
