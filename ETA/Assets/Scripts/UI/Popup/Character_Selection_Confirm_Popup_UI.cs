using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character_Selection_Confirm_Popup_UI : UI_Popup
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum Buttons
    {
        Cancel_Button,
        Save_Changes_Button
    }

    // UI 컴포넌트 바인딩 변수
    private Button cancelButton;
    private Button saveChangesButton;

    // 선택된 캐릭터 코드
    private string selectedClassCode;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 기본 초기화
        base.Init();

        // 바인딩
        Bind<Button>(typeof(Buttons));

        // 선택된 캐릭터 코드 초기화
        selectedClassCode = Character_Select.selectedClassCode;

        // 취소하기 버튼 이벤트 등록
        cancelButton = GetButton((int)Buttons.Cancel_Button);
        AddUIEvent(cancelButton.gameObject, Cancel);
        AddUIKeyEvent(cancelButton.gameObject, () => Cancel(null), KeyCode.Escape);

        // 저장하기 버튼 이벤트 등록
        saveChangesButton = GetButton((int)Buttons.Save_Changes_Button);
        AddUIEvent(saveChangesButton.gameObject, SaveChanges);
        AddUIKeyEvent(saveChangesButton.gameObject, () => SaveChanges(null), KeyCode.Return);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 취소하기 메서드
    private void Cancel(PointerEventData data)
    {
        ClosePopupUI();
    }

    // 저장하기 메서드
    private void SaveChanges(PointerEventData data)
    {
        // 선택된 캐릭터 코드로 ClassReqDto 객체 생성 및 네트워크 호출
        ClassReqDto dto = new ClassReqDto { classCode = selectedClassCode };
        Managers.Network.SelectClassCall(dto, SelectClass);
        
    }

    // UI 업데이트 메서드
    private void UpdateUI()
    {
        // 방에 속해 있지 않다면 로비 씬의 마네킹 변경
        if (!PhotonNetwork.InRoom) FindObjectOfType<Lobby_Scene>().ChangeMannequin();

        // 모든 Popup UI를 닫음
        CloseAllPopupUI();

        // 로비 Popup UI를 띄움
        Managers.UI.ShowPopupUI<Lobby_Popup_UI>("[Lobby]_Lobby_Popup_UI");
    }

    // 클래스 선택 콜백 메서드
    private void SelectClass()
    {
        // 현재 클래스 정보 요청
        Managers.Network.CurrentClassCall(SetClass);
    }

    // 클래스 설정 콜백 메서드
    private void SetClass(CurClassDto dto)
    {
        // 플레이어 데이터 설정
        Managers.Player.SetClassCode(dto.classCode);
        Managers.Player.SetLevel(dto.playerLevel);
        Managers.Player.SetExp(dto.currentExp);

        // 플레이어 클래스 설정
        Managers.Photon.SetPlayerClass();

        SetCurrentSkills();

        // UI 업데이트
        UpdateUI();
    }

    private void SetCurrentSkills()
    {
        SkillReqDto requestBody = new SkillReqDto { classCode = Managers.Player.GetClassCode() };
        List<SkillRequestDto> list = new List<SkillRequestDto>();

        SkillInfo[] skillInfos = null;
        switch (Managers.Player.GetClassCode())
        {
            case "C001":
                skillInfos = Managers.Player.warriorSkills;
                break;
            case "C002":
                skillInfos = Managers.Player.warriorSkills;
                break;
            case "C003":
                skillInfos = Managers.Player.warriorSkills;
                break;
            default:
                Debug.LogError("없는 직업입니다.");
                break;
        }
        for(int i = 0; i < 8; i++)
        {
            if (skillInfos[i] == null) continue;
            SkillRequestDto e = new SkillRequestDto { index = i, skillCode = skillInfos[i].skillCode };
            list.Add(e);
        }


        requestBody.skillList = list;

        Managers.Network.LearnSkillCall(requestBody, null);
    }
}
