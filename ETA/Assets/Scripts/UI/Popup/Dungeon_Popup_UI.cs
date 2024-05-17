using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using PlayerStates;
using Photon.Pun;
using WebSocketSharp;
using System.Linq;
using System.IO;
using ExitGames.Client.Photon;

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
        StarShardPlain_Icon,
        ForgottenTemple_Icon,
        SeaOfAbyss_Icon,

        // 보스 상태
        Boss_Status,

        // 보스 대사
        Boss_Script_Container,

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
        // 보스 상태
        FlowerDryad_Icon,
        KnightG_Icon,
        MummyMan_Icon,
        Ipris_Icon,
        Dragon_Icon,

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
        Skill_8_Unable_Image,

        // 스킬 아이콘
        Skill_Icon_1,
        Skill_Icon_2,
        Skill_Icon_3,
        Skill_Icon_4,
        Skill_Icon_5,
        Skill_Icon_6,
        Skill_Icon_7,
        Skill_Icon_8,

        // 스킬 슬롯
        Skill_1,
        Skill_2,
        Skill_3,
        Skill_4,
        Skill_5,
        Skill_6,
        Skill_7,
        Skill_8
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

        // 보스 대사
        Boss_Script_Text,

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
    private GameObject bossScriptContainer;
    private GameObject playerHPBar;
    private GameObject playerShieldBar;
    private GameObject[] memberHPBars = new GameObject[3];
    private GameObject[] memberShieldBars = new GameObject[3];
    private Image[] bossIcons = new Image[5];
    private Image[] partyMembers = new Image[3];
    private Image[][] partyMemberIcons = new Image[3][];
    private Image[] skillCooldownImages = new Image[8];
    private Image[] skillUnableImages = new Image[8];
    private Image[] skillIcons = new Image[8];
    private Image[] skillSlotIcons = new Image[8];
    private TextMeshProUGUI dungeonTierText;
    private TextMeshProUGUI dungeonNameText;
    public TextMeshProUGUI timeText;
    private TextMeshProUGUI[] memberLevelTexts = new TextMeshProUGUI[3];
    private TextMeshProUGUI[] memberNicknameTexts = new TextMeshProUGUI[3];
    private TextMeshProUGUI playerTierText;
    private TextMeshProUGUI playerNicknameText;
    private TextMeshProUGUI playerHPText;
    private TextMeshProUGUI playerLevelText;
    private TextMeshProUGUI bossLevelText;
    private TextMeshProUGUI bossNameText;
    private TextMeshProUGUI bossHPText;
    private TextMeshProUGUI bossScriptText;
    private TextMeshProUGUI[] skillCooldownTexts = new TextMeshProUGUI[8];
    private Slider dungeonProgressBar;
    private Slider bossHPSlider;
    private Slider playerEXPSlider;

    // 게임 위치 및 진행 상태 변수
    private GameObject checkpoints;
    private int totalCheckpoints;
    public int currentCheckpointIndex = 0;
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

    // 콜라보 시스템
    private CollavoSystem collavoSystem;

    // 현재 숫자를 추적하는 외부 변수
    private int currentNum = 2;

    // 스킬 정보 팝업
    public GameObject skillInfoPopup;

    // 콜라보 슬롯 이펙트
    private GameObject[,] collavoSlots = new GameObject[3, 8];


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
        isTutorialScene = Managers.Scene.CurrentScene.SceneType == Define.Scene.Tutorial;


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
            Debug.Log(dungeonIcons[i].gameObject.name);
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

        // 보스 아이콘 초기화
        for (int i = 0; i < 5; i++)
        {
            bossIcons[i] = GetImage((int)Images.FlowerDryad_Icon + i);
        }

        // 보스 상태창 초기화 및 비활성화
        bossStatus = GetObject((int)GameObjects.Boss_Status);
        bossStatus.SetActive(false);

        // 보스 대사 초기화
        bossScriptText = GetText((int)Texts.Boss_Script_Text);
        bossScriptContainer = GetObject((int)GameObjects.Boss_Script_Container);
        bossScriptContainer.SetActive(false);


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


        // --------------- 스킬 슬롯 UI 초기화 ---------------

        // 스킬 슬롯 업데이트 코루틴 시작
        //StartCoroutine(LoadSkillSlotDataCoroutine());

        // 스킬 아이콘 초기화
        for (int i = 0; i < 8; i++)
        {
            int index = i;
            skillIcons[index] = GetImage((int)Images.Skill_Icon_1 + index);
            skillSlotIcons[index] = GetImage((int)Images.Skill_1 + index);

            // UI 이벤트를 추가하기 위해 EventTrigger 컴포넌트를 가져옵니다.
            EventTrigger eventTrigger = skillSlotIcons[index].gameObject.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                // EventTrigger 컴포넌트가 없다면 추가합니다.
                eventTrigger = skillSlotIcons[index].gameObject.AddComponent<EventTrigger>();
            }

            // 포인터 진입 이벤트를 추가
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => OnPointerEnter((PointerEventData)data, index));
            eventTrigger.triggers.Add(entryEnter);

            // 포인터 나감 이벤트를 추가
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => OnPointerExit((PointerEventData)data));
            eventTrigger.triggers.Add(entryExit);
        }

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

        // 콜라보 슬롯 이펙트 찾기
        for (int classIndex = 0; classIndex < 3; classIndex++)
        {
            for (int slotIndex = 0; slotIndex < 8; slotIndex++)
            {
                string collavoSlotName = classIndex switch
                {
                    0 => $"Warrior_Collavo_Skill_{slotIndex + 1}",
                    1 => $"Archer_Collavo_Skill_{slotIndex + 1}",
                    2 => $"Mage_Collavo_Skill_{slotIndex + 1}",
                    _ => ""
                };

                collavoSlots[classIndex, slotIndex] = GameObject.Find(collavoSlotName);
            }
        }

        // 콜라보 슬롯 이펙트 찾기
        for (int classIndex = 0; classIndex < 3; classIndex++)
        {
            for (int slotIndex = 0; slotIndex < 8; slotIndex++)
            {
                string collavoSlotName = slotIndex switch
                {
                    0 => $"Warrior_Collavo_Skill_{slotIndex + 1}",
                    1 => $"Archer_Collavo_Skill_{slotIndex + 1}",
                    2 => $"Mage_Collavo_Skill_{slotIndex + 1}",
                    _ => ""
                };

                collavoSlots[classIndex, slotIndex].gameObject.SetActive(false);
            }
        }

        
        expResult = false;
        
        if(PhotonNetwork.IsMasterClient)
            StartCoroutine("WaitForAllPlayersToEnterDungeon");

        Managers.Photon.CloseRoom();

        // 콜라보 시스템 참조
        collavoSystem = FindObjectOfType<CollavoSystem>();
    }


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

        // 콜라보 발동 여부 인식
        CheckCollavo();
    }

    void OnDestroy()
    {
        // 등록된 이벤트 핸들러 해제
        KnightGController.OnBossDestroyed -= HandleBossDestroyed;
        MummyManController.OnMummyDestroyed -= HandleBossDestroyed;
        PlayerController.OnPlayerDestroyed -= HandlePlayerDestroyed;

        if (isTutorialScene)
        {
            // 튜토리얼이 끝나면 채팅 버튼과 메뉴 버튼을 다시 활성화
            openChatButton.gameObject.SetActive(true);
            openMenuButton.gameObject.SetActive(true);
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 포인터가 슬롯에 진입했을 때
    public void OnPointerEnter(PointerEventData eventData, int index)
    {
        // 특정 이름을 가진 Canvas를 찾음
        GameObject dungeonPopupUIGameObject = GameObject.Find("[Dungeon]_Dungeon_Popup_UI");
        if (dungeonPopupUIGameObject != null)
        {
            Canvas canvas = dungeonPopupUIGameObject.GetComponent<Canvas>();

            GameObject skillInfoObject = Resources.Load<GameObject>("Prefabs/UI/SubItem/Skill_Info");

            if (skillInfoObject != null) // skillInfoObject가 null이 아닌지 확인
            {
                if (skillInfoPopup == null)
                {
                    skillInfoPopup = Instantiate(skillInfoObject);
                    if (canvas != null)
                    {
                        // 팝업의 부모를 특정 캔버스로 설정.
                        skillInfoPopup.transform.SetParent(canvas.transform, false);
                    }
                }

                // 팝업 위치 조정: 마우스 위치를 기준으로 팝업 위치 설정
                Vector3 newPosition = Input.mousePosition;
                if (canvas != null)
                {
                    RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, newPosition, canvas.worldCamera, out newPosition);

                    skillInfoPopup.transform.position = newPosition;
                }

                // Skill_Info 컴포넌트 참조
                Skill_Info skillInfoComponent = skillInfoObject.GetComponent<Skill_Info>();
                string[] skills = skillSlot != null ? skillSlot.LoadedSkills : null;
                if (skillInfoComponent != null) // skillInfoComponent가 null이 아닌지 확인
                {
                    skillInfoComponent.Initialize();
                    skillInfoComponent.UpdateSkillInfo(Skill_Select.ChangeCodeToName(Managers.Player.GetClassCode()), skills[index]);
                }
                else
                {
                    Debug.LogError("Skill_Info 컴포넌트를 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogError("Skill_Info 게임 오브젝트를 찾을 수 없습니다.");
            }

            // 팝업 활성화
            skillInfoPopup.SetActive(true);

            // 팝업을 최상단으로
            skillInfoPopup.transform.SetAsLastSibling();
        }
        else
        {
            Debug.LogError("[Dungeon]_Dungeon_Popup_UI 캔버스를 찾을 수 없습니다.");
        }
    }


    // 포인터가 슬롯에서 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (skillInfoPopup != null)
        {
            // 팝업 비활성화
            skillInfoPopup.SetActive(false);
        }
    }

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
            1 => ("초급 던전", "별의 조각 평원", dungeonIcons[1]),
            2 => ("중급 던전", "잊혀진 신전", dungeonIcons[2]),
            3 => ("고급 던전", "심연의 바다", dungeonIcons[3]),
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
        // 드래곤 아이콘 비활성화
        bossIcons[4].gameObject.SetActive(false);

        // 던전에 맞는 아이콘 활성화
        for (int i = 0; i < 4; i++)
        {
            bossIcons[i].gameObject.SetActive(i == selectedDungeonNumber);
        }

        // 보스 HP 슬라이더 설정
        bossHPSlider.value = 1;

        // 보스 레벨 텍스트 설정
        bossLevelText.text = selectedDungeonNumber switch
        {
            1 => "Lv. 8",
            2 => "Lv. 14",
            3 => "Lv. 20",
            _ => "Lv. 3"
        };

        // 보스 파괴 이벤트 핸들러 등록
        KnightGController.OnBossDestroyed += HandleBossDestroyed;
        MummyManController.OnMummyDestroyed += HandleBossDestroyed;
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

        // 이프리스 체력 10% 남았을 시
        if (selectedDungeonNumber == 3 && bossHPSlider.value <= 0.10f)
        {
            // 보스 상태 창 비활성화
            bossStatus.SetActive(false);

            // 이프리스 대사 활성화
            ShowAndHideBossDialogue("나는 이프리스. 너희에게, 진정한 파멸을 보여주리라!", Color.red);
        }
    }

    // 보스 대사를 표시하고 일정 시간 후에 다시 숨기는 메서드
    public void ShowAndHideBossDialogue(string text, Color textColor)
    {
        StartCoroutine(ShowBossDialogueCoroutine(text, textColor));
    }

    private IEnumerator ShowBossDialogueCoroutine(string text, Color textColor)
    {
        // 대사와 색상 설정
        bossScriptContainer.SetActive(true);
        bossScriptText.text = text;
        bossScriptText.color = textColor;

        // 5초 동안 대기
        yield return new WaitForSeconds(5);

        // 대사 숨기기
        bossScriptContainer.SetActive(false);
    }

    // 던전 진행 슬라이더 업데이트 메서드
    public void UpdateProgress()
    {
        if (currentCheckpointIndex < totalCheckpoints)
        {
            // 체크포인트 통과시 인덱스 증가
            currentCheckpointIndex++;

            // 체크 포인트 통과 시 받는 경험치 증가
            currentExp += 10 * (currentCheckpointIndex-1);
            //Debug.Log($"{currentCheckpointIndex} 웨이브 도착 현재 경험치는 {currentExp}, 얻은 경험치는 {10 * (currentCheckpointIndex - 1)}");

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
            bossNameText.text = selectedDungeonNumber switch
            {
                0 => "숲의 요정, 플리아드",
                1 => "천체의 기사, 나이트-G",
                2 => "무덤의 지배자, 톰바크",
                3 => "심해의 수호자, 이프리스",
                _ => "알 수 없는 보스",
            };

            // 보스 등장 대사 활성화
            string[] bossDialogues = selectedDungeonNumber switch
            {
                0 => new string[]
                {
                    "숲의 왕, 플리아드가 너희의 도전을 기다렸다. 진정한 자연의 힘을 각오하라!",
                    "자연의 아름다움에 숨은 힘, 그것은 바로 나, 플리아드. 너희의 용기를 검증하겠다.",
                    "이 숲의 규칙을 바꾸려는 건가? 플리아드가 너희를 심판할 것이다!"
                },
                1 => new string[]
                {
                    "별들이 나의 길잡이, 나이트-G. 너희의 운명은 별빛 아래 결정될 것이다!",
                    "별들의 광채 속에서, 나이트-G가 천체의 힘을 빌어 나타났다.",
                    "너희에게 별의 축복이 함께하기를, 그것이 너희에게 필요할 테니."
                },
                2 => new string[]
                {
                    "으하하하하! 내 앞에서 살아남을 수 있을까?",
                    "죽음의 그림자, 톰바크가 너희를 기다린다!",
                    "어둠 속에서 깨어난 톰바크, 너희를 파멸로 이끌 것이다!"
                },
                3 => new string[]
                {
                    "심연의 파도가 나를 부른다, 심해의 수호자 이프리스. 여기가 너희의 끝이다!",
                    "해저의 어둠 속에서, 나는 너희를 삼킬 것이다!",
                    "심해의 분노를 맛보아라, 이프리스가 너희를 멸망으로 이끌 것이다!"
                },
                _ => new string[] { "" }
            };


            System.Random random = new System.Random();
            string selectedDialogue = bossDialogues[random.Next(bossDialogues.Length)];

            Color bossColor = selectedDungeonNumber switch
            {
                0 => new Color(1.0f, 0.9f, 0.5f), // 연한 노랑
                1 => new Color(0.7f, 0.9f, 1.0f), // 연한 하늘색
                2 => new Color(0.8f, 0.7f, 0.5f), // 연한 갈색
                3 => new Color(0.9f, 0.1f, 0.1f), // 진한 빨강
                _ => Color.white,
            };

            ShowAndHideBossDialogue(selectedDialogue, bossColor);
        }
    }

    // 보스 처치 시 실행되는 메서드
    private void HandleBossDestroyed()
    {
        // 보스 상태창 비활성화
        bossStatus.SetActive(false);

        // 보스 클리어
        Managers.Photon.SendDungeonEnd(timeText.text, true);

        // 던전 결과 Popup UI를 띄움
        Result_Popup_UI result = Managers.UI.ShowPopupUI<Result_Popup_UI>("[Dungeon]_Result_Popup_UI");

        if (selectedDungeonNumber!= 0)
        {
            currentExp += 30;
            currentExp += 10;

            int addExp = (selectedDungeonNumber - 1) * 5;

            currentExp *= addExp != 0 ? addExp : 1;

            long originExp = Managers.Player.GetExp();
            int originLevel = Managers.Player.GetLevel();
            long originNeedExp = Managers.Player.GetNeedExp();
            int getExp = currentExp;

            if (selectedDungeonNumber != 0)
            {
                SummaryExp(dungeonNameText.text + "던전 클리어");
            }

            result.Initialize();
            result.EarnExp(getExp, originExp, originLevel, originNeedExp, "던전 클리어!");
            Managers.Photon.SendDungeonEnd(timeText.text, false);
        }

        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        PlayerZone zone = GameObject.FindObjectOfType<PlayerZone>();
        zone.StopMovement();
        foreach(var player in players)
        {
            player.Evasion = true;          // 던전을 클리어한 뒤에 보스의 공격에 맞지 않도록 세팅
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
            Result_Popup_UI result = Managers.UI.ShowPopupUI<Result_Popup_UI>("[Dungeon]_Result_Popup_UI");

            currentExp += 10;

            int addExp = (selectedDungeonNumber - 1) * 5;
            currentExp *= addExp != 0 ? addExp : 1;
            long originExp = Managers.Player.GetExp();
            int originLevel = Managers.Player.GetLevel();
            long originNeedExp = Managers.Player.GetNeedExp();
            int getExp = currentExp;

            if (selectedDungeonNumber != 0)
            {
                SummaryExp(dungeonNameText.text + "던전에서 사망");
            }

            result.Initialize();
            result.EarnExp(getExp, originExp, originLevel, originNeedExp, "던전 공략 실패");
            Managers.Photon.SendDungeonEnd(timeText.text, false);
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
    public void SummaryExp(string message)
    {
        if (expResult) return;
        expResult = true;
        if (currentExp == 0) return;

        // 경험치 합산
        Managers.Player.AddExp(currentExp);
        EXPStatisticsReqDto dto = new EXPStatisticsReqDto();
        dto.classCode = Managers.Player.GetClassCode();
        dto.currentExp = Managers.Player.GetExp();
        dto.playerLevel = Managers.Player.GetLevel();
        dto.reason = message;
        dto.expDelta = currentExp;
        Managers.Network.EXPStatisticsCall(dto);
        //Debug.Log($"{currentExp} 얻었다넹");
        currentExp = 0;
    }

    // 스킬 슬롯 업데이트 코루틴
    private IEnumerator LoadSkillSlotDataCoroutine()
    {
        // 스킬 슬롯 참조
        foreach(var slot in FindObjectsOfType<SkillSlot>())
        {
            if (slot.GetComponent<PhotonView>().IsMine)
            {
                skillSlot = slot;
                Debug.Log(slot.gameObject.name);
                break;
            }
        }
        

        // 스킬 슬롯이 참조될때까지 기다림
        yield return null;

        // 참조 완료 후 UI 업데이트
        UpdateSlotSkillIcons();
    }

    // 스킬 슬롯 아이콘 업데이트 메서드
    public void UpdateSlotSkillIcons()
    {
        string className = Managers.Player.GetClassCode() switch
        {
            "C001" => "Warrior",
            "C002" => "Archer",
            "C003" => "Mage",
            _ => ""
        };

        string[] skills = skillSlot != null ? skillSlot.LoadedSkills : null;

        if (skills == null)
        {
            // 스킬이 없는 경우 모든 슬롯을 기본 이미지로 설정
            FillRemainingSlots(0);
            return;
        }

        // 스킬 개수만큼 반복
        for (int i = 0; i < skills.Length; i++)
        {
            Sprite sprite = LoadSkillSprite(className, skills[i]);

            // 이미지 컴포넌트에 Sprite 할당
            if (sprite != null && skillIcons[i] != null)
            {
                skillIcons[i].sprite = sprite;
            }
        }

        // 남은 슬롯들을 기본 이미지로 채우기
        if (skills.Length < skillIcons.Length)
        {
            FillRemainingSlots(skills.Length);
        }
    }

    // 남은 슬롯들을 기본 이미지로 채우는 메서드
    private void FillRemainingSlots(int startIndex)
    {
        for (int i = startIndex; i < skillIcons.Length; i++)
        {
            if (skillIcons[i] != null)
            {
                skillIcons[i].sprite = Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image");
            }
        }
    }

    // 스킬 아이콘 로드 메서드
    private Sprite LoadSkillSprite(string className, string skillName)
    {
        // 스킬 아이콘 로드 시도
        Sprite loadedSprite = Resources.Load<Sprite>($"Sprites/SkillIcon/{className}/{skillName}");

        // 로드된 스킬 아이콘이 null이면 기본 이미지를 반환, 있다면 로드된 스킬 아이콘을 반환
        return loadedSprite == null ? Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image") : loadedSprite;
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

    // 콜라보 발동 여부 체크하는 메서드
    private void CheckCollavo()
    {
        if (skillSlot == null) return;

        int classNumber = Managers.Player.GetClassCode() switch
        {
            "C001" => 0,
            "C002" => 1,
            "C003" => 2,
            _ => 3
        };

        for (int i = 0; i < 8; i++)
        {
            int slotNumber = i;
            var skill = skillSlot.Skills[slotNumber];

            if (skill == null)
            {
                //Debug.Log("없는 스킬입니다. 콜라보 관련 설정을 하지 않습니다.");
                continue;
            }

            if (skill.SkillType != Define.SkillType.Holding)
            {
                // 이펙트 제거
                collavoSlots[classNumber, slotNumber].gameObject.SetActive(false);
                continue;
            }

            if (collavoSystem.IsCastingSkill(skill.CollavoSkillName))
            {
                // 이펙트 활성화
                collavoSlots[classNumber, slotNumber].gameObject.SetActive(true);
            }
            else
            {
                // 이펙트 제거
                collavoSlots[classNumber, slotNumber].gameObject.SetActive(false);
            }
        }
    }


    IEnumerator WaitForAllPlayersToEnterDungeon()
    {
        bool allPlayersReady = false;
        // 플레이어들의 준비 상태를 확인합니다.
        while (!allPlayersReady)
        {
            allPlayersReady = CheckAllPlayersReady();

            // 상태를 1초마다 확인합니다.
            yield return new WaitForSeconds(1f);
        }
        // 모든 플레이어가 준비되면 던전 시작 로직을 실행합니다.
        FindObjectOfType<PhotonChat>().CreateParty();
    }

    // 모든 플레이어의 준비 상태를 확인하는 함수
    private bool CheckAllPlayersReady()
    {
        Define.Scene sceneName = (Define.Scene)PhotonNetwork.MasterClient.CustomProperties["currentScene"];
        foreach (var player in PhotonNetwork.PlayerList)
        {

            if (player.CustomProperties.TryGetValue("currentScene", out object obj))
            {
                Define.Scene currentScene = (Define.Scene)obj;
                if (currentScene != sceneName)
                {
                    Debug.Log("현재 없는 플레이어가 있습니다.");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true; // 모든 플레이어가 준비되었습니다.
    }
}