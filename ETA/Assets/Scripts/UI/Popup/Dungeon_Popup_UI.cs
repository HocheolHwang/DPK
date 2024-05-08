using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using PlayerStates;
using System;
using Photon.Pun;

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
        // 던전 아이콘
        Tutorial_Icon,
        DeepForest_Icon,
        ForgottenTemple_Icon,
        StarShardPlain_Icon,

        // 보스 상태
        Boss_Status,

        // 플레이어 상태
        Player_HP_Bar,
        Player_Shield_Bar,

        // 파티원 상태
        Member_HP_Bar_1,
        Member_HP_Bar_2,
        Member_HP_Bar_3,
        Member_Shield_Bar_1,
        Member_Shield_Bar_2,
        Member_Shield_Bar_3
    }

    enum Images
    {
        // 파티원 상태
        Party_Member_1,
        Party_Member_2,
        Party_Member_3,

        // 파티원 클래스 아이콘
        Warrior_Icon_1,
        Acher_Icon_1,
        Mage_Icon_1,
        Warrior_Icon_2,
        Acher_Icon_2,
        Mage_Icon_2,
        Warrior_Icon_3,
        Acher_Icon_3,
        Mage_Icon_3,

        // 스킬 쿨타임
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
        // 던전 상태
        Dungeon_Tier_Text,
        Dungeon_Name_Text,
        Time_Text,

        // 파티원 상태
        Member_Level_Text_1,
        Member_Level_Text_2,
        Member_Level_Text_3,
        Member_Nickname_Text_1,
        Member_Nickname_Text_2,
        Member_Nickname_Text_3,

        // 플레이어 상태
        Player_Tier_Text,
        Player_Nickname_Text,
        Player_HP_Text,
        Player_Level_Text,

        // 보스 상태
        Boss_Level_Text,
        Boss_Name_Text,
        Boss_HP_Text,

        // 스킬 쿨타임
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
        // 던전 상태
        Dungeon_Progress_Bar,

        // 플레이어 상태
        Player_EXP_Slider,

        // 보스 상태
        Boss_HP_Slider
    }

    // UI 컴포넌트 바인딩 변수
    private Button openChatButton;
    private Button openMenuButton;
    private GameObject[] dungeonIcons = new GameObject[4];
    private GameObject bossStatus;
    private GameObject playerHPBar;
    private GameObject playerShieldBar;
    private GameObject[] memberHPBars = new GameObject[3];
    private GameObject[] memberShieldBars = new GameObject[3];
    private Image[] partyMembers = new Image[3];
    private Image[][] partyMemberIcons = new Image[3][];
    private Image[] skillCooldownImages = new Image[8];
    private Image[] skillUnableImages = new Image[8];
    private TextMeshProUGUI dungeonTierText;
    private TextMeshProUGUI dungeonNameText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI[] memberLevelTexts = new TextMeshProUGUI[3];
    private TextMeshProUGUI[] memberNicknameTexts = new TextMeshProUGUI[3];
    private TextMeshProUGUI playerTierText;
    private TextMeshProUGUI playerNicknameText;
    private TextMeshProUGUI playerHPText;
    private TextMeshProUGUI playerLevelText;
    private TextMeshProUGUI bossLevelText;
    private TextMeshProUGUI bossNameText;
    private TextMeshProUGUI bossHPText;
    private TextMeshProUGUI[] skillCooldownTexts = new TextMeshProUGUI[8];
    private Slider dungeonProgressBar;
    private Slider bossHPSlider;
    private Slider playerEXPSlider;

    // 게임 위치 및 진행 상태 변수
    private GameObject checkpoints;
    private int totalCheckpoints;
    private int currentCheckpointIndex = 0;
    private float gameTime = 0f;

    public int CurrentCheckPointIndex { get => currentCheckpointIndex; private set => currentCheckpointIndex = value; }

    // 게임 플레이어 및 보스 상태 변수
    public Stat playerStat;
    public Stat[] playersStat = new Stat[3];
    public Stat bossStat;

    // 선택된 던전 번호
    private int selectedDungeonNumber;

    // 현재 Scene 상태 변수
    private bool isTutorialScene;

    // 현재 누적된 경험치
    private int currentExp = 0;

    // 경험치
    private bool expResult = false;

    // 스킬 슬롯
    public SkillSlot skillSlot;

    // 파티원 수
    private int partySize = PhotonNetwork.PlayerList.Length;


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

        // 선택된 던전 번호 초기화
        selectedDungeonNumber = FindObjectOfType<GameSystem>().currentDungeonNum;

        // 체크포인트 초기화
        checkpoints = GameObject.Find("CheckPoints");

        // 던전 아이콘 초기화를 반복문으로 처리
        for (int i = 0; i < dungeonIcons.Length; i++)
        {
            // 던전 아이콘 배열 초기화
            dungeonIcons[i] = GetObject((int)GameObjects.Tutorial_Icon + i);
        }

        // 던전 등급 및 이름 초기화
        dungeonTierText = GetText((int)Texts.Dungeon_Tier_Text);
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

        // 보스 정보 초기화
        bossLevelText = GetText((int)Texts.Boss_Level_Text);
        bossNameText = GetText((int)Texts.Boss_Name_Text);
        bossHPText = GetText((int)Texts.Boss_HP_Text);
        bossHPSlider = GetSlider((int)Sliders.Boss_HP_Slider);

        // 보스 정보 업데이트
        UpdateBossInfo();


        // --------------- 파티원 및 플레이어 정보 UI 초기화 ---------------

        // 파티원 아이콘 초기화
        partyMemberIcons[0] = new Image[3];
        partyMemberIcons[1] = new Image[3];
        partyMemberIcons[2] = new Image[3];

        // 파티원 정보 초기화
        for (int i = 0; i < 3; i++)
        {
            // 파티원 정보
            partyMembers[i] = GetImage((int)Images.Party_Member_1 + i);
            memberLevelTexts[i] = GetText((int)Texts.Member_Level_Text_1 + i);
            memberNicknameTexts[i] = GetText((int)Texts.Member_Nickname_Text_1 + i);
            memberHPBars[i] = GetObject((int)GameObjects.Member_HP_Bar_1 + i);
            memberShieldBars[i] = GetObject((int)GameObjects.Member_Shield_Bar_1 + i);

            // 클래스 아이콘
            partyMemberIcons[0][i] = GetImage((int)Images.Warrior_Icon_1 + i);
            partyMemberIcons[1][i] = GetImage((int)Images.Warrior_Icon_2 + i);
            partyMemberIcons[2][i] = GetImage((int)Images.Warrior_Icon_3 + i);
        }

        // 플레이어 정보 초기화
        playerHPBar = GetObject((int)GameObjects.Player_HP_Bar);
        playerShieldBar = GetObject((int)GameObjects.Player_Shield_Bar);
        playerTierText = GetText((int)Texts.Player_Tier_Text);
        playerNicknameText = GetText((int)Texts.Player_Nickname_Text);
        playerHPText = GetText((int)Texts.Player_HP_Text);
        playerLevelText = GetText((int)Texts.Player_Level_Text);
        playerEXPSlider = GetSlider((int)Sliders.Player_EXP_Slider);

        // 플레이어 정보 업데이트
        UpdatePlayerInfo();

        // 파티 정보 업데이트
        UpdatePartyInfo();

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

        expResult = false;

        FindObjectOfType<PhotonChat>().gameObject.AddComponent<SendRoomLog>();
        Managers.Photon.CloseRoom();
    }

    //public void PartyEnter()
    //{
    //    photonView.RPC("Managers.Photon.SendRoomEnterLog", RpcTarget.All);
    //    //Managers.Photon.SendRoomEnterLog();
    //}


    // ------------------------------ 유니티 생명주기 메서드 ------------------------------

    void Update()
    {
        // 게임 시간 업데이트
        UpdateTime();

        // HP 업데이트
        UpdateHP();

        // 스킬 쿨타임 업데이트
        for (int i = 0; i < 8; i++)
        {
            UpdateCooldownUI(i);
        }

        // 파티원 수가 바뀌면 파티 정보 UI 및 파티원 수 업데이트
        if (partySize != PhotonNetwork.PlayerList.Length)
        {
            UpdatePartyInfo();
            partySize = PhotonNetwork.PlayerList.Length;
        }
    }

    void OnDestroy()
    {
        // 등록된 이벤트 핸들러 해제
        KnightGController.OnBossDestroyed -= HandleBossDestroyed;
        PlayerController.OnPlayerDestroyed -= HandlePlayerDestroyed;

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
        // 체크 포인트 개수 확인
        if (checkpoints != null)
        {
            totalCheckpoints = checkpoints.transform.childCount;
        }
        else
        {
            Debug.LogError("'checkpoints' 오브젝트를 찾을 수 없습니다.");
        }

        // 튜토리얼 Scene일 경우, 선택된 던전 번호를 0으로 설정
        if (isTutorialScene) selectedDungeonNumber = 0;

        // 아이콘 비활성화를 반복문으로 처리
        for (int i = 0; i < dungeonIcons.Length; i++)
        {
            // 던전 아이콘 배열 비활성화
            dungeonIcons[i].SetActive(false);
        }

        // 던전 번호에 따라 다른 텍스트와 아이콘 설정
        (string dungeonTier, string dungeonName, GameObject activeIcon) = selectedDungeonNumber switch
        {
            1 => ("초급 던전", "깊은 숲", dungeonIcons[1]),
            2 => ("중급 던전", "잊혀진 신전", dungeonIcons[2]),
            3 => ("고급 던전", "별의 조각 평원", dungeonIcons[3]),
            4 => ("테스트 던전", "Test 1", dungeonIcons[3]),
            5 => ("테스트 던전", "Test 2", dungeonIcons[3]),
            _ => ("튜토리얼 던전", "던전처리기사 실기 시험장", dungeonIcons[0])
        };

        // 던전 티어 설정
        dungeonTierText.text = dungeonTier;

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

        // 보스 레벨 텍스트 설정
        bossLevelText.text = selectedDungeonNumber switch
        {
            1 => "Lv. 5",
            2 => "Lv. 12",
            3 => "Lv. 20",
            _ => "Lv. 2"
        };

        // 보스 파괴 이벤트 핸들러 등록
        KnightGController.OnBossDestroyed += HandleBossDestroyed;
    }

    // 파티원 정보 업데이트 메서드
    private void UpdatePlayerInfo()
    {
        // 플레이어 등급, 닉네임 및 레벨 설정
        playerTierText.text = isTutorialScene ? "던전처리기사 수험생" : "견습 던전처리기사";
        playerNicknameText.text = Managers.Player.GetNickName();
        playerLevelText.text = $"Lv. {Managers.Player.GetLevel()}";

        // 플레이어 경험치 설정
        playerEXPSlider.value = (Managers.Player.GetExp() / (float)Managers.Player.GetNeedExp());

        // 플레이어 파괴 이벤트 핸들러 등록
        //PlayerController.OnPlayerDestroyed += HandlePlayerDestroyed;
    }

    // 게임 시간 업데이트 메서드
    public void UpdateTime()
    {
        gameTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // HP와 실드 업데이트 메서드
    public static void UpdateHealthAndShieldBars(GameObject hpBar, GameObject shieldBar, Stat stat, float maxLength)
    {
        // 체력바와 실드바의 RectTransform을 참조
        RectTransform hpRectTransform = hpBar.GetComponent<RectTransform>();
        RectTransform shieldRectTransform = shieldBar.GetComponent<RectTransform>();

        // 체력 및 실드 비율 계산
        float total = Mathf.Max(stat.Hp + stat.Shield, stat.MaxHp);
        float hpScale = stat.Hp / total;
        float shieldScale = stat.Shield / total;

        // 체력바와 실드바의 너비 설정
        hpRectTransform.sizeDelta = new Vector2(hpScale * maxLength, hpRectTransform.sizeDelta.y);
        shieldRectTransform.sizeDelta = new Vector2(shieldScale * maxLength, shieldRectTransform.sizeDelta.y);
    }

    // HP 업데이트 메서드
    public void UpdateHP()
    {
        // 플레이어 체력바 업데이트
        

        // 플레이어 체력 업데이트
        if (playerStat != null)
        {
            if (playerStat.Shield > 0)
            {
                playerHPText.text = $"{playerStat.Hp} / {playerStat.MaxHp} (+{playerStat.Shield})";
            }
            else
            {
                playerHPText.text = $"{playerStat.Hp} / {playerStat.MaxHp}";
            }

            UpdateHealthAndShieldBars(playerHPBar, playerShieldBar, playerStat, 294);
        }
        else
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (var playerObj in playerObjects)
            {
                Debug.Log(playerObj.name);
                if (playerObj.GetComponent<PhotonView>().IsMine)
                {
                    playerStat = playerObj.GetComponent<Stat>();
                    break;
                }
            }
        }

        // 파티원 체력 업데이트
        if(playersStat[0] != null)
        {
            for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                UpdateHealthAndShieldBars(memberHPBars[i], memberShieldBars[i], playersStat[i], 220);
            }
        }

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
            currentExp += 10 * currentCheckpointIndex;

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
        if (selectedDungeonNumber!= 0)
        {
            currentExp += 10 * currentCheckpointIndex;
            SummaryExp(dungeonNameText.text + "던전 클리어");
        }

        Managers.Photon.SendDungeonEnd(timeText.text, true);

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

    // 플레이어 사망 시 실행되는 메서드
    public void HandlePlayerDestroyed()
    {
        // 플레이어 체력이 0 이하일 때
        if (playerStat.Hp <= 0)
        {
            // 던전 결과 Popup UI를 띄움
            Managers.UI.ShowPopupUI<Result_Popup_UI>("[Dungeon]_Result_Popup_UI");
        }
        
        if (selectedDungeonNumber != 0)
        {
            SummaryExp(dungeonNameText.text + "던전에서 사망");
        }

        Managers.Photon.SendDungeonEnd(timeText.text, false);
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
    public void SummaryExp(string message)
    {
        if (expResult) return;
        expResult = true;
        if (currentExp == 0) return;

        currentExp += 10;

        // 던전 난이도 보상
        if (selectedDungeonNumber > 1)
            currentExp = ((selectedDungeonNumber-1) * 5) * currentExp;
        
        // 경험치 합산
        Managers.Player.AddExp(currentExp);
        EXPStatisticsReqDto dto = new EXPStatisticsReqDto();
        dto.classCode = Managers.Player.GetClassCode();
        dto.currentExp = Managers.Player.GetExp();
        dto.classCode = Managers.Player.GetClassCode();
        dto.playerLevel = Managers.Player.GetLevel();
        dto.reason = message;
        dto.expDelta = currentExp;
        Managers.Network.EXPStatisticsCall(dto);
        currentExp = 0;
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
        if (skillSlot == null || skillSlot.Skills[skillIndex] == null) return;
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

    // 파티 정보 업데이트 메서드
    public void UpdatePartyInfo()
    {
        if (PhotonNetwork.InRoom) // 파티 참가 상태일 경우
        {
            // 파티 멤버 정보 업데이트
            for (int i = 0; i < partyMembers.Length; i++)
            {
                if (i < PhotonNetwork.PlayerList.Length)
                {
                    Photon.Realtime.Player member = PhotonNetwork.PlayerList[i];

                    // 파티 멤버 정보 업데이트
                    partyMembers[i].gameObject.SetActive(true);
                    if(member.CustomProperties["PlayerLevel"] != null)
                    {
                        memberLevelTexts[i].text = $"Lv. {(int)member.CustomProperties["PlayerLevel"]}";
                    }
                    else
                    {
                        memberLevelTexts[i].text = $"Lv. {1}";
                    }
                    
                    memberNicknameTexts[i].text = PhotonNetwork.PlayerList[i].NickName;

                    // 클래스 아이콘 업데이트
                    UpdateClassIcon(i, (string)member.CustomProperties["CurClass"]);
                }
                else
                {
                    // 나머지 파티 멤버 정보 비활성화
                    partyMembers[i].gameObject.SetActive(false);
                }
            }
        }
        else // 파티 미참가 상태일 경우
        {
            // 플레이어 정보 업데이트
            partyMembers[0].gameObject.SetActive(true);
            memberLevelTexts[0].text = $"Lv. {Managers.Player.GetLevel()}";
            memberNicknameTexts[0].text = Managers.Player.GetNickName();

            // 클래스 아이콘 업데이트
            UpdateClassIcon(0, Managers.Player.GetClassCode());

            // 나머지 파티 멤버 정보 비활성화
            partyMembers[1].gameObject.SetActive(false);
            partyMembers[2].gameObject.SetActive(false);
        }
    }

    // 아이콘 업데이트 메서드
    private void UpdateClassIcon(int memberIndex, string classCode)
    {
        // 모든 아이콘을 비활성화
        for (int i = 0; i < partyMemberIcons[memberIndex].Length; i++)
        {
            partyMemberIcons[memberIndex][i].gameObject.SetActive(false);
        }

        // 클래스 코드에 따라 해당 아이콘만 활성화
        switch (classCode)
        {
            case "C001":
                partyMemberIcons[memberIndex][0].gameObject.SetActive(true);
                break;
            case "C002":
                partyMemberIcons[memberIndex][1].gameObject.SetActive(true);
                break;
            case "C003":
                partyMemberIcons[memberIndex][2].gameObject.SetActive(true);
                break;
            default:
                Debug.Log("알 수 없는 클래스 코드: " + classCode);
                break;
        }
    }

    // 파티원 상태 및 클래스 업데이트 메서드 
    public void SetMembersInfo()
    {
        PlayerController[] cons = FindObjectsOfType<PlayerController>();

        foreach (var con in cons)
        {
            if(con.photonView.Owner.CustomProperties["PlayerIndex"] != null)
            {
                int index = (int)con.photonView.Owner.CustomProperties["PlayerIndex"];
                playersStat[index] = con.Stat;
            }
            if(con.photonView.Owner.CustomProperties["CurClass"] != null)
            {
                string classCode = (string)con.photonView.Owner.CustomProperties["CurClass"];
            }
        }
    }
}
