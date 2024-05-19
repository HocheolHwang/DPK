using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillSO : ScriptableObject
{
    [Header("스킬 아이콘")]
    public Sprite Icon; // 스킬 아이콘

    [Header("스킬 종류")]
    public bool IsCollavo; // 콜라보 스킬 여부
    public bool IsCounter; // 카운터 스킬 여부
    public Define.SkillType SkillType; // 스킬 타입

    [Header("스킬 상세 정보")]
    public int RequiredLevel; // 습득 필요 레벨
    public int Damage; // 스킬 피해량
    public float CoolDownTime; // 스킬 쿨타임

    [Header("스킬 기본 정보")]
    public string SkillCode; // 스킬 코드
    public string SkillName; // 스킬 영어 이름
    public string SkillKoreanName; // 스킬 한글 이름
    [TextArea]
    public string SkillDescription; // 스킬 설명

    [Header("콜라보 스킬 정보")]
    public string ConnectedSkillKoreanName; // 콜라보를 함께 발동할 스킬의 한글 이름
    public string CollavoSkillKoreanName; // 콜라보 스킬 한글 이름
    [TextArea]
    public string CollavoSkillDescription; // 콜라보 스킬 설명
}
