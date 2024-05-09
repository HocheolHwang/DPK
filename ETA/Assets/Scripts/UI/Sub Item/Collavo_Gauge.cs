using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collavo_Gauge : MonoBehaviour
{
    // ------------------------------ 변수 정의 ------------------------------

    // 파티원 아이콘
    private Image memberIcon;

    // 스킬 아이콘
    private Image skillIcon;

    // 파티원 닉네임
    private TMP_Text memberNicknameText;

    // 콜라보 게이지
    private Slider collavoSlider;


    // ------------------------------ UI 초기화 ------------------------------
    private void Start()
    {
        // 이미지 컴포넌트 찾기
        memberIcon = GameObject.Find("Member_Icon").GetComponent<Image>();
        skillIcon = GameObject.Find("Skill_Icon").GetComponent<Image>();

        // 텍스트 컴포넌트 찾기
        memberNicknameText = GameObject.Find("Member_Nickname_Text").GetComponent<TMP_Text>();

        // 슬라이더 컴포넌트 찾기
        collavoSlider = GameObject.Find("Collavo_Slider").GetComponent<Slider>();

        // 콜라보 정보 업데이트
        UpdateCollavoInfo();
    }


    // ------------------------------ 유니티 생명주기 메서드 ------------------------------

    void Update()
    {
        // 콜라보 게이지 업데이트
        UpdateCollavoGauge();
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 콜라보 정보 업데이트 메서드
    private void UpdateCollavoInfo()
    {
        // @@@@@@@@@@@@@@@@@@@ TODO: 콜라보 사용한 파티원 직업 가져오는 코드 필요 @@@@@@@@@@@@@@@@@@@

        // 콜라보 사용한 파티원의 직업 코드에 따라 다른 텍스트 적용
        string className = Managers.Player.GetClassCode() switch // (임시) 파티원 직업 가져오는 코드 필요
        {
            "C001" => "Warrior",
            "C002" => "Archer",
            "C003" => "Mage",
            _ => ""
        };

        // 직업에 해당하는 이미지 저장
        memberIcon.sprite = Resources.Load<Sprite>($"Sprites/Class Icon/{className}");




        // @@@@@@@@@@@@@@@@@@@ TODO: 콜라보 사용한 스킬 가져오는 코드 필요 @@@@@@@@@@@@@@@@@@@

        // 콜라보 사용한 파티원의 직업 코드에 따라 다른 텍스트 적용
        string skillName = "WindSlash"; // (임시)

        // 사용한 스킬에 해당하는 이미지 저장
        skillIcon.sprite = Resources.Load<Sprite>($"Sprites/SkillIcon/{className}/{skillName}");




        // @@@@@@@@@@@@@@@@@@@ TODO: 콜라보 사용한 파티원 닉네임 가져오는 코드 필요 @@@@@@@@@@@@@@@@@@@

        // 콜라보 사용한 파티원의 닉네임 적용
        memberNicknameText.text = Managers.Player.GetNickName(); // (임시) 파티원 닉네임 가져오는 코드 필요





    }

    // 콜라보 게이지 업데이트 메서드
    private void UpdateCollavoGauge()
    {
        // @@@@@@@@@@@@@@@@@@@ TODO: 콜라보 스킬 사용한 시간 가져와서 Slider에 적용하는 코드 필요 @@@@@@@@@@@@@@@@@@@

        // 콜라보 게이지 업데이트
        collavoSlider.value = 0; // (임시)
    }
}
