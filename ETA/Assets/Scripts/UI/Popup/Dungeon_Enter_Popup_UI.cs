using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

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
    private void DungeonEnter(PointerEventData data)
    {
        // Scene 이동 전에 모든 스택을 비움
        CloseAllPopupUI();

        // 선택된 던전 번호 가져오기
        int selectedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);

        // 선택된 던전 번호에 따라 다른 씬으로 이동
        Define.Scene sceneName = selectedDungeonNumber switch
        {
            1 => Define.Scene.DeepForest,
            2 => Define.Scene.ForgottenTemple,
            3 => Define.Scene.StarShardPlain,
            _ => Define.Scene.Unknown // 던전 번호가 인식할 수 없는 값일 경우 null을 반환
        };

        if (sceneName == Define.Scene.Unknown)
        {
            // 메서드 종료
            Debug.LogWarning("알 수 없는 던전 번호입니다. Scene 이동이 취소되었습니다.");
            return; 
        }

        // 선택된 던전 Scene으로 이동
        //SceneManager.LoadScene(sceneName);
        // TMP
        //Managers.Scene.LoadScene(sceneName);

        Managers.Scene.LoadScene(Define.Scene.MultiPlayTest);

        // 다른 애들도 가라고 RPC
        FindObjectOfType<MyPhoton>().ChangeSceneAllPlayer(Define.Scene.MultiPlayTest);

    }

    // 선택된 던전 텍스트 업데이트 메서드
    private void UpdateSelectedDungeon()
    {
        // 선택된 던전 번호 가져오기
        int secidedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 1);

        // 선택된 던전 번호에 따라 다른 텍스트를 설정
        dungeonEnterText.text = secidedDungeonNumber switch
        {
            1 => "[깊은 숲]에 입장하시겠습니까?",
            2 => "[잊혀진 신전]에 입장하시겠습니까?",
            3 => "[별의 조각 평원]에 입장하시겠습니까?",
            _ => "알 수 없는 던전입니다.",
        };
    }
}
