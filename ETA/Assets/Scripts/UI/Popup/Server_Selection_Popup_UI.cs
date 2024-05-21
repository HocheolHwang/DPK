using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class Server_Selection_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        // 서버 버튼
        Server_1_Button,
        Server_2_Button,
        Server_3_Button,

        // 서버 선택 버튼
        Server_Selection_Button,

        // 게임 종료 버튼
        Game_Exit_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button[] serverButtons = new Button[3];
    private Button serverSelectionButton;
    private Button gameExitButton;

    // 선택된 서버를 확인하는 변수
    private int CurrentServerNumber
    {
        get { return _currentServerNumber; }
        set
        {
            _currentServerNumber = value;
            SelectServer(_currentServerNumber);
        }
    }

    // 실제 변수 값
    private int _currentServerNumber;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 서버 선택 버튼을 반복문으로 처리
        for (int i = 0; i < 3; i++)
        {
            int serverNumber = i;
            serverButtons[i] = GetButton((int)Buttons.Server_1_Button + i);
            AddUIEvent(serverButtons[i].gameObject, (PointerEventData data) => SelectServer(serverNumber));
        }

        // 서버 선택 버튼 이벤트 등록
        serverSelectionButton = GetButton((int)Buttons.Server_Selection_Button);
        AddUIEvent(serverSelectionButton.gameObject, ServerSelection);
        AddUIKeyEvent(serverSelectionButton.gameObject, () => ServerSelection(null), KeyCode.Return);

        // 게임 종료 버튼 이벤트 등록
        gameExitButton = GetButton((int)Buttons.Game_Exit_Button);
        AddUIEvent(gameExitButton.gameObject, OpenGameExit);
        AddUIKeyEvent(gameExitButton.gameObject, () => OpenGameExit(null), KeyCode.Escape);

        // 서버 선택 상태 초기화
        serverButtons[0].Select();
        serverButtons[0].onClick.Invoke();
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 선택한 서버 번호 업데이트 메서드
    private void SelectServer(int serverNumber)
    {
        _currentServerNumber = serverNumber;
    }

    // 서버 선택 메서드
    private void ServerSelection(PointerEventData data)
    {
        // TODO: currentServerNumber 숫자에 따라 서로 다른 서버로 접속하는 코드 작성 필요
        string appId = _currentServerNumber switch
        {
            0 => "f6389f3a-e687-4cba-84d5-bf19ca54c9bc",
            1 => "91851ade-708c-4a66-8f90-5b526eba80a2",
            2 => "169642ab-8e8d-42f1-bb00-ffeffe4d038c",
            _ => ""
        };

        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = appId;

        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 로그인 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Before_Login_Popup_UI>("[Login]_Before_Login_Popup_UI");
        Managers.UI.ShowPopupUI<Login_Popup_UI>("[Login]_Login_Popup_UI");
    }

    // 게임 종료 Popup UI 띄우기 메서드
    private void OpenGameExit(PointerEventData data)
    {
        // 게임 종료 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Game_Exit_Popup_UI>("[Common]_Game_Exit_Popup_UI");
    }
}
