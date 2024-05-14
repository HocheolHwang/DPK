using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Skill_Select : MonoBehaviour
{
    // ------------------------------ 변수 정의 ------------------------------

    // --------------- 스킬 목록 ---------------

    // 스킬 목록 정보
    private Button[] skillContainers = new Button[16]; // 스킬 버튼
    private Image[] skillIcons = new Image[16]; // 스킬 아이콘
    private Image[] skillCanNotBeUsedIcons = new Image[16]; // 스킬 비활성화 아이콘
    private TMP_Text[] skillNames = new TMP_Text[16]; // 스킬 이름
    private TMP_Text[] skillRequiredLevels = new TMP_Text[16]; // 필요 레벨


    // --------------- 스킬 슬롯 ---------------

    // 슬롯에 저장된 스킬 이미지
    private Image[] slotSkillIcons = new Image[8];


    // --------------- 캐릭터 정보 ---------------

    // 선택된 캐릭터 코드
    private string selectedClassCode;

    // 선택된 캐릭터 코드 이전 값
    private string previousSelectedClassCode;

    // 클래스 코드와 스킬 이름 배열을 매핑하는 딕셔너리
    Dictionary<string, string[]> classSkills = new Dictionary<string, string[]>()
{
    {"C001",new string[]
        {
            "DrawSword", "Guard", "Massacre", "Guard",
            "Sting", "SwordWave", "TelekineticSwords", "Whirlwind",
            "WindSlash", "Guard", "Guard", "Guard",
            "Guard", "Guard", "Guard", "Guard"
        }
    },
    {"C002", new string[]
        {
            "ArrowBomb", "ArrowShower", "ArrowStab", "ForestSpirit",
            "LightningShot", "RapidArrow", "WindBall", "WindShield",
            "WindShield", "WindShield", "WindShield", "WindShield",
            "WindShield", "WindShield", "WindShield", "WindShield"
        }
    },
    {"C003", new string[]
         {
            "Tornado", "Thunder", "Rage", "QuickFreeze",
            "Protection", "Meteor", "Infusion", "IceBone",
            "Heal", "Gravity", "FlashLight", "BloodBoom",
            "Adrenalin", "Adrenalin", "Adrenalin", "Adrenalin"
        }
    }
};

    // ------------------------------ UI 초기화 ------------------------------

    private void Start()
    {
        // --------------- 캐릭터 정보 초기화 ---------------

        // 선택된 캐릭터 코드 초기화
        selectedClassCode = Character_Select.selectedClassCode;

        // 선택된 캐릭터 코드의 초기 이전 값 설정
        previousSelectedClassCode = selectedClassCode;


        // --------------- 스킬 목록 ---------------

        // 스킬 목록 정보 연결
        for (int i = 0; i < 16; i++)
        {
            skillContainers[i] = GameObject.Find($"Skill_{i + 1}_Container").GetComponent<Button>();
            skillIcons[i] = GameObject.Find($"Skill_{i + 1}_Icon").GetComponent<Image>();
            skillCanNotBeUsedIcons[i] = GameObject.Find($"Skill_{i + 1}_Can_Not_Be_Used").GetComponent<Image>();
            skillNames[i] = GameObject.Find($"Skill_{i + 1}_Name").GetComponent<TMP_Text>();
            skillRequiredLevels[i] = GameObject.Find($"Skill_{i + 1}_Required_Level").GetComponent<TMP_Text>();
        }


        // --------------- 스킬 슬롯 ---------------

        // 슬롯에 저장된 스킬 이미지 오브젝트 연결
        for (int i = 0; i < 8; i++)
        {
            slotSkillIcons[i] = GameObject.Find($"Slot_Skill_{i + 1}").GetComponent<Image>();
        }


        // --------------- 스킬 정보 업데이트---------------

        // 모든 스킬 정보 업데이트
        UpdateAllSkillInfo(selectedClassCode);

        // 선택한 스킬 정보 업데이트
        UpdateSelectedSkillInfo(selectedClassCode);
    }


    // ------------------------------ 유니티 생명주기 메서드 ------------------------------

    private void Update()
    {
        // 선택된 캐릭터 코드 초기화
        selectedClassCode = Character_Select.selectedClassCode;

        // selectedClassCode 값이 이전 값과 다를 경우
        if (selectedClassCode != previousSelectedClassCode)
        {
            // 모든 스킬 정보 업데이트
            UpdateAllSkillInfo(selectedClassCode);

            // 선택한 스킬 정보 업데이트
            UpdateSelectedSkillInfo(selectedClassCode);

            // 이전 selectedClassCode 값을 현재 값으로 업데이트
            previousSelectedClassCode = selectedClassCode;
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 모든 스킬 정보를 업데이트하는 메서드
    private void UpdateAllSkillInfo(string classCode)
    {
        // classCode가 null이 아닌지 확인
        if (classCode == null)
        {
            Debug.Log("classCode는 null일 수 없습니다.");
            return;
        }

        // 스킬 목록 업데이트
        UpdateSkillList(classCode);

        // 스킬 슬롯 업데이트
        UpdateSkillSlot(classCode);
    }

    // 스킬 목록 업데이트 메서드
    private void UpdateSkillList(string classCode)
    {
        // 플레이어 레벨 가져오기
        int playerLevel = Managers.Player.GetLevel();

        for (int i = 0; i < 16; i++)
        {
            // SkillSO 객체 로드
            SkillSO skillData = Resources.Load<SkillSO>($"Scriptable/Skill/{classSkills[classCode][i]}");
            if (skillData == null) continue;

            skillIcons[i].sprite = skillData.Icon;
            skillNames[i].text = skillData.SkillKoreanName;
            skillRequiredLevels[i].text = $"필요레벨 {skillData.RequiredLevel}";

            // 필요 레벨이 플레이어 레벨보다 클 경우 텍스트 색상을 빨간색으로, 그렇지 않으면 흰색으로 설정
            if (skillData.RequiredLevel > playerLevel)
            {
                skillCanNotBeUsedIcons[i].gameObject.SetActive(true);
                skillNames[i].color = new Color(1, 0, 0, 0.8f);
                skillRequiredLevels[i].color = new Color(1, 0, 0, 0.8f);
            }
            else
            {
                skillCanNotBeUsedIcons[i].gameObject.SetActive(false);
                skillNames[i].color = Color.white;
                skillRequiredLevels[i].color = Color.white;
            }
        }
    }

    // 스킬 슬롯 아이콘 업데이트 메서드
    public void UpdateSkillSlot(string classCode)
    {
        SkillInfo[] skills = classCode switch
        {
            "C001" => Managers.Player.warriorSkills,
            "C002" => Managers.Player.archerSkills,
            "C003" => Managers.Player.mageSkills,
            _ => null
        };

        if (skills == null)
        {
            // 스킬이 없는 경우 모든 슬롯을 기본 이미지로 설정
            FillRemainingSlots(0);
            return;
        }

        // 스킬 개수만큼 반복
        for (int i = 0; i < skills.Length; i++)
        {
            SkillSO skillData = Resources.Load<SkillSO>($"Scriptable/Skill/{skills[i].skillName}");

            // 이미지 컴포넌트에 Sprite 할당
            if (skillData != null)
            {
                slotSkillIcons[i].sprite = skillData.Icon;
            }
        }

        // 남은 슬롯들을 기본 이미지로 채우기
        if (skills.Length < slotSkillIcons.Length)
        {
            FillRemainingSlots(skills.Length);
        }
    }

    // 남은 슬롯들을 기본 이미지로 채우는 메서드
    private void FillRemainingSlots(int startIndex)
    {
        for (int i = startIndex; i < slotSkillIcons.Length; i++)
        {
            if (slotSkillIcons[i] != null)
            {
                slotSkillIcons[i].sprite = Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image");
            }
        }
    }

    // 선택한 스킬 정보 업데이트 메서드
    public void UpdateSelectedSkillInfo(string classCode)
    {
        if (classCode == null || !classSkills.ContainsKey(classCode))
        {
            Debug.LogError("Invalid classCode: " + classCode);
            return;
        }

        // 스킬 목록 정보 연결
        for (int i = 0; i < 16; i++)
        {
            // 클래스 코드에 따른 스킬 이름 가져오기
            string skillName = classSkills[classCode][i];

            // Skill_Info 컴포넌트 참조
            Skill_Info skillInfoComponent = GameObject.Find("Skill_Info").GetComponent<Skill_Info>();

            // 버튼 클릭 이벤트 추가
            skillContainers[i].onClick.AddListener(() => skillInfoComponent.UpdateSkillInfo(skillName));
        }
    }
}
