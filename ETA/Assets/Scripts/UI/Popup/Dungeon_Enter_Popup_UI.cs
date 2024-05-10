using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Dungeon_Enter_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button,
        Dungeon_Enter_Button
    }

    enum Texts
    {
        Dungeon_Enter_Text
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button dungeonEnterButton;
    private TextMeshProUGUI dungeonEnterText;

    // 선택된 던전 번호
    private int selectedDungeonNumber;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // 선택된 던전 번호 초기화
        selectedDungeonNumber = FindObjectOfType<Lobby_Scene>().currentDungeonNumber;

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 입장하기 버튼 이벤트 등록
        dungeonEnterButton = GetButton((int)Buttons.Dungeon_Enter_Button);
        AddUIEvent(dungeonEnterButton.gameObject, DungeonEnter);
        AddUIKeyEvent(dungeonEnterButton.gameObject, () => DungeonEnter(null), KeyCode.Return);

        // 선택된 던전 텍스트 업데이트
        dungeonEnterText = GetText((int)Texts.Dungeon_Enter_Text);
        UpdateSelectedDungeon();
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 입장하기 메서드
    public void DungeonEnter(PointerEventData data)
    {
        if(PhotonNetwork.InRoom == false)
        {
            FindObjectOfType<Lobby_Scene>().isSoloPlay = true;
            Managers.Photon.MakeRoom(Managers.Player.GetNickName() + "의 파티");
            return;
        }

        foreach(var player in PhotonNetwork.PlayerList)
        {
            if(player.CustomProperties.TryGetValue("currentScene", out object obj))
            {
                
                Define.Scene currentScene = (Define.Scene)obj;
                if (currentScene != Define.Scene.Lobby)
                {
                    Debug.Log("로비에 없는 플레이어가 있습니다.");
                    return;
                }
            }
        }
        // Scene 이동 전에 모든 스택을 비움
        CloseAllPopupUI();

        // 선택된 던전 번호에 따라 다른 씬으로 이동
        Define.Scene sceneName = selectedDungeonNumber switch
        {
            1 => Define.Scene.StarShardPlain,
            2 => Define.Scene.ForgottenTemple,
            3 => Define.Scene.SeaOfAbyss,
            _ => Define.Scene.Unknown // 던전 번호가 인식할 수 없는 값일 경우 null을 반환
        };

        if (sceneName == Define.Scene.Unknown)
        {
            // 메서드 종료
            Debug.LogWarning("알 수 없는 던전 번호입니다. Scene 이동이 취소되었습니다.");
            return; 
        }

        // 선택된 던전 Scene으로 이동
        // TMP
        FindObjectOfType<GameSystem>().ChangeSceneAllPlayer(sceneName);
        //Debug.Log($"이동할 Scene은? {sceneName}");
        Managers.Scene.LoadScene(sceneName);

        // Managers.Scene.LoadScene(Define.Scene.MultiPlayTest);

        // 다른 애들도 가라고 RPC
        
        // FindObjectOfType<GameSystem>().ChangeSceneAllPlayer(Define.Scene.MultiPlayTest);
    }

    // 선택된 던전 텍스트 업데이트 메서드
    private void UpdateSelectedDungeon()
    {
        // 선택된 던전 번호에 따라 다른 텍스트를 설정
        dungeonEnterText.text = selectedDungeonNumber switch
        {
            1 => "[별의 조각 평원]에 입장하시겠습니까?",
            2 => "[잊혀진 신전]에 입장하시겠습니까?",
            3 => "[심연의 바다]에 입장하시겠습니까?",
            _ => "알 수 없는 던전입니다.",
        };
    }
}
