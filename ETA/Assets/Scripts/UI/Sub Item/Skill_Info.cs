using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Define;


public class Skill_Info : MonoBehaviour
{
    // ------------------------------ 변수 정의 ------------------------------

    // UI 컴포넌트 바인딩 변수
    private GameObject skillInfoContent; // 스킬 정보 패널

    private TMP_Text skillKoreanName; // 스킬 한글 이름
    private TMP_Text requiredLevel; // 필요 레벨

    private Image icon; // 스킬 아이콘
    private TMP_Text skillCategory; // 스킬 종류
    private TMP_Text skillType; // 스킬 타입
    private TMP_Text coolDownTime; // 스킬 쿨타임

    private TMP_Text skillDescription; // 스킬 설명

    private GameObject skillCategoryContainer; // 스킬 종류 컨테이너

    private Image skillCategoryIcon; // 스킬 종류 아이콘
    private TMP_Text skillCategoryTitleText; // 스킬 종류
    private TMP_Text questionMarkText; // 스킬 종류 설명

    private TMP_Text collavoSkillKoreanName; // 콜라보 스킬 한글 이름
    private TMP_Text skillCategoryDescription; // 콜라보 스킬 or 카운터 스킬 설명
    private TMP_Text connectedSkillKoreanName; // 콜라보를 함께 발동할 스킬의 한글 이름

    private GameObject connectedSkillNameContainer; // 콜라보 연동 스킬 컨테이너

    // 스킬 종류
    private bool isCollavo; // 콜라보 스킬 여부
    private bool isCounter; // 카운터 스킬 여부

    // 스킬 상세 정보
    private int damage; // 스킬 피해량

    // 스킬 기본 정보
    private string skillCode; // 스킬 코드
    private string skillName; // 스킬 영어 이름


    // ------------------------------ UI 초기화 ------------------------------

    private void Start()
    {
        // 스킬 정보 초기화
        skillInfoContent = GameObject.Find("Skill_Info_Content");

        skillKoreanName = GameObject.Find("Skill_Korean_Name").GetComponent<TMP_Text>();
        requiredLevel = GameObject.Find("Required_Level").GetComponent<TMP_Text>();

        icon = GameObject.Find("Icon").GetComponent<Image>();
        skillCategory = GameObject.Find("Skill_Category").GetComponent<TMP_Text>();
        skillType = GameObject.Find("Skill_Type").GetComponent<TMP_Text>();
        coolDownTime = GameObject.Find("Coll_Down_Time").GetComponent<TMP_Text>();

        skillDescription = GameObject.Find("Skill_Description").GetComponent<TMP_Text>();

        skillCategoryContainer = GameObject.Find("Skill_Category_Container");

        skillCategoryIcon = GameObject.Find("Skill_Category_Icon").GetComponent<Image>();
        skillCategoryTitleText = GameObject.Find("Skill_Category_Title_Text").GetComponent<TMP_Text>();
        questionMarkText = GameObject.Find("Question_Mark_Text").GetComponent<TMP_Text>();

        collavoSkillKoreanName = GameObject.Find("Collavo_Skill_Korean_Name").GetComponent<TMP_Text>();
        skillCategoryDescription = GameObject.Find("Skill_Category_Description").GetComponent<TMP_Text>();
        connectedSkillKoreanName = GameObject.Find("Connected_Skill_Name").GetComponent<TMP_Text>();

        connectedSkillNameContainer = GameObject.Find("Connected_Skill_Name_Container");

        // 스킬 패널 비활성화
        skillInfoContent.SetActive(false);
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 모든 스킬 정보를 업데이트하는 메서드
    public void UpdateSkillInfo(string className, string skillName)
    {
        // SkillSO 객체 로드
        SkillSO skillData = Resources.Load<SkillSO>($"Scriptable/Skill/{className}/{skillName}");

        if (skillData != null)
        {
            // 스킬 패널 활성화
            skillInfoContent.SetActive(true);

            // 스킬 이름 업데이트
            skillKoreanName.text = skillData.SkillKoreanName;

            // 필요 레벨 업데이트
            requiredLevel.text = $"필요레벨 {skillData.RequiredLevel}";

            // 스킬 아이콘 업데이트
            icon.sprite = skillData.Icon;

            // 스킬 종류 업데이트
            isCollavo = skillData.IsCollavo;
            isCounter = skillData.IsCounter;
            skillCategory.text = isCollavo ? "[ 콜라보 스킬 ]" : (isCounter ? "[ 카운터 스킬 ]" : "[ 일반 스킬 ]");

            // 스킬 타입 업데이트
            switch (skillData.SkillType)
            {
                case SkillType.Target:
                    skillType.text = "타겟 지정";
                    break;
                case SkillType.Range:
                    skillType.text = "범위 지정";
                    break;
                case SkillType.Holding:
                    skillType.text = "홀딩";
                    break;
                case SkillType.Immediately:
                    skillType.text = "즉시 발동";
                    break;
                default:
                    skillType.text = "알 수 없음";
                    break;
            }

            // 스킬 쿨타임 업데이트
            coolDownTime.text = $"재사용 대기시간 {skillData.CoolDownTime}초";

            // 스킬 설명 업데이트
            skillDescription.text = skillData.SkillDescription;

            // 콜라보 스킬 정보 업데이트
            if (isCollavo)
            {
                skillCategoryContainer.SetActive(true);
                UpdateCollavoInfo(skillData);
            }
            else if (isCounter)
            {
                skillCategoryContainer.SetActive(true);
                UpdateCounterInfo();
            }
            else
            {
                skillCategoryContainer.SetActive(false);
            }
        }
        else
        {
            // 스킬 패널 비활성화
            skillInfoContent.SetActive(false);

            Debug.LogError("해당 스킬 이름의 SkillSO를 찾을 수 없습니다: " + skillName);
        }
    }

    // 콜라보 스킬 정보를 업데이트하는 메서드
    private void UpdateCollavoInfo(SkillSO skillData)
    {
        // 콜라보 스킬 아이콘 업데이트
        skillCategoryIcon.sprite = Resources.Load<Sprite>($"Sprites/Skill Slot/Collavo Icon");

        // 콜라보 스킬 텍스트 업데이트
        skillCategoryTitleText.text = "콜라보";

        // 콜라보 스킬 설명 업데이트
        questionMarkText.text = "아군과의 연계를 통해 발동되는 합동 공격 스킬입니다.";

        // 콜라보 스킬 이름 활성화 및 업데이트
        collavoSkillKoreanName.gameObject.SetActive(true);
        collavoSkillKoreanName.text = skillData.CollavoSkillKoreanName;

        // 콜라보 스킬 설명 업데이트
        skillCategoryDescription.text = skillData.CollavoSkillDescription;

        // 콜라보 연동 스킬 이름 활성화 및 업데이트
        connectedSkillNameContainer.gameObject.SetActive(true);
        connectedSkillKoreanName.text = skillData.ConnectedSkillKoreanName;
    }

    // 카운터 스킬 정보를 업데이트하는 메서드
    private void UpdateCounterInfo()
    {
        // 카운터 스킬 아이콘 업데이트
        skillCategoryIcon.sprite = Resources.Load<Sprite>($"Sprites/Skill Slot/Counter Icon");

        // 카운터 스킬 텍스트 업데이트
        skillCategoryTitleText.text = "카운터";

        // 카운터 스킬 설명 업데이트
        questionMarkText.text = "적절한 타이밍에 적중하면 발동하는 효과입니다.";

        // 콜라보 스킬 이름 텍스트 비활성화
        collavoSkillKoreanName.gameObject.SetActive(false);

        // 카운터 스킬 설명 업데이트
        skillCategoryDescription.text = "보스 몬스터를 일정 시간 동안 그로기 상태에 빠뜨립니다.";

        // 콜라보 연동 스킬 컨테이너 비활성화
        connectedSkillNameContainer.SetActive(false);
    }
}
