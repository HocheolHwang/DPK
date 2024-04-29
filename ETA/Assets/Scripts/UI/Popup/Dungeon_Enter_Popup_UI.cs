using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Dungeon_Enter_Popup_UI : UI_Popup
{
    // 텍스트 인덱스
    enum Texts
    {
        Dungeon_Enter_Text
    }

    // 버튼 인덱스
    enum Buttons
    {
        Cancel_Button,
        Dungeon_Enter_Button
    }

    // 클래스 멤버 변수로 선언
    private TextMeshProUGUI dungeonEnterText;

    

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // 선택된 던전
        dungeonEnterText = GetText((int)Texts.Dungeon_Enter_Text);
        UpdateSelectedDungeon();

        // 돌아가기 버튼 이벤트 등록
        Button cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 입장하기 버튼 이벤트 등록
        Button dungeonEnterButton = GetButton((int)Buttons.Dungeon_Enter_Button);
        AddUIEvent(dungeonEnterButton.gameObject, DungeonEnter);
        AddUIKeyEvent(dungeonEnterButton.gameObject, () => DungeonEnter(null), KeyCode.Return);
    }

    // 선택된 던전 업데이트하기
    private void UpdateSelectedDungeon()
    {
        int secidedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 0);
        if (secidedDungeonNumber != 0)
        {
            // 선택된 던전 번호에 따라 다른 텍스트를 설정
            switch (secidedDungeonNumber)
            {
                case 1:
                    dungeonEnterText.text = "[깊은 숲]에 입장하시겠습니까?";
                    break;
                case 2:
                    dungeonEnterText.text = "[잊혀진 신전]에 입장하시겠습니까?";
                    break;
                case 3:
                    dungeonEnterText.text = "[별의 조각 평원]에 입장하시겠습니까?";
                    break;
                default:
                    dungeonEnterText.text = "알 수 없는 던전입니다.";
                    break;
            }
        }
        else
        {
            Debug.LogError("Dungeon_Select_Popup_UI 인스턴스를 찾을 수 없습니다.");
        }
    }


    // 로비 돌아가기
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 던전 입장하기
    private void DungeonEnter(PointerEventData data)
    {
        // 모든 팝업 UI를 닫습니다.
        CloseAllPopupUI();

        int secidedDungeonNumber = PlayerPrefs.GetInt("SelectedDungeonNumber", 0);
        if (secidedDungeonNumber != 0)
        {
            // 선택된 던전 번호에 따라 다른 씬으로 이동합니다.
            switch (secidedDungeonNumber)
            {
                case 1:
                    SceneManager.LoadScene("DeepForest");
                    break;
                case 2:
                    SceneManager.LoadScene("ForgottenTemple");
                    break;
                case 3:
                    SceneManager.LoadScene("StarShardPlain");
                    break;
                default:
                    Debug.LogError("알 수 없는 던전 번호입니다.");
                    break;
            }
        }
        else
        {
            Debug.LogError("Dungeon_Select_Popup_UI 인스턴스를 찾을 수 없습니다.");
        }
    }

}
