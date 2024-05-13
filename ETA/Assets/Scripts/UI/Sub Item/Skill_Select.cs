using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Skill_Select : MonoBehaviour
{
    // ------------------------------ 변수 정의 ------------------------------

    // --------------- 스킬 목록 ---------------

    // 계열별 스킬 이미지를 저장하는 딕셔너리
    //private Dictionary<Image, string, int> skillList = new Dictionary<Image, string, int>
    //{
    //    { Image, string, int a},
    //    { Image, string, int a},
    //    { Image, string, int a},
    //    { Image, string, int a},
    //    { Image, string, int a},
    //    { Image, string, int a},
    //    { Image, string, int a},
    //    { Image, string, int a},
    //    { Image, string, int a},
    //};


    // --------------- 스킬 슬롯 ---------------

    // 슬롯에 저장된 스킬 이미지
    private Image[] slotSkillIcons = new Image[8];


    // --------------- 캐릭터 정보 ---------------

    // 선택된 캐릭터 코드
    private string selectedClassCode;

    // 선택된 캐릭터 코드 이전 값
    private string previousSelectedClassCode;


    // ------------------------------ UI 초기화 ------------------------------

    private void Start()
    {
        // --------------- 스킬 목록 ---------------

        // 스킬 계열 텍스트 오브젝트 연결
        //for (int i = 0; i < 4; i++)
        //{
        //    skillCategoryTexts[i] = GameObject.Find($"Category_{i + 1}_Skill_Text").GetComponent<TMP_Text>();
        //}

        // 스킬 이미지 오브젝트 연결
        //for (int category = 1; category <= 4; category++)
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        categorySkillIconsByCategory[category][i] = GameObject.Find($"Category_{category}_Skill_{i + 1}").GetComponent<Image>();
        //    }
        //}


        // --------------- 스킬 슬롯 ---------------

        // 슬롯에 저장된 스킬 이미지 오브젝트 연결
        for (int i = 0; i < 8; i++)
        {
            slotSkillIcons[i] = GameObject.Find($"Slot_Skill_{i + 1}").GetComponent<Image>();
        }


        // --------------- 캐릭터 정보 초기화 ---------------

        // 선택된 캐릭터 코드 초기화
        selectedClassCode = Character_Select.selectedClassCode;

        // 선택된 캐릭터 코드의 초기 이전 값 설정
        previousSelectedClassCode = selectedClassCode;


        // --------------- 스킬 정보 업데이트---------------

        // 모든 스킬 정보 업데이트
        UpdateAllSkillInfo(selectedClassCode);
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

            // 이전 selectedClassCode 값을 현재 값으로 업데이트
            previousSelectedClassCode = selectedClassCode;
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 스킬 계열 텍스트 업데이트 메서드
    //private void UpdateSkillCategoryTexts(string selectedClassCode)
    //{
    //    if (skillCategoriesByClassCode.TryGetValue(selectedClassCode, out string[] categories))
    //    {
    //        for (int i = 0; i < skillCategoryTexts.Length; i++)
    //        {
    //            skillCategoryTexts[i].text = categories[i];
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("알 수 없는 클래스 코드: " + selectedClassCode);
    //    }
    //}

    // 계열별 스킬 이미지 업데이트 메서드
    private void UpdateCategorySkillIcons()
    {

    }

    // 공용 스킬 이미지 업데이트 메서드
    private void UpdateCommonSkillIcons()
    {

    }

    // 스킬 슬롯 이미지 업데이트 메서드
    private void UpdateSlotSkillIcons()
    {

    }

    // 모든 스킬 정보를 업데이트하는 메서드
    private void UpdateAllSkillInfo(string classCode)
    {
        // classCode가 null이 아닌지 확인
        if (classCode == null)
        {
            Debug.Log("classCode는 null일 수 없습니다.");
            return;
        }

        // 스킬 계열 텍스트 업데이트
        // UpdateSkillCategoryTexts(classCode);

        // 계열별 스킬 이미지 업데이트
        UpdateCategorySkillIcons();

        // 공용 스킬 이미지 업데이트
        UpdateCommonSkillIcons();

        // 스킬 슬롯 이미지 업데이트
        UpdateSlotSkillIcons();
    }
}
