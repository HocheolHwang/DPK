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
    private TextMeshProUGUI[] clearTimeTexts = new TextMeshProUGUI[5];
    private TextMeshProUGUI myClearTimeText;
    private TextMeshProUGUI[] nicknameTexts = new TextMeshProUGUI[5];
    private TextMeshProUGUI myNicknameText;
    private TextMeshProUGUI[] levelTexts = new TextMeshProUGUI[5];
    private TextMeshProUGUI myLevelText;

    // 선택된 랭킹 번호
    private int selectedRankingNumber = 0;


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

        // my 랭킹 아이템 초기화
        myCharacterImage = GetImage((int)Images.My_Character_Image);
        myClearTimeText = GetText((int)Texts.My_Clear_Time_Text);
        myNicknameText = GetText((int)Texts.My_Nickname_Text);
        myLevelText = GetText((int)Texts.My_Level_Text);

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 레벨 랭킹이 눌러진 상태로 시작
        ViewRanking(0, null);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 랭킹 보기 메서드
    private void ViewRanking(int rankingNumber, PointerEventData data)
    {
        selectedRankingNumber = rankingNumber;
        SetRankingButtonInactive();
        UpdateRankingDisplay();
    }

    // 랭킹 버튼 비활성화 메서드
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

        // my 캐릭터 이미지 및 my 클리어 시간 텍스트 활성화/비활성화
        myCharacterImage.gameObject.SetActive(isLevelRanking);
        myClearTimeText.gameObject.SetActive(!isLevelRanking);
    }

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }
}
