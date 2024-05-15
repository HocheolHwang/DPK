using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character_Select_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button,
        Save_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button saveButton;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 컴포넌트 바인딩
        Bind<Button>(typeof(Buttons));

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 저장하기 버튼 이벤트 등록
        saveButton = GetButton((int)Buttons.Save_Button);
        AddUIEvent(saveButton.gameObject, SaveCharacter);
        AddUIKeyEvent(saveButton.gameObject, () => SaveCharacter(null), KeyCode.Return);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 저장하기 메서드
    private void SaveCharacter(PointerEventData data)
    {
        // Skill_Select 인스턴스 찾기
        Skill_Select skillSelectInstance = FindObjectOfType<Skill_Select>();

        // 임시 슬롯을 실제 슬롯에 저장
        switch (Character_Select.selectedClassCode)
        {
            case "C001":
                Managers.Player.warriorSkills = skillSelectInstance.DeepCopySkillSlot(skillSelectInstance.tempWarriorSkills);
                break;
            case "C002":
                Managers.Player.archerSkills = skillSelectInstance.DeepCopySkillSlot(skillSelectInstance.tempArcherSkills);
                break;
            case "C003":
                Managers.Player.mageSkills = skillSelectInstance.DeepCopySkillSlot(skillSelectInstance.tempMageSkills);
                break;
            default:
                break;
        }

        // 캐릭터 선택 확인 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Character_Selection_Confirm_Popup_UI>("[Lobby]_Character_Selection_Confirm_Popup_UI");
    }
}
