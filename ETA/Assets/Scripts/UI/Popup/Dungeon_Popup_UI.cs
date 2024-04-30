using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class Dungeon_Popup_UI : UI_Popup
{
    // 게임오브젝트 인덱스
    enum GameObjects
    {
        Boss_Status
    }

    // 텍스트 인덱스
    enum Texts
    {
        Dungeon_Name_Text
    }

    // Slider 인덱스
    enum Sliders
    {
        Dungeon_Progress_Bar
    }

    // 버튼 인덱스
    enum Buttons
    {
        Open_Chat_Button,
        Open_Menu_Button
    }

    // 추가된 멤버 변수
    private GameObject bossStatus;
    private TextMeshProUGUI dungeonNameText;
    private Slider dungeonProgressBar;
    public Transform[] checkpoints;
    private int totalCheckpoints = 3;
    private int currentCheckpointIndex = 0;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
        Bind<Button>(typeof(Buttons));

        // 보스 상태창
        bossStatus = GetObject((int)GameObjects.Boss_Status);

        // 선택된 던전
        dungeonNameText = GetText((int)Texts.Dungeon_Name_Text);
        UpdateSelectedDungeon();

        // Slider와 GameObject 연결
        dungeonProgressBar = GetSlider((int)Sliders.Dungeon_Progress_Bar);
        dungeonProgressBar.value = 0;

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

    void OnDestroy()
    {
        KnightGController.OnBossDestroyed -= HandleBossDestroyed;
        bossStatus.gameObject.SetActive(false);
    }

    // 현재 던전 이름 업데이트하기
    private void UpdateSelectedDungeon()
    {
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);

        // 던전 번호에 따라 다른 텍스트를 설정
        switch (selectedDungeonNumber)
        {
            case 1:
                dungeonNameText.text = "깊은 숲";
                break;
            case 2:
                dungeonNameText.text = "잊혀진 신전";
                break;
            case 3:
                dungeonNameText.text = "별의 조각 평원";
                break;
            default:
                dungeonNameText.text = "알 수 없는 던전입니다.";
                break;
        }
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
            bossStatus.gameObject.SetActive(true);
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
