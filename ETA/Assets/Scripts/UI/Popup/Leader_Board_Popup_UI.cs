using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Leader_Board_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        // 랭킹 버튼
        Level_Rangking_Button,
        StarShardPlain_Ranking_Button,
        ForgottenTemple_Ranking_Button,
        SeaOfAbyss_Ranking_Button,

        // 닫기 버튼
        Cancel_Button
    }

    enum Images
    {
        // 버튼 비활성화 이미지
        Level_Rangking_Inactive_Background,
        StarShardPlain_Ranking_Inactive_Background,
        ForgottenTemple_Ranking_Inactive_Background,
        SeaOfAbyss_Ranking_Inactive_Background,

        // 레벨 랭킹 캐릭터 이미지
        Character_Image_1,
        Character_Image_2,
        Character_Image_3,
        Character_Image_4,
        Character_Image_5,

        // 레벨 랭킹 캐릭터 My 이미지
        My_Character_Image
    }

    enum Texts
    {
        // 랭킹 목록 제목 텍스트
        Character_Title_Text,
        Nickname_Title_Text,
        Level_Title_Text,

        // 랭킹 등수 My 텍스트
        My_Ranking_Text,

        // 클리어 시간 텍스트
        Clear_Time_Text_1,
        Clear_Time_Text_2,
        Clear_Time_Text_3,
        Clear_Time_Text_4,
        Clear_Time_Text_5,

        // 클리어 시간 My 텍스트
        My_Clear_Time_Text,

        // 닉네임 / 파티 이름 텍스트
        Nickname_Text_1,
        Nickname_Text_2,
        Nickname_Text_3,
        Nickname_Text_4,
        Nickname_Text_5,

        // 닉네임 / 파티 이름 My 텍스트
        My_Nickname_Text,

        // 레벨 / 파티 레벨 텍스트
        Level_Text_1,
        Level_Text_2,
        Level_Text_3,
        Level_Text_4,
        Level_Text_5,

        // 레벨 / 파티 레벨 My 텍스트
        My_Level_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button[] rangkingButtons = new Button[4];
    private Button cancelButton;
    private Image[] inactiveBackgrounds = new Image[4];
    private Image[] characterImages = new Image[5];
    private Image myCharacterImage;
    private TextMeshProUGUI[] titleTexts = new TextMeshProUGUI[3];
    private TextMeshProUGUI myRankingText;
    private TextMeshProUGUI[] clearTimeTexts = new TextMeshProUGUI[5];
    private TextMeshProUGUI myClearTimeText;
    private TextMeshProUGUI[] nicknameTexts = new TextMeshProUGUI[5];
    private TextMeshProUGUI myNicknameText;
    private TextMeshProUGUI[] levelTexts = new TextMeshProUGUI[5];
    private TextMeshProUGUI myLevelText;

    // 선택된 랭킹 번호
    private int selectedRankingNumber = 0;

    // UI가 호출되면 3개 던전 저장
    private DungeonRankListResDto[] dungeonRankList = new DungeonRankListResDto[3];
    private PlayerRankResDto playerRankList;

    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 랭킹 버튼 초기화
        for (int i = 0; i < rangkingButtons.Length; i++)
        {
            int rankingNumber = i;
            rangkingButtons[i] = GetButton((int)Buttons.Level_Rangking_Button + i);
            inactiveBackgrounds[i] = GetImage((int)Images.Level_Rangking_Inactive_Background + i);

            // 랭킹 버튼에 이벤트 등록
            AddUIEvent(rangkingButtons[i].gameObject, (PointerEventData data) => ViewRanking(rankingNumber, data));
        }

        // 랭킹 목록 제목 초기화
        for (int i = 0; i < titleTexts.Length; i++)
        {
            titleTexts[i] = GetText((int)Texts.Character_Title_Text + i);
        }

        // 랭킹 아이템 초기화
        for (int i = 0; i < characterImages.Length; i++)
        {
            characterImages[i] = GetImage((int)Images.Character_Image_1 + i);
            clearTimeTexts[i] = GetText((int)Texts.Clear_Time_Text_1 + i);
            nicknameTexts[i] = GetText((int)Texts.Nickname_Text_1 + i);
            levelTexts[i] = GetText((int)Texts.Level_Text_1 + i);
        }

        // My 랭킹 아이템 초기화
        myRankingText = GetText((int)Texts.My_Ranking_Text);
        myCharacterImage = GetImage((int)Images.My_Character_Image);
        myClearTimeText = GetText((int)Texts.My_Clear_Time_Text);
        myNicknameText = GetText((int)Texts.My_Nickname_Text);
        myLevelText = GetText((int)Texts.My_Level_Text);

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        Managers.Network.PlayerRankCall(5, SavePlayerRankList);
        Managers.Network.DungeonRankCall("1", SaveFirstDungeonRankList);

        // 레벨 랭킹이 눌러진 상태로 시작
        ViewRanking(0, null);
    }

    private void initialize()
    {
        // 랭킹 목록 제목 초기화
        for (int i = 0; i < titleTexts.Length; i++)
        {
            titleTexts[i].text = "";
        }

        // 랭킹 아이템 초기화
        for (int i = 0; i < characterImages.Length; i++)
        {
            clearTimeTexts[i].text = "";
            nicknameTexts[i].text = "";
            levelTexts[i].text = "";
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 랭킹 보기 메서드
    private void ViewRanking(int rankingNumber, PointerEventData data)
    {
        // 선택된 랭킹 번호 업데이트
        selectedRankingNumber = rankingNumber;

        // 랭킹 버튼 비활성화 이미지 설정
        SetRankingButtonInactive();

        // 레벨 랭킹과 던전 랭킹 구분
        UpdateRankingDisplay();

        // 선택된 랭킹 업데이트
        (selectedRankingNumber == 0 ? (Action)UpdateLevelRanking : UpdateDungeonRanking)();

    }

    // 랭킹 버튼 비활성화 이미지 설정 메서드
    private void SetRankingButtonInactive()
    {
        for (int i = 0; i < inactiveBackgrounds.Length; i++)
        {
            inactiveBackgrounds[i].gameObject.SetActive(i != selectedRankingNumber);
        }
    }

    // 레벨 랭킹과 던전 랭킹을 구분하는 메서드
    private void UpdateRankingDisplay()
    {
        // 레벨 랭킹 여부
        bool isLevelRanking = selectedRankingNumber == 0;

        // 랭킹 타이틀 수정
        titleTexts[0].text = isLevelRanking ? "캐릭터" : "클리어 시간";
        titleTexts[1].text = isLevelRanking ? "닉네임" : "파티 이름";
        titleTexts[2].text = isLevelRanking ? "레벨" : "파티 레벨";

        for (int i = 0; i < characterImages.Length; i++)
        {
            // 캐릭터 이미지 및 클리어 시간 텍스트 활성화/비활성화
            characterImages[i].gameObject.SetActive(isLevelRanking);
            clearTimeTexts[i].gameObject.SetActive(!isLevelRanking);
        }

        // My 캐릭터 이미지 및 My 클리어 시간 텍스트 활성화/비활성화
        myCharacterImage.gameObject.SetActive(isLevelRanking);
        myClearTimeText.gameObject.SetActive(!isLevelRanking);
    }

    // 레벨 랭킹 업데이트 메서드
    private void UpdateLevelRanking()
    {
        initialize();

        // --------------- 레벨 랭킹 업데이트 ---------------

        for (int i = 0; i < characterImages.Length; i++)
        {
            // @@@@@@@@ TODO: 해당 등수 캐릭터의 직업 코드를 가져오는 코드 필요 @@@@@@@@
            
            string className = playerRankList.rankingList[i].className switch
            {
                "워리어" => "Warrior",
                "아처" => "Archer",
                "위자드" => "Mage",
                _ => ""
            };

            characterImages[i].sprite = Resources.Load<Sprite>($"Sprites/Class Icon/{className}");

            // @@@@@@@@ TODO: 해당 등수 캐릭터의 닉네임을 가져오는 코드 필요 @@@@@@@@
            string nickName = playerRankList.rankingList[i].nickname;//"응애";
            nicknameTexts[i].text = nickName;

            // @@@@@@@@ TODO: 해당 등수 캐릭터의 레벨을 가져오는 코드 필요 @@@@@@@@
            int level = playerRankList.rankingList[i].playerLevel;
            levelTexts[i].text = level.ToString();
        }


        // --------------- My 레벨 랭킹 업데이트 ---------------

        // 각 직업의 레벨을 변수에 할당
        int archerLevel = Managers.Player.GetArcherLevel();
        int warriorLevel = Managers.Player.GetWarriorLevel();
        int mageLevel = Managers.Player.GetMageLevel();

        // 가장 높은 레벨을 가진 직업의 이름을 저장할 변수 초기화
        string highestLevelClassName = "Warrior"; // 기본값 전사로 설정
        int highestLevel = warriorLevel;

        // 궁수의 레벨이 현재 가장 높은 레벨보다 높은지 확인
        if (archerLevel > highestLevel)
        {
            highestLevelClassName = "Archer";
            highestLevel = archerLevel;
        }

        // 마법사의 레벨이 현재 가장 높은 레벨보다 높은지 확인
        if (mageLevel > highestLevel)
        {
            highestLevelClassName = "Mage";
            highestLevel = mageLevel;
        }

        // My 랭킹 업데이트
        myCharacterImage.sprite = Resources.Load<Sprite>($"Sprites/Class Icon/{highestLevelClassName}");
        myNicknameText.text = Managers.Player.GetNickName();
        myLevelText.text = highestLevel.ToString();

        // @@@@@@@@ TODO: 레벨 랭킹 My 등수를 가져오는 코드 필요 @@@@@@@@
        myRankingText.text = $"{99}st";
    }

    // 던전 랭킹 업데이트 메서드
    private void UpdateDungeonRanking()
    {
        initialize();
        // --------------- 던전 랭킹 업데이트 ---------------

        int rankSize = clearTimeTexts.Length;
        if (clearTimeTexts.Length > dungeonRankList[selectedRankingNumber-1].rankingList.Length)
        {
            rankSize = dungeonRankList[selectedRankingNumber-1].rankingList.Length;
        }

        for (int i = 0; i < dungeonRankList[selectedRankingNumber - 1].rankingList.Length; i++)
        {
            DungeonResDto curRank = dungeonRankList[selectedRankingNumber-1].rankingList[i];
            // @@@@@@@@ TODO: 해당 등수 클리어 시간을 가져오는 코드 필요 @@@@@@@@
            float minutes = curRank.clearTime / 60.0f;
            float seconds = curRank.clearTime % 60.0f;
            string timeText = string.Format("{0:00}:{1:00}", Mathf.RoundToInt(minutes), Mathf.RoundToInt(seconds));
            string clearTime = timeText;
            clearTimeTexts[i].text = clearTime;

            // @@@@@@@@ TODO: 해당 등수 파티 이름을 가져오는 코드 필요 @@@@@@@@
            string partyName = curRank.partyTitle;
            nicknameTexts[i].text = partyName;

            // @@@@@@@@ TODO: 해당 등수 파티 레벨을 가져오는 코드 필요 @@@@@@@@
            int partyLevel = 55;
            string partyList = "";
            
            for(int j = 0; j< curRank.playerList.Length; j++)
            {
                partyList += curRank.playerList[j] + ", ";
            }
            levelTexts[i].text = partyList;//partyLevel.ToString();
        }


        // --------------- My 레벨 랭킹 업데이트 ---------------

        // @@@@@@@@ TODO: 해당 등수 클리어 시간을 가져오는 코드 필요 @@@@@@@@
        myClearTimeText.text = "00:00:30";

        // @@@@@@@@ TODO: 해당 등수 파티 이름을 가져오는 코드 필요 @@@@@@@@
        myNicknameText.text = "내 파티라고";

        // @@@@@@@@ TODO: 해당 등수 파티 레벨을 가져오는 코드 필요 @@@@@@@@
        myLevelText.text = "321";

        // @@@@@@@@ TODO: 내 등수를 가져오는 코드 필요 @@@@@@@@
        myRankingText.text = $"{selectedRankingNumber}st";
    }

    public void SavePlayerRankList(PlayerRankResDto dto)
    {
        playerRankList = dto;
        UpdateLevelRanking();
    }

    public void SaveFirstDungeonRankList(DungeonRankListResDto dto)
    {
        dungeonRankList[0] = dto;
        Managers.Network.DungeonRankCall("2", SaveSecondDungeonRankList);
    }
    public void SaveSecondDungeonRankList(DungeonRankListResDto dto)
    {
        dungeonRankList[1] = dto;
        Managers.Network.DungeonRankCall("3", SaveThirdDungeonRankList);
    }
    public void SaveThirdDungeonRankList(DungeonRankListResDto dto)
    {
        dungeonRankList[2] = dto;
    }
}
