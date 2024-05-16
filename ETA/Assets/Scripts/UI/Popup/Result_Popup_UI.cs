using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;

public class Result_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Load_Lobby_Button
    }

    enum Texts
    {
        // 던전 결과
        Result_Title_Text,

        // 전투 시간
        Time_Text,

        // 던전 이름
        Dungeon_Name_Text,

        // 최고 기록 타이틀
        Best_Record_Title_Text,

        // 최고 기록
        Best_Record_Text,

        // 획득 경험치
        EXP_Text,

        // 전후 레벨
        Before_Level_Text,
        After_Level_Text,

        // 전후 경험치
        Before_EXP_Text,
        After_EXP_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button loadLobbyButton;
    private TextMeshProUGUI resultTitleText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI dungeonNameText;
    private TextMeshProUGUI bestRecordTitleText;
    private TextMeshProUGUI bestRecordText;
    private TextMeshProUGUI expText;
    private TextMeshProUGUI[] levelTexts = new TextMeshProUGUI[2];
    private TextMeshProUGUI[] expTexts = new TextMeshProUGUI[2];

    // Dungeon_Popup_UI 인스턴스에 대한 참조
    private Dungeon_Popup_UI dungeonPopupUI;

    // ---------------------------- 변수 -------------------------------------
    private int curExp;
    private long originExp;
    private int originLevel;
    private long originNeedExp;
    private string dungeonResultText = "던전 클리어!";


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 텍스트 초기화
        resultTitleText = GetText((int)Texts.Result_Title_Text);
        timeText = GetText((int)Texts.Time_Text);
        dungeonNameText = GetText((int)Texts.Dungeon_Name_Text);
        bestRecordTitleText = GetText((int)Texts.Best_Record_Title_Text);
        bestRecordText = GetText((int)Texts.Best_Record_Text);
        expText = GetText((int)Texts.EXP_Text);

        // 경험치 획득 전후 레벨 및 경험치 초기화
        for (int i = 0; i < levelTexts.Length; i++)
        {
            levelTexts[i] = GetText((int)Texts.Before_Level_Text + i);
            expTexts[i] = GetText((int)Texts.Before_EXP_Text + i);
        }

        // dungeonPopupUI 찾기
        dungeonPopupUI = FindObjectOfType<Dungeon_Popup_UI>();

        // 기본 정보 업데이트
        UpdateBasicInformation();

        // 던전 결과 업데이트
        UpdateDungeonResult();

        // 로비로 이동하기 버튼 이벤트 등록
        loadLobbyButton = GetButton((int)Buttons.Load_Lobby_Button);
        AddUIEvent(loadLobbyButton.gameObject, LoadLobbyScene);
        AddUIKeyEvent(loadLobbyButton.gameObject, () => LoadLobbyScene(null), KeyCode.Return);

        Debug.Log("여기서 초기화가ㅏ 됩니다.");
    }

    public void Initialize()
    {
        // 텍스트 초기화
        resultTitleText = GetText((int)Texts.Result_Title_Text);
        timeText = GetText((int)Texts.Time_Text);
        dungeonNameText = GetText((int)Texts.Dungeon_Name_Text);
        bestRecordTitleText = GetText((int)Texts.Best_Record_Title_Text);
        bestRecordText = GetText((int)Texts.Best_Record_Text);
        expText = GetText((int)Texts.EXP_Text);
    }

    // ------------------------------ 메서드 정의 ------------------------------

    // 기본 정보 업데이트 메서드
    private void UpdateBasicInformation()
    {
        // 시간 업데이트
        timeText.text = dungeonPopupUI.timeText.text;

        // 던전 이름 업데이트
        dungeonNameText.text = FindObjectOfType<GameSystem>().currentDungeonNum switch
        {
            1 => "별의 조각 평원",
            2 => "잊혀진 신전",
            3 => "심연의 바다",
            _ => ""
        };

        // @@@@@@@@ TODO: 해당 던전의 최고 기록을 가져오는 코드 필요 @@@@@@@@
        bestRecordText.text = ""; // 최고 기록
    }

    // 던전 결과 업데이트 메서드
    private void UpdateDungeonResult()
    {
        // @@@@@@@@ TODO: 던전 클리어 여부를 확인하는 코드 필요 @@@@@@@@

        // ----------------- 사망 시 -----------------
         resultTitleText.text = dungeonResultText;

        // 최고기록 타이틀
        bestRecordTitleText.text = "최고 기록";


        // ----------------- 클리어 시 -----------------
        // resultTitleText.text = "던전 클리어!";

        // 클리어 했을 시 && 최고 기록일 시
        //bestRecordTitleText.text = "최고 기록 (NEW!)";
        //bestRecordText.text = timeText.text; // 최고 기록 갱신

        // @@@@@@@@ TODO: 클리어 시 획득 경험치 가져오는 코드 필요 @@@@@@@@
        expText.text = $"{curExp}";

        // @@@@@@@@ TODO: 직전 레벨 및 경험치와 이후 레벨 및 경험치 가져오는 코드 필요 @@@@@@@@

        levelTexts[0].text = $"Lv. {originLevel}";
        expTexts[0].text = $"{((float)originExp / originNeedExp) * 100.0f} %";
        
        levelTexts[1].text = $"Lv. {Managers.Player.GetLevel()}";
        expTexts[1].text = $"{((float)Managers.Player.GetExp() / (float)Managers.Player.GetNeedExp()) * 100.0f} %";
            
    }

    // 로비로 이동하기 메서드
    private void LoadLobbyScene(PointerEventData data)
    {
        // 씬 이동하기 전에 모든 스택을 비움
        CloseAllPopupUI();

        // 로비 씬으로 이동
        Managers.Scene.LoadScene(Define.Scene.Lobby);
    }

    public void EarnExp(int currentExp, long exp, int level, long needExp, string resultText)
    {
        Init();
        curExp = currentExp;
        originExp = exp;
        originLevel = level;
        originNeedExp = needExp;
        dungeonResultText = resultText;
        UpdateDungeonResult();
    }
}
