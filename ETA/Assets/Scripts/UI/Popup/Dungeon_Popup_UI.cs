using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using PlayerStates;
using Photon.Pun;

public class Dungeon_Popup_UI : UI_Popup
{
    // 게임오브젝트 인덱스
    enum GameObjects
    {
        Tutorial_Icon,
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
        Player_Tier_Text,
        Player_Nickname_Text,
        Player_HP_Text,
        Boss_HP_Text,
        Boss_Name_Text,
    }

    // Slider 인덱스
    enum Sliders
    {
        Dungeon_Progress_Bar,
        Member_HP_Slider_1,
        Boss_HP_Slider,
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
    private GameObject tutorialIcon;
    private GameObject deepForestIcon;
    private GameObject forgottenTempleIcon;
    private GameObject starShardPlainIcon;
    private GameObject bossStatus;
    private TextMeshProUGUI dungeonNameText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI memberNicknameText1;
    private TextMeshProUGUI playerTierText;
    private TextMeshProUGUI playerNicknameText;
    private TextMeshProUGUI playerHPText;
    private TextMeshProUGUI bossHPText;
    private Slider dungeonProgressBar;
    private Slider memberHPSlider1;
    private Slider bossHPSlider;
    private Slider playerHPSlider;
    private Slider playerEXPSlider;
    public Transform[] checkpoints;
    private int totalCheckpoints = 3;
    private int currentCheckpointIndex = 0;
    private float gameTime = 0f;

    private Button openChatButton;
    private Button openMenuButton;

    public Stat playerStat;
    public Stat bossStat;

    // 현재 Scene이 튜토리얼 Scene인지 확인
    private bool isTutorialScene;


    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
        Bind<Button>(typeof(Buttons));

        isTutorialScene = SceneManager.GetActiveScene().name == "Tutorial";

        // 닉네임
        memberNicknameText1 = GetText((int)Texts.Member_Nickname_Text_1);
        playerTierText = GetText((int)Texts.Player_Tier_Text);

        if (isTutorialScene)
        {
            playerTierText.text = "던전처리기사 수험생";
        }
        else
        {
            playerTierText.text = "견습 던전처리기사";
        }

        playerNicknameText = GetText((int)Texts.Player_Nickname_Text);
        UpdateUserInfo();

        // 던전 아이콘
        tutorialIcon = GetObject((int)GameObjects.Tutorial_Icon);
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

        bossHPText = GetText((int)Texts.Boss_HP_Text);
        bossHPSlider = GetSlider((int)Sliders.Boss_HP_Slider);
        bossHPSlider.value = 1;

        playerHPText = GetText((int)Texts.Player_HP_Text);
        playerHPSlider = GetSlider((int)Sliders.Player_HP_Slider);
        playerHPSlider.value = 1;
        memberHPSlider1 = GetSlider((int)Sliders.Member_HP_Slider_1);
        memberHPSlider1.value = 1;

        playerEXPSlider = GetSlider((int)Sliders.Player_EXP_Slider);

        // TODO : 내 캐릭터 찾기

        //GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log(playerObjects);

        //foreach(var playerObj in playerObjects)
        //{
        //    Debug.Log(playerObj.name);
        //    if (playerObj.GetComponent<PhotonView>().IsMine)
        //    {
        //        playerStat = playerObj.GetComponent<Stat>();
        //        break;
        //    }
            
        //}

        //Debug.Log(playerStat);
        //GameObject playerObject = GameObject.FindWithTag("Player");
        //if (playerObject != null)
        //{
        //    playerStat = playerObject.GetComponent<Stat>();
        //}


        KnightGController.OnBossDestroyed += HandleBossDestroyed;


        openChatButton = GetButton((int)Buttons.Open_Chat_Button);
        openMenuButton = GetButton((int)Buttons.Open_Menu_Button);
        if (isTutorialScene)
        {
            // 튜토리얼 Scene에서 채팅 및 메뉴 버튼 숨김
            openChatButton.gameObject.SetActive(false);
            openMenuButton.gameObject.SetActive(false);
        }
        else
        {
            // 채팅 Popup UI 열기 버튼 이벤트 등록
            AddUIEvent(openChatButton.gameObject, OpenChat);
            AddUIKeyEvent(openChatButton.gameObject, () => OpenChat(null), KeyCode.C);

            // 메뉴 Popup UI 열기 버튼 이벤트 등록
            AddUIEvent(openMenuButton.gameObject, OpenMenu);
            AddUIKeyEvent(openMenuButton.gameObject, () => OpenMenu(null), KeyCode.Escape);
        }
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

        if (isTutorialScene)
        {
            // 튜토리얼이 끝나면 채팅 버튼과 메뉴 버튼을 다시 활성화
            openChatButton.gameObject.SetActive(true);
            openMenuButton.gameObject.SetActive(true);
        }
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
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 0);
        tutorialIcon.SetActive(false);
        deepForestIcon.SetActive(false);
        forgottenTempleIcon.SetActive(false);
        starShardPlainIcon.SetActive(false);

        // 현재 Scene이 튜토리얼 Scene일 경우 선택된 던전 정보를 0으로 만듬
        if (isTutorialScene) selectedDungeonNumber = 0;
        
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
                dungeonNameText.text = "던전처리기사 실기 시험장";
                tutorialIcon.SetActive(true);
                break;
        }
    }

    public void UpdateHP()
    {
        // 체력 업데이트

        if(playerStat != null)
        {
            playerHPText.text = $"{playerStat.Hp} / {playerStat.MaxHp}";
            playerHPSlider.value = (float)playerStat.Hp / playerStat.MaxHp;

            memberHPSlider1.value = (float)playerStat.Hp / playerStat.MaxHp;
        }
        else
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log(playerObjects);

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


        if (bossStatus.activeSelf == false) return;
        // 보스 체력 업데이트
        bossHPText.text = $"{bossStat.Hp} / {bossStat.MaxHp}";
        bossHPSlider.value = (float)bossStat.Hp / bossStat.MaxHp;
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
            BossMonster bossObject = FindObjectOfType<BossMonster>();
            if (bossObject != null)
            {
                bossStat = bossObject.GetComponent<Stat>();
            }
            // 보스 상태창을 염
            bossStatus.SetActive(true);
            GetText((int)Texts.Boss_Name_Text).text = bossStat.gameObject.name;
        }
    }

    // 던전 결과 Popup UI 열기
    private void HandleBossDestroyed()
    {
        Managers.UI.ShowPopupUI<Result_Popup_UI>("[Dungeon]_Result_Popup_UI");
        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        PlayerZone zone = GameObject.FindObjectOfType<PlayerZone>();
        zone.StopMovement();
        foreach(var player in players)
        {
           
            player.isFinished = true;
        }
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
