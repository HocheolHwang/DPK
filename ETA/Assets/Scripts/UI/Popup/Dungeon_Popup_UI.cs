using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Dungeon_Popup_UI : UI_Popup
{
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
    private Slider dungeonProgressBar;
    public Transform[] checkpoints;
    private int totalCheckpoints = 3;
    private int currentCheckpointIndex = 0;

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<Slider>(typeof(Sliders));
        Bind<Button>(typeof(Buttons));

        // Slider와 GameObject 연결
        dungeonProgressBar = GetSlider((int)Sliders.Dungeon_Progress_Bar);

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
