using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Character_Select : MonoBehaviour
{
    // ------------------------------ 변수 정의 ------------------------------

    // 캐릭터 선택
    private Button warriorSelectButton;
    private Button archerSelectButton;
    private Button mageSelectButton;
    private Image[] characterUnselecteds = new Image[3];

    // 캐릭터 정보
    private Image[] characterBackgrounds = new Image[3];
    private Image[] hpItems = new Image[5];
    private Image[] apItems = new Image[5];
    private Image[] dpItems = new Image[5];
    private TMP_Text characterNameText;
    private TMP_Text characterDetailText;
    private TMP_Text characterClassText;
    private TMP_Text characterDifficultyText;
    private TMP_Text characterLevelText;
    private TMP_Text hpText;
    private TMP_Text apText;
    private TMP_Text dpText;

    // 사진 자세히 보기
    private Button charactorPhotoDetailButton;
    private Image photoDetailContainer;
    private Image[] characterImages = new Image[3];

    // 선택된 캐릭터 코드
    public static string selectedClassCode;


    // ------------------------------ UI 초기화 ------------------------------

    private void Start()
    {
        // --------------- 캐릭터 선택 ---------------

        // 캐릭터 선택 버튼 오브젝트 연결
        warriorSelectButton = GameObject.Find("Warrior_Select_Button").GetComponent<Button>();
        archerSelectButton = GameObject.Find("Archer_Select_Button").GetComponent<Button>();
        mageSelectButton = GameObject.Find("Mage_Select_Button").GetComponent<Button>();

        // 각 버튼에 대한 클릭 리스너 추가
        warriorSelectButton.onClick.AddListener(WarriorSelect);
        archerSelectButton.onClick.AddListener(ArcherSelect);
        mageSelectButton.onClick.AddListener(MageSelect);

        // 캐릭터 선택 버튼 배경 이미지 오브젝트 연결
        characterUnselecteds[0] = GameObject.Find("Warrior_Unselected").GetComponent<Image>();
        characterUnselecteds[1] = GameObject.Find("Archer_Unselected").GetComponent<Image>();
        characterUnselecteds[2] = GameObject.Find("Mage_Unselected").GetComponent<Image>();


        // --------------- 캐릭터 정보 ---------------

        // 캐릭터 배경 이미지 오브젝트 연결
        characterBackgrounds[0] = GameObject.Find("Warrior_Background").GetComponent<Image>();
        characterBackgrounds[1] = GameObject.Find("Archer_Background").GetComponent<Image>();
        characterBackgrounds[2] = GameObject.Find("Mage_Background").GetComponent<Image>();

        // 캐릭터 스텟 이미지 오브젝트 연결
        for (int i = 0; i < 5; i++)
        {
            hpItems[i] = GameObject.Find($"HP_Item_{i + 1}").GetComponent<Image>();
            apItems[i] = GameObject.Find($"AP_Item_{i + 1}").GetComponent<Image>();
            dpItems[i] = GameObject.Find($"DP_Item_{i + 1}").GetComponent<Image>();
        }

        // 캐릭터 정보 텍스트 오브젝트 연결
        characterNameText = GameObject.Find("Character_Name_Text").GetComponent<TMP_Text>();
        characterDetailText = GameObject.Find("Character_Detail_Text").GetComponent<TMP_Text>();
        characterClassText = GameObject.Find("Character_Class_Text").GetComponent<TMP_Text>();
        characterDifficultyText = GameObject.Find("Character_Difficulty_Text").GetComponent<TMP_Text>();
        characterLevelText = GameObject.Find("Character_Level_Text").GetComponent<TMP_Text>();
        hpText = GameObject.Find("HP_Text").GetComponent<TMP_Text>();
        apText = GameObject.Find("AP_Text").GetComponent<TMP_Text>();
        dpText = GameObject.Find("DP_Text").GetComponent<TMP_Text>();


        // --------------- 사진 자세히 보기 ---------------

        // 사진 자세히 보기 버튼 및 이미지 오브젝트 연결
        charactorPhotoDetailButton = GameObject.Find("Charactor_Photo_Detail_Button").GetComponent<Button>();
        photoDetailContainer = GameObject.Find("Photo_Detail_Container").GetComponent<Image>();
        characterImages[0] = GameObject.Find("Warrior_Image").GetComponent<Image>();
        characterImages[1] = GameObject.Find("Archer_Image").GetComponent<Image>();
        characterImages[2] = GameObject.Find("Mage_Image").GetComponent<Image>();

        // 사진 자세히 보기 버튼에 이벤트 리스너 추가 및 비활성화
        EventTrigger trigger = charactorPhotoDetailButton.gameObject.AddComponent<EventTrigger>();
        photoDetailContainer.gameObject.SetActive(false);

        // PointerEnter 이벤트에 대한 이벤트 엔트리 생성
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(pointerEnterEntry);

        // PointerExit 이벤트에 대한 이벤트 엔트리 생성
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(pointerExitEntry);


        // --------------- 캐릭터 정보 초기화 ---------------

        // 선택된 캐릭터 코드 초기화
        selectedClassCode = Managers.Player.GetClassCode();

        // 현재 캐릭터 정보로 업데이트
        switch (Managers.Player.GetClassCode())
        {
            case "C001":
                WarriorSelect();
                break;
            case "C002":
                ArcherSelect();
                break;
            case "C003":
                MageSelect();
                break;
            default:
                break;
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // Warrior 선택 메서드
    private void WarriorSelect()
    {
        // 캐릭터 정보 업데이트
        UpdateCharacterInfo("레오", "Warrior", "보통", 1, 250, 50, 10);

        // 선택된 캐릭터 코드 업데이트
        selectedClassCode = "C001";
    }

    // Archer 선택 메서드
    private void ArcherSelect()
    {
        // 캐릭터 정보 업데이트
        UpdateCharacterInfo("아리아", "Archer", "쉬움", 1, 250, 50, 10);

        // 선택된 캐릭터 코드 업데이트
        selectedClassCode = "C002";
    }

    // Mage 선택 메서드
    private void MageSelect()
    {
        // 캐릭터 정보 업데이트
        UpdateCharacterInfo("이안", "Mage", "어려움", 1, 250, 50, 10);

        // 선택된 캐릭터 코드 업데이트
        selectedClassCode = "C003";
    }

    // 마우스가 자세히 보기 버튼 위로 올라갔을 때 호출될 메소드
    public void OnPointerEnter(PointerEventData eventData)
    {
        photoDetailContainer.gameObject.SetActive(true);
    }

    // 마우스가 자세히 보기 버튼에서 벗어났을 때 호출될 메소드
    public void OnPointerExit(PointerEventData eventData)
    {
        photoDetailContainer.gameObject.SetActive(false);
    }

    // 캐릭터 정보 업데이트 메서드
    private void UpdateCharacterInfo(string name, string className, string difficulty, int level, int hp, int ap, int dp)
    {
        // 텍스트 업데이트
        characterNameText.text = name;
        characterDetailText.text = UpdateDetail(className);
        characterClassText.text = className;
        characterDifficultyText.text = difficulty;
        characterLevelText.text = $"Lv. {level}";
        hpText.text = $"{hp}";
        apText.text = $"{ap}";
        dpText.text = $"{dp}";

        // 사진 자세히 보기 이미지 업데이트
        characterImages[0].gameObject.SetActive(className == "Warrior");
        characterImages[1].gameObject.SetActive(className == "Archer");
        characterImages[2].gameObject.SetActive(className == "Mage");

        // 캐릭터 선택 버튼 배경 이미지 업데이트
        characterUnselecteds[0].gameObject.SetActive(className != "Warrior");
        characterUnselecteds[1].gameObject.SetActive(className != "Archer");
        characterUnselecteds[2].gameObject.SetActive(className != "Mage");

        // 캐릭터 배경 이미지 업데이트
        characterBackgrounds[0].gameObject.SetActive(className == "Warrior");
        characterBackgrounds[1].gameObject.SetActive(className == "Archer");
        characterBackgrounds[2].gameObject.SetActive(className == "Mage");

        // HP 아이템 활성화
        int activeHpItems = hp / 50;
        for (int i = 0; i < hpItems.Length; i++)
        {
            hpItems[i].gameObject.SetActive(i < activeHpItems);
        }

        // AP 아이템 활성화
        int activeApItems = ap / 10;
        for (int i = 0; i < apItems.Length; i++)
        {
            apItems[i].gameObject.SetActive(i < activeApItems);
        }

        // DP 아이템 활성화
        int activeDpItems = dp / 5;
        for (int i = 0; i < dpItems.Length; i++)
        {
            dpItems[i].gameObject.SetActive(i < activeDpItems);
        }
    }

    // 캐릭터 설명 업데이트 메서드
    private string UpdateDetail(string className)
    {
        switch (className)
        {
            case "Warrior":
                return @"레오는 강력하고 용감한 전사로,
모든 적에게 두려움 없이 맞섭니다.
그는 동료들을 위험으로부터 보호하는 방패입니다.";
            case "Archer":
                return @"아리아는 빠르고 정확한 궁수로,
그녀의 활은 항상 목표를 찾습니다.
그녀는 적을 신속하게 제거하여 전투의 흐름을 바꿉니다.";
            case "Mage":
                return @"이안은 신비하고 지혜로운 마법사로,
마법의 수수께끼를 탐구합니다.
그는 다양한 마법으로 동료들의 전투를 지원합니다.";
            default:
                return "";
        }
    }
}
