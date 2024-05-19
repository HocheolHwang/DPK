using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

        // My 레벨 랭킹 캐릭터 이미지
        My_Character_Image,

        // My 랭킹 아이템 컨테이너
        My_Ranking_Item_Container
    }

    enum Texts
    {
        // 랭킹 목록 제목 텍스트
        Character_Title_Text,
        Nickname_Title_Text,
        Record_Title_Text,

        // 내 랭킹 타이틀 텍스트
        My_Ranking_Title_Text,

        // My 랭킹 등수 텍스트
        My_Ranking_Text,

        // 파티 이름 텍스트
        Party_Name_Text_1,
        Party_Name_Text_2,
        Party_Name_Text_3,
        Party_Name_Text_4,
        Party_Name_Text_5,

        // My 파티 이름 텍스트
        My_Party_Name_Text,

        // 닉네임 / 파티원 닉네임 텍스트
        Nickname_Text_1,
        Nickname_Text_2,
        Nickname_Text_3,
        Nickname_Text_4,
        Nickname_Text_5,

        // My 닉네임 / 파티원 닉네임 텍스트
        My_Nickname_Text,

        // 레벨 / 클리어 시간 텍스트
        Record_Text_1,
        Record_Text_2,
        Record_Text_3,
        Record_Text_4,
        Record_Text_5,

        // My 레벨 / 클리어 시간 텍스트
        My_Record_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button[] rangkingButtons = new Button[4];
    private Button cancelButton;
    private Image[] inactiveBackgrounds = new Image[4];
    private Image[] characterImages = new Image[5];
    private Image myCharacterImage;
    private Image myRankingItemContainer;
    private TextMeshProUGUI[] titleTexts = new TextMeshProUGUI[3];
    private TextMeshProUGUI myRankingTitleText;
    private TextMeshProUGUI myRankingText;
    private TextMeshProUGUI[] partyNameTexts = new TextMeshProUGUI[5];
    private TextMeshProUGUI myPartyNameText;
    private TextMeshProUGUI[] nicknameTexts = new TextMeshProUGUI[5];
    private TextMeshProUGUI myNicknameText;
    private TextMeshProUGUI[] recordTexts = new TextMeshProUGUI[5];
    private TextMeshProUGUI myRecordText;

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
            partyNameTexts[i] = GetText((int)Texts.Party_Name_Text_1 + i);
            nicknameTexts[i] = GetText((int)Texts.Nickname_Text_1 + i);
            recordTexts[i] = GetText((int)Texts.Record_Text_1 + i);
        }

        // My 랭킹 아이템 컨테이너 초기화
        myRankingItemContainer = GetImage((int)Images.My_Ranking_Item_Container);

        // My 랭킹 아이템 초기화
        myRankingTitleText = GetText((int)Texts.My_Ranking_Title_Text);
        myRankingText = GetText((int)Texts.My_Ranking_Text);
        myCharacterImage = GetImage((int)Images.My_Character_Image);
        myPartyNameText = GetText((int)Texts.My_Party_Name_Text);
        myNicknameText = GetText((int)Texts.My_Nickname_Text);
        myRecordText = GetText((int)Texts.My_Record_Text);

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        Managers.Network.PlayerRankCall(5, SavePlayerRankList);
        Managers.Network.DungeonRankCall("1", SaveFirstDungeonRankList);

        // 레벨 랭킹이 눌러진 상태로 시작
        ViewRanking(0, null);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 랭킹 초기화
    private void initialize()
    {
        // 랭킹 아이템 초기화
        for (int i = 0; i < partyNameTexts.Length; i++)
        {
            partyNameTexts[i].text = "";
            nicknameTexts[i].text = "";
            recordTexts[i].text = "";
        }
    }

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
        titleTexts[0].text = isLevelRanking ? "캐릭터" : "파티 이름";
        titleTexts[1].text = isLevelRanking ? "닉네임" : "파티원";
        titleTexts[2].text = isLevelRanking ? "레벨" : "클리어 시간";

        for (int i = 0; i < characterImages.Length; i++)
        {
            // 캐릭터 이미지 및 파티 이름 텍스트 활성화/비활성화
            characterImages[i].gameObject.SetActive(isLevelRanking);
            partyNameTexts[i].gameObject.SetActive(!isLevelRanking);
        }

        // 타이틀 텍스트 변경
        myRankingTitleText.text = isLevelRanking ? "내 랭킹" : "이 달의 우수 기사";

        // My 랭킹 아이템 활성화/비활성화
        myRankingItemContainer.gameObject.SetActive(isLevelRanking);

        // My 캐릭터 이미지 및 My 파티 이름 텍스트 활성화/비활성화
        myCharacterImage.gameObject.SetActive(isLevelRanking);
        myPartyNameText.gameObject.SetActive(!isLevelRanking);
    }

    // 레벨 랭킹 업데이트 메서드
    private void UpdateLevelRanking()
    {
        initialize();

        // --------------- 레벨 랭킹 업데이트 ---------------
        for (int i = 0; i < characterImages.Length; i++)
        {
            // playerRankList와 rankingList가 null이 아닌지 확인
            if (playerRankList != null && playerRankList.rankingList != null)
            {
                var rankItem = playerRankList.rankingList[i];

                // 직업 사진 가져오기
                if (rankItem != null && !string.IsNullOrEmpty(rankItem.className))
                {
                    string className = rankItem.className;
                    characterImages[i].sprite = Resources.Load<Sprite>($"Sprites/Class Icon/{className}");
                }

                // 닉네임 가져오기
                if (rankItem != null && !string.IsNullOrEmpty(rankItem.nickname))
                {
                    string nickName = rankItem.nickname;
                    nicknameTexts[i].text = nickName;
                }

                // 등수 가져오기
                if (rankItem != null)
                {
                    int level = rankItem.playerLevel;
                    recordTexts[i].text = level.ToString();
                }
            }
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
        myRecordText.text = highestLevel.ToString();

        // My 랭킹 등수
        myRankingText.text = "-";
    }

    // 던전 랭킹 업데이트 메서드
    private void UpdateDungeonRanking()
    {
        initialize();

        // --------------- 던전 랭킹 업데이트 ---------------

        int rankSize = recordTexts.Length;
        if (recordTexts.Length > dungeonRankList[selectedRankingNumber-1].rankingList.Length)
        {
            rankSize = dungeonRankList[selectedRankingNumber-1].rankingList.Length;
        }

        for (int i = 0; i < dungeonRankList[selectedRankingNumber - 1].rankingList.Length; i++)
        {
            DungeonResDto curRank = dungeonRankList[selectedRankingNumber-1].rankingList[i];
            
            // 클리어 시간
            float minutes = curRank.clearTime / 60.0f;
            float seconds = curRank.clearTime % 60.0f;
            string timeText = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(minutes), Mathf.FloorToInt(seconds));
            string clearTime = timeText;
            recordTexts[i].text = clearTime;

            // 파티 이름
            string partyName = curRank.partyTitle;
            partyNameTexts[i].text = partyName;

            // 파티원 닉네임 목록
            string partyList = "";
            
            for(int j = 0; j< curRank.playerList.Length; j++)
            {
                partyList += curRank.playerList[j] + ", ";
            }

            // 마지막 쉼표와 공백 제거
            partyList = partyList.TrimEnd(',', ' ');
            nicknameTexts[i].text = partyList;
        }


        // --------------- My 레벨 랭킹 업데이트 ---------------

        myPartyNameText.text = "";
        myNicknameText.text = "";
        myRecordText.text = "";
        myRankingText.text = "";
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
