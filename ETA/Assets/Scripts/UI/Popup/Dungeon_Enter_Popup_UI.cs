using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Dungeon_Enter_Popup_UI : UI_Popup
{
    // 버튼 인덱스
    enum Buttons
    {
        Cancel_Button,
        Dungeon_Enter_Button
    }

    // 로그인 UI 초기화
    public override void Init()
    {
        base.Init(); // 기본 초기화

        // 버튼 바인딩
        Bind<Button>(typeof(Buttons));

        // 돌아가기 버튼 이벤트 등록
        Button cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 입장하기 버튼 이벤트 등록
        Button dungeonEnterButton = GetButton((int)Buttons.Dungeon_Enter_Button);
        AddUIEvent(dungeonEnterButton.gameObject, DungeonEnter);
        AddUIKeyEvent(dungeonEnterButton.gameObject, () => DungeonEnter(null), KeyCode.Return);
    }

    // 로비 돌아가기
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 던전 입장하기
    private void DungeonEnter(PointerEventData data)
    {
        // 씬 이동하기 전에 모든 스택을 비움
        CloseAllPopupUI();

        // TODO: 선택한 던전에 따라 다른 곳으로 이동하도록 하는 코드 필요
        // 현재는 DeepForest로 가도록 임시 구성
        SceneManager.LoadScene("DeepForest");
    }
}
